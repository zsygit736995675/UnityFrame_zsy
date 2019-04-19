# -*- coding:utf-8 -*-

import os
import sys
import except_handler
from path_util import *
from config import *

import pck_config
import gen_res_idx
import gen_id
import copy_res
import effect_convertor
import pck_msg

ARG_PACK_CONF = "config"
ARG_PACK_EFFECT = "effect"

def build( options = [ARG_PACK_CONF, ARG_PACK_EFFECT] ):
	exphook = sys.excepthook
	sys.excepthook = except_handler._excepthook

	buildConfig(options)

	sys.excepthook = exphook

def buildConfig(options = [ARG_PACK_CONF, ARG_PACK_EFFECT],bpack = False ):
	if len(options) > 2:
		doCopyConfig(options[2])
		
	doPackConfigResources(bpack)
	
	if len(options) > 2:
		doRemoveConfig(options[2])
		
def doCopyConfig(dstpath):	
	dstpath = os.path.join(SRC_GAME_CONF_PATH,dstpath)	
	#clear_folder_type(SRC_GAME_CONF_PATH,XLS_EXT)
	#clear_folder_type(SRC_GAME_CONF_PATH,TXT_EXT_NAME)
	copy_folder_type(dstpath, SRC_GAME_CONF_PATH, XLS_EXT)
	#copy_folder_type(dstpath, SRC_GAME_CONF_PATH, TXT_EXT_NAME)
	print ("copy config success")

def doRemoveConfig(dstpath):
	dstpath = os.path.join(SRC_GAME_CONF_PATH,dstpath)

	delete_files_from_des_folder(dstpath,SRC_GAME_CONF_PATH)
	
def doPackConfigResources(bpack):
	print ("pck config begin...")
	print("----------------------")
	print(" SRC_GAME_CONF_PATH = ")
	print(SRC_GAME_CONF_PATH)
	print("----------------------")
	print(" EXP_GAME_CONF_CODE_PATH = ")
	print(EXP_GAME_CONF_CODE_PATH)
	print("----------------------")
	print(" EXP_GAME_CONF_BIN_PATH = ")
	print(EXP_GAME_CONF_BIN_PATH)
	print("----------------------")
	print(" EXP_GAME_CONF_CSV_PATH = ")
	print(EXP_GAME_CONF_CSV_PATH)
	print("----------------------")
	pck_config.pack_config_dir(SRC_GAME_CONF_PATH,
							   EXP_GAME_CONF_CODE_PATH,
							   EXP_GAME_CONF_BIN_PATH,
							   EXP_GAME_CONF_CSV_PATH)

							 
	#if bpack:
	#	clear_folder_type(RES_OTHER_CONFIG_PATH,".bytes")
	#	copy_folder_type(EXP_GAME_CONF_BIN_PATH, RES_OTHER_CONFIG_PATH, ".bytes")
	#	clear_folder_type(EXP_WEB_GAME_CONF_CODE_PATH,".cs")
	#	clear_folder_type(EXP_WEB_GAME_CONF_BIN_PATH,".bytes")
	#	copy_folder_type(EXP_GAME_CONF_CODE_PATH, EXP_WEB_GAME_CONF_CODE_PATH, ".cs")
	#	copy_folder_type(EXP_GAME_CONF_BIN_PATH, EXP_WEB_GAME_CONF_BIN_PATH, ".bytes")
	#else:
	#	clear_folder_type(RES_OTHER_CONFIG_PATH,".bytes")
	#	copy_folder_type(EXP_GAME_CONF_BIN_PATH, RES_OTHER_CONFIG_PATH, ".bytes")
	
	print ("pck config success")

if __name__ == '__main__':
	argv = sys.argv

	if len(argv) == 1:
		options = [ARG_PACK_CONF, ARG_PACK_EFFECT]
	else:
		options = [ARG_PACK_CONF, ARG_PACK_EFFECT]+argv[1:]

	build(options)