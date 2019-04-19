# -*- coding:utf-8 -*-
# Author : zhaoyin

from __future__ import print_function
from path_util import *
from config import *
from build_config_byte import *

import os, io, re, sys, time
from datetime import *

import except_handler
import pck_config
import copy_res

import Newbuild
import Newbuildhelper
import diff_bundle

from VerControlSVN import VerControl
from tools import *
from RuntimeOS import SubProcess, console

#-----------------------------------------------------

def build( optionsPath = "" , isPublish = False):
    exphook = sys.excepthook
    sys.excepthook = except_handler._excepthook
    
    dateTimeStart = datetime.now();
    print( "start build res ..." + optionsPath)
    
    currDir = os.getcwd()
    svn = VerControl()
    svn.cleanup(os.path.abspath(os.path.join(ROOT_PATH, '../')), currDir)
    updateFolder(SRC_PATH)
    buildMsg(optionsPath)
    
    readyExpFile(optionsPath)

    updateFolder(SRC_GAME_CONF_PATH)
    configBuild(optionsPath)
    
    updateFolder(SRC_HELP_PATH)
    helperBuild(optionsPath)
    
    updateFolder(CLIENT_BIN_RES_PATH)
    updateFolder(CLIENT_BIN_WEB_PATH)
    packBundleToBin(optionsPath, isPublish)
            
    updateFolder(DST_GAME_CONF_CODE_PATH)
    copyRes(optionsPath, isPublish)
    
    print("")
    print( "end build res ..." + optionsPath )
    printUseTime(dateTimeStart);
    
    sys.excepthook = exphook

    
#----------------------------------------------------- 
def buildMsg(optionsPath):
    dateTimeStart = datetime.now();   
    print("")
    rundir = os.path.abspath(SHARED_PWMSG_PATH)
    cmd = [os.path.join(rundir,'AutoBuildMsgToSVN.bat'),]
    print( "build msg begin..." + rundir)
    def outline( line ):
        print( line, end='' )
    p = SubProcess(cmd, rundir, outline, shell=True)
    p.run()
    p.wait()
    if p.returncode() != 0:
        print( 'ERROR : BuildMsg failed.', file=sys.stderr )
        print( p.returncode() )
    print( "build msg end..." )    
    printUseTime(dateTimeStart);
    
    
def readyExpFile(optionsPath):
    print("")
    print( "ready exp begin..." )
    updateFolder(EXP_PATH);
    updateFolder(EXP_WEB_PATH);
    if("" != optionsPath):
        srcpath = EXP_PATH + optionsPath
        updateFolder(srcpath);
        recurse_copy_folder(srcpath, EXP_PATH, '');
        srcpath = EXP_WEB_PATH + optionsPath
        updateFolder(srcpath);
        recurse_copy_folder(srcpath, EXP_WEB_PATH, '');
    print("")
    print( "ready exp end..." )


def configBuild(optionsPath):
    dateTimeStart = datetime.now();
    print("")
    print( "config build begin..." )
    optionsPathArray = [ARG_PACK_CONF, ARG_PACK_EFFECT ,optionsPath]
    buildConfig(optionsPathArray,True)
    print( "config build end..." )
    printUseTime(dateTimeStart);
    
    
def helperBuild(optionsPath):
    dateTimeStart = datetime.now();
    print("")
    print( "helper build begin..." )
    Newbuildhelper.doPackHelp()
    print( "helper build end..." )
    printUseTime(dateTimeStart);
    
    
def packBundleToBin(optionsPath, isPublish = False):
    dateTimeStart = datetime.now();
    print("")
    print( "Pack bundle begin..." )
    diff_bundle.DiffBundle(Newbuild.doPackIndexMap)
    if not isPublish:
        currDir = os.getcwd()
        msg = 'auto ver build commit.'
        svn = VerControl()
        svn.add(os.path.join(DST_RES_PATH, BUNDLEMAP_FILE),currDir)
        svn.commit(os.path.join(DST_RES_PATH, BUNDLEMAP_FILE),msg,currDir)
        svn.add(os.path.join(DST_WEB_RES_PATH, BUNDLEMAP_FILE),currDir)
        svn.commit(os.path.join(DST_WEB_RES_PATH, BUNDLEMAP_FILE),msg,currDir)
    print( "Pack bundle end..." )
    printUseTime(dateTimeStart);
    
'''
 函数功能说明:拷贝资源
 参数说明:
 optionsPath: 版本类型CN、QQ等等
 isPublish: 布尔类型 是否是打版的时候调用
'''
def copyRes(optionsPath, isPublish):
    dateTimeStart = datetime.now();
    print("")
    print( "copy Config CS begin..." )
    copy_res.copy_res()
    currDir = os.getcwd()
    msg = 'auto ver build commit.'
    svn = VerControl()
    if not isPublish:
        commitFolder(CLIENT_BIN_RES_PATH)
        commitFolder(CLIENT_BIN_WEB_PATH)
    
    commitFolder(DST_GAME_CONF_CODE_PATH)
    print( "copy Config CS end..." )
    printUseTime(dateTimeStart);

#-----------------------------------------------------
    
if __name__ == '__main__':
    argv = sys.argv
    #argv = ['AutoBuildToSvn.py', 'CN', '-p'];
    #To check wheter function be called when publish game
    isPublish = False
    if len(argv) == 3 and (argv[2] == '-p' or argv[2] == '-P'):
        isPublish = True

    if len(argv) == 1:
        optionsPath = ""
    else:
        optionsPath = str(argv[1])

    isPublish = True;
    build(optionsPath, isPublish)
