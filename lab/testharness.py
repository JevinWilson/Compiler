#!/usr/bin/env python3


EXE = r"C:\Users\jaw06\Desktop\Compiler\lab\bin\Debug\net8.0\lab.exe"



import os
import os.path
import subprocess
import json
import sys

def main():
    global EXE

    base = os.path.abspath(os.path.dirname(__file__))

    print(sys.argv)
    if len(sys.argv) > 1:
        EXE=sys.argv[1]

    if not os.path.exists(f"{base}/tests"):
        print("Could not find tests folder")
        return

    numtests = 0
    alltests=[]
    for dirpath,dirs,files in os.walk(f"{base}/tests/inputs"):
        for f in files:
            if f.endswith(".txt"):
                alltests.append( (dirpath,f) )

    alltests.sort()

    for dirpath,f in alltests:
        print("Testing",f,"...")

        numtests += 1

        resultfile = f"{base}/tests/outputs/{f}"

        try:
            os.unlink("tree.json")
        except FileNotFoundError:
            pass

        rv = run(f"{base}/tests/inputs/{f}")

        print("Parser finished with",f)

        if os.path.exists(resultfile):
            if rv == 0:
                with open(resultfile) as fp:
                    try:
                        expectedJ = json.load(fp)
                    except json.JSONDecodeError as e:
                        print("Error in test harness file",resultfile)
                        print("Badly formatted: At line",e.lineno," column",e.colno,":",e.msg)
                        return
                try:
                    with open("tree.json") as fp:
                        actualJ = json.load(fp)
                except FileNotFoundError:
                    print("Program returned success, but tree.json does not exist")
                    return
                except json.JSONDecodeError as e:
                    print("tree.json is badly formatted: At line",e.lineno," column",e.colno,":",e.msg)
                    return

                ok = compare(expectedJ, actualJ)
                if not ok:
                    print("Tree does not match expected")
                    return

            else:
                print("Problem: Did not parse",f,"but it should have parsed")
                return
        else:
            if rv == 0:
                print("Problem: Parsed",f,"but it should not have")
                return
            else:
                pass

    if numtests == 0 :
        print("Did not run any tests?!")
        return

    print(numtests,"tests passed OK")

    return

def run(fname):
    cmd = [EXE,fname]
    P = subprocess.run(cmd)
    return P.returncode

def compare(J1,J2):
    if J1.get("sym") != J2.get("sym"):
        print("Symbol mismatch:",J1.get("sym"),"vs.",J2.get("sym"))
        return False
    if J1.get("token") != J2.get("token"):
        print("Token mismatch:",J1.get("token"),"vs.",J2.get("token"))
        return False
    c1 = J1.get("children",[])
    c2 = J2.get("children",[])
    if len(c1) != len(c2):
        print("Child length mismatch:",len(c1),"vs.",len(c2))
        return False
    for i in range(len(c1)):
        if not compare(c1[i],c2[i]):
            return False
    return True

main()
input("Press 'enter' to quit")
