#!/usr/bin/env python3


EXE = r"c:\users\bob\lab.exe"



import os
import os.path
import subprocess
import json
import sys


def main():
    global EXE
    stopOnFirstFail=False

    base = os.path.abspath(os.path.dirname(__file__))

    verbose=True

    i=1
    while i < len(sys.argv):
        if sys.argv[i] == '-q':
            verbose=False
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

    stats = {
        "boolean": [0,0],
        "float": [0,0],
        "int": [0,0]
    }


    for dirpath,f in alltests:
        print("Testing",f,"...")

        kind = f.split("-")[0]

        compilerstatus, exestatus = run(f"{base}/tests/{f}",verbose)

        expfile = f.replace(".txt",".expected.txt")
        expfile = os.path.join(dirpath,expfile)

        with open(expfile) as fp:
            expected = fp.read().strip()

        if expected == "invalid":
            if compilerstatus == 0:
                print("Compiler accepted invalid input [BAD]")
                stats[kind][1]+=1
            else:
                print("Compiler rejected invalid input [OK]")
                stats[kind][0] += 1
        else:
            if expected == "True":
                expected = 1
            elif expected == "False":
                expected = 0
            else:
                assert 0
            if compilerstatus != 0:
                print("Compiler rejected valid input [BAD]")
                stats[kind][1] += 1
            else:
                if exestatus != expected:
                    print(f"Executable returned {exestatus} but it should have returned {expected} [BAD]")
                    stats[kind][1] += 1
                else:
                    print("OK!")
                    stats[kind][0] += 1


    #end loop

    for kind in stats:
        lbl=f"{kind} inputs:"
        print(f"{lbl:20s}{stats[kind][0]} passed     {stats[kind][1]} failed")


    return

def run(fname,verbose):
    cmd = [EXE,fname]
    P = subprocess.run(cmd, stdout=None if verbose else subprocess.DEVNULL,
                            stderr=None if verbose else subprocess.DEVNULL)
    compilerstatus =  P.returncode
    if compilerstatus != 0:
        return compilerstatus,None
    P = subprocess.run( [os.path.join(".","out.exe")] )
    exestatus = P.returncode
    return compilerstatus,exestatus

main()
#input("Press 'enter' to quit")
