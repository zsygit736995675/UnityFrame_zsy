# -*- coding:utf-8 -*-
# Author : yangtaibao

from __future__ import print_function
from path_util import *
from config import *
from build_config_byte import *

import os, io, re
import multiprocessing
import sys
import except_handler
import pck_config
import copy_res

import time
import Newbuild
import Newbuildhelper
import diff_bundle
from AutoBuildToSvn import *
from tools import *

from datetime import *
from VerControlSVN import VerControl
from RuntimeOS import console
from macpath import curdir

def outputLog():
    logFile = open(os.path.join(TOOL_PATH, "pack.log"), "r")
    logData = logFile.read()
    logFile.close()
    console.Error(logData)

def prepareWork(versions = []):
    #clean up current root directory to avoid svn locked whole directory
    print("Run svn cleanup "+os.path.abspath(os.path.join(ROOT_PATH, '../')));
    currDir = os.getcwd()
    svn = VerControl()
    svn.cleanup(os.path.abspath(os.path.join(ROOT_PATH, '../')), currDir)
    print("clean up end...");

    #svn revert and update for src/project files
    timeStart = datetime.now();
    ''' --*-- Program just output file to this directory, so don't need to svn update --*--
    #Updating packing program output directory
    updateFolder(EXP_WEB_PATH);
    for v in versions:
        updateFolder(EXP_WEB_PATH + v)
        updateFolder(EXP_PATH + v)
    updateFolder(EXP_WEB_PATH+"CN"); 
    '''
    updateFolder(RES_PROJECT_PATH);
    printUseTime(timeStart);

def packImpl(optionSrcs):
    currDir = os.getcwd()
    cmd = "Unity.exe -batchmode -quit -projectPath "
    
    if ("PackAll" in optionSrcs) or len(optionSrcs) == 0:
        param = " -executeMethod PackEntry.PackAll";
        ret = os.system(cmd + RES_PROJECT_PATH + param)
        if ret == 0:
            print("Pack all source file success!");
        else:
            console.Info(methodName + " Done");                                                 
            print("Pack error, please check log, function name: PackEntry.PackAll");
    else:
        for methodName in optionSrcs:
            timeStart = datetime.now();
            param = " -executeMethod PackEntry."+methodName +" -logFile " + os.path.join(TOOL_PATH, "pack.log")+ "  ";
            ret = os.system(cmd + RES_PROJECT_PATH + param);

            if ret == 0:
                console.Info(methodName + " Done\n");
            else:
                console.Error("There are some errors when running " + methodName+ "\n");
                outputLog()

    
#Using svn to comment file those were packed
def postworkAfterPack(versions = []):

    srcpath = EXP_PATH + versions[0]
    print('copy ' +  srcpath + ' to exp folder' )
    recurse_copy_folder(srcpath, EXP_PATH, '');
    srcpath = EXP_WEB_PATH + versions[0]
    print('copy ' +  srcpath + ' to expweb folder' )
    recurse_copy_folder(srcpath, EXP_WEB_PATH, '');
    commitFolder(EXP_WEB_PATH);
    commitFolder(EXP_PATH);
    for v in versions:
        commitFolder(EXP_PATH + v)
        commitFolder(EXP_WEB_PATH + v)
   
    
def pack( optionSrcs = [], versions = [""]):
    exphook = sys.excepthook
    sys.excepthook = except_handler._excepthook
    #To revert and update related folder before pack source file
    prepareWork(versions);
    #Call Unity.exe to Pack All or optional source files
    timeStart = datetime.now();
    optionList = "";
    counter = 0;
    for idx in range(0, len(optionSrcs)):
        optionList += str(idx)+"--->"+optionSrcs[idx]+"\n";
    console.Info(optionList);
    console.Info("=============start pack res :==================\n")
    packImpl(optionSrcs);
    postworkAfterPack(versions);
    print("")
    print( "end pack res ...")
    printUseTime(timeStart);
    
    sys.excepthook = exphook
#-----------------------------------------------------
    
if __name__ == '__main__':
    options = ["PackCharacteranim", "PackAvatar","PackEffect","PackCharacter",
               "PackAct", "PackCharacterdef", "PackGui","PackGuiAtlas",
               "PackFont","PackGuiAnimation", "PackSound","PackMusic","PackTexture",
               "PackTxt"]
    argv = sys.argv
    #argv = ["AutoPackSrcToSvn", "PackAllOneByOne"]
    versions = [];

    if len(argv) == 1:
        optionSrcs = [];

    elif argv[1] == "PackAllOneByOne":
        optionSrcs = options;
        if len(argv) > 1:
            versions.extend( argv[2:] )
    else:
        versions.extend( sys.argv[1:] )

    pack(optionSrcs, versions)
