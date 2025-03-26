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

    numpassed=0
    numfailed=0

    for dirpath,f in alltests:
        print("Testing",f,"...")

        resultfile = f"{base}/tests/outputs/{f}"

        try:
            os.unlink("tree.json")
        except FileNotFoundError:
            pass

        exitcode,output = run(f"{base}/tests/inputs/{f}")

        print("Parser finished with",f)

        with open(resultfile) as fp:
            try:
                expected = json.load(fp)
            except json.JSONDecodeError as e:
                print("Error in test harness file",resultfile)
                print("Badly formatted: At line",e.lineno," column",e.colno,":",e.msg)
                return

        if expected["returncode"] == 0:
            if exitcode != 0:
                print("Parser returned an error, but it should have parsed the file successfully")
                numfailed+=1
            else:
                if compare(expected["declarations"],output):
                    print("OK!")
                    numpassed += 1
                else:
                    numfailed += 1
        else:
            if exitcode == 0:
                print("Parser accepted the file, but it should have returned an error")
                numfailed+=1
            else:
                numpassed += 1


    numtests = numpassed + numfailed
    if numtests == 0 :
        print("Did not run any tests?!")
        return

    print(numpassed,"of",numtests,"tests passed OK")
    print(numfailed,"of",numtests,"tests failed")

    return

def run(fname):
    cmd = [EXE,fname]
    P = subprocess.Popen(cmd,stdout=subprocess.PIPE)
    o,e = P.communicate()
    o=o.decode()
    o=o.strip()
    if o:
        print(o)
    return P.returncode, o

def compare(expected, output):
    classes=set()
    funcs=set()
    for line in output.split("\n"):
        if line.startswith("CLASS:"):
            name = line[6:].strip()
            classes.add(name)
        elif line.startswith("FUNC:"):
            name = line[5:].strip()
            funcs.add(name)

    ec = set(expected["classes"])
    ef = set(expected["funcs"])

    ok=True

    if ec != classes:
        print("Mismatch for classes:")
        print("Expected:", sorted(list(ec)))
        print("Got:     ", sorted(list(classes)))
        ok=False

    if ef != funcs:
        print("Mismatch for functions:")
        print("Expected:", sorted(list(ef)))
        print("Got:     ", sorted(list(funcs)))
        ok=False

    return ok

main()
input("Press 'enter' to quit")
