#!/usr/bin/env python3


EXE = r"C:\Users\jaw06\Desktop\Compiler\lab\bin\Debug\net8.0\lab.exe"



import os
import os.path
import subprocess
import json
import sys


def main():
    global EXE
    stopOnFirstFail=False

    base = os.path.abspath(os.path.dirname(__file__))

    i=1
    while i < len(sys.argv):
        if sys.argv[i] == "--stop":
            stopOnFirstFail=True
            sys.argv.pop(i)
        else:
            i+=1

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

        exitcode = run(f"{base}/tests/inputs/{f}")

        print("Parser finished with",f)

        with open(resultfile) as fp:
            try:
                expected = json.load(fp)
            except json.JSONDecodeError as e:
                print("Error in test harness file",resultfile)
                print("Badly formatted: At line",e.lineno," column",e.colno,":",e.msg)
                return

        if os.path.exists("tree.json"):
            with open("tree.json") as fp:
                actual = json.load(fp)
        else:
            actual=None


        if expected["returncode"] == 0:
            if exitcode != 0:
                print("Parser returned an error, but it should have parsed the file successfully")
                numfailed+=1
            else:
                if compare(expected["tree"],actual,0):
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
        #endif

        if stopOnFirstFail and numfailed>0:
            print("At least one test failed. Stopping.")
            break
    #end loop

    numtests = numpassed + numfailed
    if numtests == 0 :
        print("Did not run any tests?!")
        return

    print(numpassed,"of",numtests,"tests passed OK")
    print(numfailed,"of",numtests,"tests failed")

    return

def run(fname):
    cmd = [EXE,fname]
    P = subprocess.run(cmd)
    return P.returncode

def compare(expected, actual, depth):

    if expected["sym"] != actual["sym"]:
        print("At tree depth",depth,": Symbol mismatch")
        print("Expected:",expected["sym"])
        print("Got:     ",actual["sym"])
        return False

    #ignore types for terminals
    if expected["sym"].upper() != expected["sym"] and expected["nodeType"] != actual["nodeType"]:
        print("At tree depth",depth,": Type mismatch")
        print("Expected:",expected["nodeType"])
        print("Got:     ",actual["nodeType"])
        return False

    if len(expected["children"]) != len(actual["children"]):
        print("At tree depth",depth,": Children length mismatch")
        ec = [ q["sym"] for q in expected["children"] ]
        ec = ",".join(ec)
        ac = [ q["sym"] for q in actual["children"] ]
        ac = ",".join(ac)
        print("Expected:",expected["sym"],"with",len(expected["children"]),"children (",ec,")")
        print("Got:     ",actual["sym"],"with",len(actual["children"]),"children (",ac,")")
        return False

    if expected["varInfo"] != actual["varInfo"]:
        print("varInfo mismatch in node with symbol",expected["sym"])
        print("Expected:",expected["varInfo"])
        print("Got:     ",actual["varInfo"])
        return False

    for i in range(len(expected["children"])):
        if not compare( expected["children"][i], actual["children"][i], depth+1):
            return False
    return True


main()
input("Press 'enter' to quit")
