#!/usr/bin/env python3


EXE = r"C:\Users\jaw06\Desktop\Compiler\lab\bin\Debug\net8.0\lab.exe"


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


    regularPassed=0
    regularFailed=0
    bonusPassed=0
    bonusFailed=0
    regularTests=0
    bonusTests=0

    def bonusgood(msg):
        nonlocal regularPassed,regularFailed,bonusPassed,bonusFailed,regularTests,bonusTests
        bonusPassed+=1
        bonusTests += 1
        print("Test passed as bonus:",msg)

    def bonusbadregulargood(msg1,msg2):
        nonlocal regularPassed,regularFailed,bonusPassed,bonusFailed,regularTests,bonusTests
        bonusFailed+=1
        regularPassed+=1
        bonusTests+=1
        regularTests+=1
        print("Test failed as bonus but passed as non-bonus:",msg1,msg2)

    def regulargood(msg):
        nonlocal regularPassed,regularFailed,bonusPassed,bonusFailed,regularTests,bonusTests
        regularPassed+=1
        regularTests+=1
        print("Test passed:",msg)

    def regularbad(msg):
        nonlocal regularPassed,regularFailed,bonusPassed,bonusFailed,regularTests,bonusTests
        regularTests += 1
        regularFailed+=1
        print("Test failed:",msg)
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


        #if there's no regular or bonus notation,
        #test is regular
        if "regular" not in expected and "bonus" not in expected:
            expected={ "regular": expected }


        try:
            #some "returns" items were mistakenly stored as strings.
            v = int(expected["regular"]["returns"])
            expected["regular"]["returns"]=v
        except ValueError:
            pass


        compilerstatus, exestatus, exereturntype, exestdout, exestderr = run(f"{base}/tests/{f}",expected.get("input"),verbose)

        #we have several possibilities:
        #   1. This is a test that must be passed. Same results for bonus
        #       or non-bonus.
        #       Keys: returns, input, output
        #   2. This is a test that has one result for bonus and a different one for non-bonus
        #       Keys: regular, bonus [dictionaries]
        #           regular & bonus have returns, input, output
        #   3. This is a test that must be passed for bonus; ignored for non-bonus
        #       Keys: bonus


        if "regular" in expected and "bonus" not in expected:
            #this is a "must-pass" test
            passed,msg = check(compilerstatus, exestatus, exereturntype, exestdout, exestderr,
                expected["regular"])
            if passed:
                regulargood(msg)
            else:
                regularbad(msg)
        elif "bonus" in expected and "regular" not in expected:
            passed = check(compilerstatus, exestatus, exereturntype, exestdout, exestderr,
                expected["bonus"])
            if passed:
                bonusgood()
            else:
                bonusbad(msg)
        elif "regular" in expected and "bonus" in expected:
            #two possible outputs
            passed = check(compilerstatus, exestatus, exereturntype, exestdout, exestderr,
                expected["bonus"])
            if passed:
                bonusgood(msg)
            else:
                passed,msg2 = check(compilerstatus, exestatus, exereturntype, exestdout, exestderr,
                    expected["regular"])
                if passed:
                    bonusbadregulargood(msg,msg2)
                else:
                    regularbad(msg2)
        else:
            assert 0

    #end loop

    print("Non-bonus:",regularPassed,"passed;",regularFailed,"failed; total=",regularTests)
    print("Bonus:",bonusPassed,"passed;",bonusFailed,"failed; total=",bonusTests)

    return

def escape(s):
    r=""
    for c in s:
        if c == '\n':
            r += "<newline>"
        elif c == '\t':
            r += "<tab>"
        else:
            r += c
    return r

def check( compilerstatus, exestatus, exereturntype, exestdout, exestderr,
            expected):

    if type(expected["returns"]) == int:
        expectedreturntype = RETURN
    else:
        assert expected["returns"] in [INVALID,HANG,CRASH],f"{expected}"
        expectedreturntype = expected["returns"]


    if expected["returns"] == INVALID:
        if compilerstatus == 0:
            return False,"Compiler accepted invalid input"
        else:
            return True,"Compiler rejected invalid input"
    else:
        if compilerstatus != 0:
            return False,"Compiler rejected valid input"
        else:
            if exereturntype != expectedreturntype:
                return False, f"Bad return type: Executable {past(exereturntype,exestatus)}, but we expected it to {present(expectedreturntype,expected["returns"])}"
            else:
                if expectedreturntype == RETURN and exestatus != expected["returns"]:
                    return False,f"Bad return value: Executable {past(exereturntype,exestatus)}, but we expected it to {present(expectedreturntype,expected["returns"])}"
                else:
                    if expected.get("output") and exestdout != expected["output"]:
                        return False,(
                            f"Executable printed incorrect output\n"
                            f"Expected: {escape(expected.get('output'))}\n"
                            f"Got:      {escape(exestdout)}"
                        )
                    else:
                        return True,"OK!"

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
