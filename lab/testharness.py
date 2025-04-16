#!/usr/bin/env python3


EXE = r"c:\users\bob\lab.exe"


TIMEOUT=2


CRASH="crash"
HANG="hang"


import os
import os.path
import subprocess
import json
import sys


def past(status):
    if type(status) == int:
        return f"returned {status}"
    elif status == CRASH:
        return "crashed"
    elif status == HANG:
        return "hung"
    else:
        return f"{status}"  #we shouldn't get here?
def present(status):
    if type(status) == int:
        return f"return {status}"
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
            if f.endswith(".txt") and "expected" not in f:
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

        kind = f.split("-")[0]

        compilerstatus, exestatus, exeerror = run(f"{base}/tests/{f}",verbose)

        if exeerror != None:
            exestatus = exeerror

        expfile = f.replace(".txt",".expected.txt")
        expfile = os.path.join(dirpath,expfile)

        with open(expfile) as fp:
            expected = fp.read().strip()

        if expected == "invalid":
            if compilerstatus == 0:
                bad("Compiler accepted invalid input")
            else:
                good("Compiler rejected invalid input")
        else:
            if expected == CRASH:
                verb=""
            elif expected == HANG:
                verb=""
            else:
                expected = int(expected)
                verb="return "

            if compilerstatus != 0:
                bad("Compiler rejected valid input")
            else:
                if exestatus == expected:
                    good(f"Executable {past(exestatus)}, which was expected")
                else:
                    bad(f"Executable {past(exestatus)}, but we expected it to {present(expected)}")
    #end loop

    print(f"{stats[GOOD]} passed     {stats[BAD]} failed")


    return

def run(fname,verbose):
    #returns a tuple: compiler status, executable status, exe error
    #   cstatus     estatus     eerror  meaning
    #   0           0..255      None    Compiled OK; executable returned 0...255
    #   != 0        None        None    Did not compile; executable not run
    #   0           None        CRASH   Compiled OK; executable crashed
    #   0           None        HANG    Compiled OK; executable hung

    cmd = [EXE,fname]
    P = subprocess.run(cmd, stdout=None if verbose else subprocess.DEVNULL,
                            stderr=None if verbose else subprocess.DEVNULL)
    compilerstatus =  P.returncode
    if compilerstatus != 0:
        return compilerstatus,None,None
    try:
        P = subprocess.run( [os.path.join(".","out.exe")], timeout=TIMEOUT )
    except subprocess.TimeoutExpired:
        return compilerstatus,None,HANG

    exestatus = P.returncode
    if exestatus < 0:
        #posix: Signal; simulate a Windows error
        exestatus = 0x40000000
    exestatus &= 0xffffffff
    if exestatus >= 0x40000000 and exestatus <= 0xfffffc00:
        return compilerstatus,None,CRASH

    return compilerstatus,exestatus,None

main()
#input("Press 'enter' to quit")
