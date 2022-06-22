#!/usr/bin/env python
# coding: utf-8


import sys
import subprocess


subprocess.check_call([sys.executable, '-m', 'pip', 'install',
'pandas'])
subprocess.check_call([sys.executable, '-m', 'pip', 'install',
'numpy'])
subprocess.check_call([sys.executable, '-m', 'pip', 'install',
'datetime'])


import pandas as pd
import numpy as np
import random as rd
import os
import datetime as dt
print()
print()
print()
print()
print("How many codes do you want to generate?")
numberOfCodes = int(input())

alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z']
codes = []
while len(set(codes))<numberOfCodes:
    for i in range(numberOfCodes):
        letter1 = alphabet[rd.randint(0,25)]
        letter2 = alphabet[rd.randint(0,25)]
        letter3 = alphabet[rd.randint(0,25)]
        number1 = rd.randint(0,9)
        number2 = rd.randint(0,9)
        number3 = rd.randint(0,9)
        number4 = rd.randint(0,9)

        codes.append(f"{letter1}{letter2}{letter3}{number1}{number2}{number3}{number4}")
    




currentPath = os.path.abspath(os.getcwd())
requiredPath = os.path.join(currentPath, "batches")
if not os.path.exists(requiredPath):
    os.mkdir(requiredPath)
currentTime = dt.datetime.now()
fileName = f"{requiredPath}\\codes_{currentTime.year}_{currentTime.month}_{currentTime.day}_{currentTime.hour}{currentTime.minute}{currentTime.second}.csv"




data = pd.DataFrame(codes)
data.to_csv(fileName, index=False, header = False)






