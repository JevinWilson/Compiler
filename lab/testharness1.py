import os
import subprocess
import json
import sys

# Path to compiled C# executable
EXEC = r"lab/bin/Debug/net8.0/lab.exe"

def run_test(test_file):
  
    test_name = test_file.split(".")[0]
    expected_file = f"testcases/result-{test_file}"
    
    with open(expected_file, "r") as f:
        expected_output = json.load(f)

    # Run the tokenizer
    process = subprocess.run([EXEC, os.path.join("testcases", test_file)], capture_output=True, text=True)
    
    try:
        actual_output = json.loads(process.stdout.strip())
    except json.JSONDecodeError:
        print(f"{test_name} ... FAIL")
        return False
    
    if actual_output != expected_output:
        print(f"{test_name} ... FAIL")
        return False
    
    print(f"{test_name} ... PASS")
    return True

# Run all tests
all_tests = [f for f in sorted(os.listdir("testcases")) if f.startswith("t") and f.endswith(".txt")]

failures = 0
for test in all_tests:
    if not run_test(test):
        failures += 1

if failures > 0:
    sys.exit(1)
else:
    sys.exit(0)


