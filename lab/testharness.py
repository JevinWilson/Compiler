#!/usr/bin/env python3

import struct 

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
    verbose=False
    
    i=1
    while i < len(sys.argv):
        if sys.argv[i] in ["-k", "--stop"]:
            stopOnFirstFail=True
            sys.argv.pop(i)
        elif sys.argv[i] in ["-v","--verbose"]:
            verbose=True
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
    for dirpath,dirs,files in os.walk(f"{base}/tests"):
        for f in files:
            if f.endswith(".txt"):
                alltests.append( (dirpath,f) )

    alltests.sort()

    numpassed=0
    numfailed=0

    def good():
        nonlocal numpassed,numfailed
        numpassed+=1
        print("OK!")
    def bad(reason):
        nonlocal numpassed,numfailed
        numfailed+=1
        print("BAD!",reason)
        if stopOnFirstFail:
            sys.exit(1)
            
    for dirpath,f in alltests:
        print("Testing",f,"...")

        with open(f"{dirpath}/{f}") as fp:
            idata = fp.read()
        data=idata
        i = data.find("return")
        data = data[i+6:]
        data = data.split("\n")[0]
        i = data.find("//")
        if i != -1:
            data = data[:i]
        data=data.strip()
        
        
        if verbose:
            print(idata)
            print("~"*40)

        expected = (eval(data))
        if type(expected) == float:
            expected = struct.unpack( "Q", struct.pack( "d", expected ) )[0]
        expected = expected & 0xffffffff
        
        if verbose:
            print(expected)
            print("="*40)
            continue
        
        
        compiledok = run(EXE,f"{dirpath}/{f}")
        if compiledok != 0:
            bad("Compiler rejected input, but it should not have")
        exitcode = run("./out.exe")
        print("Got exit code",exitcode)
        
        acceptable = [expected,expected&0xff]
        if exitcode in acceptable:
            good()
        else:
            tmp = [str(q) for q in acceptable]
            tmp = ", ".join(tmp)
            bad("Expected return code to be one of: "+tmp)
                
    #end loop

    numtests = numpassed + numfailed
    if numtests == 0 :
        print("Did not run any tests?!")
        return

    print(numpassed,"of",numtests,"tests passed OK")
    print(numfailed,"of",numtests,"tests failed")

    return

def run(*cmd):
    P = subprocess.run(cmd)
    return P.returncode

main()
#input("Press 'enter' to quit")
