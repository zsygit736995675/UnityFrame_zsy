# -*- coding:utf-8 -*-

from __future__ import print_function
from path_util import *
from config import *


import os, io, re
import sys
import except_handler

import time

from datetime import *
from VerControlSVN import VerControl
from RuntimeOS import console

#-----------------------------------------------------


def printUseTime(startTime):
    seconds = (datetime.now() - startTime).seconds
    print("use time seconds-------------------" + str(seconds))
    
    
def delLocalNewFile( line ):
    v = re.split(r'^(.)\s+(.*)\r$', line)
    if len(v) != 4:
        console.Error(line+'\n')
        return
    state = v[1]
    path = v[2]
    if state == '?':
        #print("del local new file " + path )
        force_del(path)
        
            
def updateFolder(upPath):
    currDir = os.getcwd()
    svn = VerControl()
    dateTimeStart = datetime.now();
    print("")
    print( "update folder begin..." + os.path.abspath(upPath) )
    svn.revert( upPath, currDir )
    svn.state(upPath, currDir, line_proc = delLocalNewFile)
    svn.update( upPath, currDir )
    print( "update folder end..." )
    printUseTime(dateTimeStart);


def doCommitFolder( line ):
    v = re.split(r'^(.)\s+(.*)\r$', line)
    if len(v) != 4:
        console.Error(line+'\n')
        return
    state = v[1]
    path = v[2]
    currDir = os.getcwd()
    svn = VerControl()
    msg = 'auto ver build commit.'
    if state == '?':
        svn.add(path,currDir)   
    if state == '!':
        svn.rm(path,currDir)
    #svn.commit(path,msg,currDir)
    
    
        
def commitFolder(ciPath):
    currDir = os.getcwd()
    svn = VerControl()
    dateTimeStart = datetime.now();
    print("")
    print( "commit folder begin..." + os.path.abspath(ciPath) )
    svn.state(ciPath, currDir, line_proc = doCommitFolder)
    msg = 'auto ver build commit.'
    svn.commit(ciPath, msg, currDir)
    print( "commit folder end..." )
    printUseTime(dateTimeStart);

#-----------------------------------------------------    
