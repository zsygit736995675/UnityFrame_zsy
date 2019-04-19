# -*- coding:utf-8 -*-

from __future__ import print_function
import os
import sys
import except_handler
from path_util import *
from config import *
from build_config_byte import *
from RuntimeOS import console

import pck_config
import gen_res_idx
import gen_id
import copy_res
import effect_convertor
import pck_msg
#import build_config_byte

ARG_PACK_CONF_CN = "CN"
ARG_PACK_CONF = "config"
ARG_PACK_EFFECT = "effect"

def build( options = [ARG_PACK_CONF, ARG_PACK_EFFECT] ):
	exphook = sys.excepthook
	sys.excepthook = except_handler._excepthook

	#if len(options) > 2:
	#	doCopyConfig(options[2])
		
	if len(options) > 2:
		doCopyExp(options[2])
		
	#if ARG_PACK_CONF in options:
	#	doGenConfig()
	buildConfig(options,True)
	doPackIndexMap()

	# doPackResIndex()

	print( "copy res begin..." )
	copy_res.copy_res()
	print( "copy res success" )

	sys.excepthook = exphook

def doCopyExp(dstpath):
	print( "copy exp begin..." + dstpath )
	srcpath = EXP_PATH + dstpath
	print( srcpath )
	recurse_copy_folder(srcpath, EXP_PATH, '');
	srcpath = EXP_WEB_PATH + dstpath
	print( srcpath )
	recurse_copy_folder(srcpath, EXP_WEB_PATH, '');

	dstpathBundle = os.path.join(dstpath, 'exp_bundle')
	dstpathIDX = os.path.join(dstpath, 'exp_res_idx')
	
	print( "copy exp success" )
	
def doCopyConfig(dstpath):
	print( "copy config begin..." )
	dstpath = os.path.join(SRC_GAME_CONF_PATH,dstpath)
	
	#clear_folder_type(SRC_GAME_CONF_PATH,XLS_EXT)
	#clear_folder_type(SRC_GAME_CONF_PATH,TXT_EXT_NAME)
	copy_folder_type(dstpath, SRC_GAME_CONF_PATH, XLS_EXT)
	#copy_folder_type(dstpath, SRC_GAME_CONF_PATH, TXT_EXT_NAME)
	print( "copy config success" )
	
def doGenConfig():
	print( "gen config begin..." )
	pck_config.pack_config_dir(SRC_GAME_CONF_PATH,
		   EXP_GAME_CONF_CODE_PATH,
		   EXP_GAME_CONF_BIN_PATH,
		   EXP_GAME_CONF_CSV_PATH)

	clear_folder_type(RES_OTHER_CONFIG_PATH,".bytes")
	copy_folder_type(EXP_GAME_CONF_BIN_PATH, RES_OTHER_CONFIG_PATH, ".bytes")

	#copy to expweb
	clear_folder_type(EXP_WEB_GAME_CONF_CODE_PATH,".cs")
	clear_folder_type(EXP_WEB_GAME_CONF_BIN_PATH,".bytes")
	#clear_folder_type(EXP_WEB_GAME_CONF_CSV_PATH,".csv")
	copy_folder_type(EXP_GAME_CONF_CODE_PATH, EXP_WEB_GAME_CONF_CODE_PATH, ".cs")
	copy_folder_type(EXP_GAME_CONF_BIN_PATH, EXP_WEB_GAME_CONF_BIN_PATH, ".bytes")
	#copy_folder_type(EXP_GAME_CONF_CSV_PATH, EXP_WEB_GAME_CONF_CSV_PATH, ".csv")
	
	print( "gen config success" )

def doPackConfig():
    cmd = "Unity.exe -batchmode -quit -projectPath "
    param = " -executeMethod PackEntry.PackConfig"
    ret = os.system(cmd + RES_OTHER_PATH + param)
    if ret == 0 :
        print( "pack config success" )
    else:
        print( "pack condig fail. maybe some file is open by another process. try kill unity editor." )

def doPackIndexMap():
    print( "pack index map begin..." )
    cmd = "Unity.exe -batchmode -quit -projectPath "
    param = " -executeMethod PackEntry.PackMain"
    ret = os.system(cmd + RES_OTHER_PATH + param)
    if ret == 0:
        print( "pack index map success" )
    else :
        console.Error("There are some erros when running " +cmd + RES_OTHER_PATH + param)
        sys.exit(-100)
    # ResIdxMapFact.bytes 会在 PackMain 的时候生成 Py 中的处理其实没有什么卵用了

def doPackHelp():

	clear_folder_type(RES_OTHER_HELP_PATH,TXT_EXT_NAME)
	recurse_copy_folder_type_to_same_dst(SRC_HELP_PATH,RES_OTHER_HELP_PATH,TXT_EXT_NAME,".svn")

	print( "excute help begin..." )
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackEntry.PackHelper"
	os.system(cmd + RES_OTHER_PATH + param)
	print( "excute help success" )

def doPackResIndex():
	print( "gen res idx begin..." )
	gen_res_idx.gen_res_idx()
	print( "gen res idx success" )

	print( "gen id begin..." )
	gen_id.gen_id()
	print( "gen id success" )

	clear_folder_type(RES_OTHER_INDEXMAP_PATH,INDEX_DST_EXT_NAME)
	copy_folder_type(EXP_RES_CODE_PATH,RES_OTHER_INDEXMAP_PATH,INDEX_DST_EXT_NAME)

	print( "excute indexbundle begin..." )
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackEntry.PackIndex"
	os.system(cmd + RES_OTHER_PATH + param)
	print( "excute indexbundle success" )

if __name__ == '__main__':
	argv = sys.argv
	
	if len(argv) == 1:
		options = [ARG_PACK_CONF, ARG_PACK_EFFECT]
	else:
		options = [ARG_PACK_CONF, ARG_PACK_EFFECT]+argv[1:]
	
	build(options)
