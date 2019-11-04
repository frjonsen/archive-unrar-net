#! /usr/bin/python3
import argparse
import subprocess
import sys
import os.path
import os

parser = argparse.ArgumentParser()
parser.add_argument("--size", '-S', metavar="size", type=int)
parser.add_argument("--root-path", "-R", metavar="root")
parser.add_argument("--save-path", "-P", metavar="save_path")

args = parser.parse_args()

path = os.path.expanduser("~/unpack.log")
unarchive_log = os.path.expanduser("~/unpack_unarchive.log")
with open(path, "a") as f:
    f.write(f"Unpacking {args.root_path}, size {args.size}\n")
    f.write(f"Save path was {args.save_path}\n")
    f.write(f"TV Env is {os.environ.get('TV')}\n\n")
    f.flush()

    five_gb = 5000000000

    if not args.save_path.endswith("Downloads/"):
        f.write("Save path does not end in Downloads/, assuming already unpacked\n")
        sys.exit(0)

    if (args.size > five_gb):
        f.write("Torrent was above 5GB, not unpacking\n")
        sys.exit(0)

    with open(unarchive_log, "a") as u, subprocess.Popen("unarchive", cwd=args.root_path, shell=True, universal_newlines=True, stdout=subprocess.PIPE) as p:
        f.write("Start to unpack\n")
        for line in p.stdout:
            print(line)
            u.write(line)
        u.flush()
        p.wait()