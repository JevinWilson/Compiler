import time
import subprocess

start = time.time()
subprocess.run(["./out.exe"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
end = time.time()
print(f"{end - start:.3f} seconds")