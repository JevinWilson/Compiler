#!/usr/bin/env python3


EXE = r"c:\users\bob\lab.exe"


TIMEOUT=2

INVALID="invalid"
CRASH="crash"
HANG="hang"
RETURN="return"

import os
import os.path
import subprocess
import json
import sys


def past(status,code):
    if status == RETURN:
        return f"returned {code}"
    elif status == CRASH:
        return "crashed"
    elif status == HANG:
        return "hung"
    else:
        return f"{status}"  #we shouldn't get here?
def present(status,code):
    if status == RETURN:
        return f"return {code}"
    elif status == CRASH:
        return "crash"
    elif status == HANG:
        return "hang"
    else:
        return f"{status}"  #we shouldn't get here?

def main():
    global EXE
    stopOnFirstFail=False

    base = os.path.abspath(os.path.dirname(__file__))

    verbose=True
    keepgoing=True
    skip=0

    i=1
    while i < len(sys.argv):
        if sys.argv[i] == '-q':
            verbose=False
            del sys.argv[i]
        elif sys.argv[i] == '-k':
            keepgoing=False
            del sys.argv[i]
        elif sys.argv[i] == '-s':
            del sys.argv[i]
            skip=int(sys.argv[i])
            del sys.argv[i]
        else:
            i+=1

    if len(sys.argv) > 1:
        EXE=sys.argv[1]

    if not os.path.exists(f"{base}/tests"):
        print("Could not find tests folder")
        return

    numtests = 0
    alltests=[]
    for dirpath,dirs,files in os.walk(f"{base}/tests"):
        for f in files:
            if f.endswith(".txt") and "results" not in f:
                alltests.append( (dirpath,f) )

    alltests.sort()

    #first=pass, second=fail
    stats = [0,0]
    GOOD=0
    BAD=1

    def good(msg):
        stats[GOOD]+=1
        print("OK: ",msg)
    def bad(msg):
        stats[BAD]+=1
        print("BAD:",msg)
        if not keepgoing:
            sys.exit(1)


    for dirpath,f in alltests:

        if skip > 0:
            print("Skipping",f,"...")
            skip-=1
            continue

        print("Testing",f,"...")

        expfile = f.replace(".txt",".results.txt")
        expfile = os.path.join(dirpath,expfile)

        with open(expfile) as fp:
            expected = json.loads(fp.read().strip())
            try:
                v = int(expected["returns"])
                expected["returns"]=v
            except ValueError:
                pass

            if type(expected["returns"]) == int:
                expected["returntype"] = RETURN
            else:
                assert expected["returns"] in [INVALID,HANG,CRASH],f"{expected}"
                expected["returntype"] = expected["returns"]

        compilerstatus, exestatus, exereturntype, exestdout, exestderr = run(f"{base}/tests/{f}",expected.get("input"),verbose)


        if expected["returns"] == INVALID:
            if compilerstatus == 0:
                bad("Compiler accepted invalid input")
            else:
                good("Compiler rejected invalid input")
        else:
            if compilerstatus != 0:
                bad("Compiler rejected valid input")
            else:
                if exereturntype != expected["returntype"]:
                    bad(f"Bad return type: Executable {past(exereturntype,exestatus)}, but we expected it to {present(expected["returntype"],expected["returns"])}")
                else:
                    if expected["returntype"] == RETURN and exestatus != expected["returns"]:
                        bad(f"Bad return value: Executable {past(exereturntype,exestatus)}, but we expected it to {present(expected["returntype"],expected["returns"])}")
                    else:
                        if expected.get("output") and exestdout != expected["output"]:
                            bad(f"Executable printed incorrect output")
                        else:
                            good("OK!")
    #end loop

    print(f"{stats[GOOD]} passed     {stats[BAD]} failed")


    return

def run(fname,input,verbose):
    #returns a tuple: compiler status, executable status, exe error
    #   cstatus     estatus     eerror  meaning
    #   != 0        None        None    Did not compile; executable not run
    #   0           0..255      None    Compiled OK; executable returned 0...255
    #   0           None        CRASH   Compiled OK; executable crashed
    #   0           None        HANG    Compiled OK; executable hung

    cmd = [EXE,fname]
    P = subprocess.run(cmd, stdout=None if verbose else subprocess.DEVNULL,
                            stderr=None if verbose else subprocess.DEVNULL)
    compilerstatus =  P.returncode
    if compilerstatus != 0:
        return compilerstatus,None,None,b"",b""

    if input:
        input=input.encode()
    try:
        P = subprocess.run( [os.path.join(".","out.exe")],
            timeout=TIMEOUT,
            input=input,
            capture_output=True )
    except subprocess.TimeoutExpired:
        return compilerstatus,None,HANG,P.stdout,P.stderr

    exestatus = P.returncode
    if exestatus < 0:
        #posix: Signal; simulate a Windows error
        exestatus = 0x40000000
    exestatus &= 0xffffffff
    if exestatus >= 0x40000000 and exestatus <= 0xfffffc00:
        return compilerstatus,None,CRASH,P.stdout,P.stderr

    return compilerstatus,exestatus,RETURN,P.stdout.decode(errors="replace"),P.stderr

main()
#input("Press 'enter' to quit")
