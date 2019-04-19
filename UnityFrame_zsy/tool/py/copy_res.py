# -*- coding:utf-8 -*-

import os
import shutil
from config import *
from path_util import *

def copy_setting_only():
	dir = os.getcwd()
	os.chdir(ROOT_PATH)
	
	#copy settings
	shutil.copy(os.path.join(CONF_PATH, "config.txt"), DST_RES_PATH)
	shutil.copy(os.path.join(CONF_PATH, "config.txt"), DST_WEB_RES_PATH)
	
	shutil.copy(os.path.join(CONF_PATH, "UpdateNotice.txt"), DST_RES_PATH)
	shutil.copy(os.path.join(CONF_PATH, "UpdateNotice.txt"), DST_WEB_RES_PATH)
	
	shutil.copy(os.path.join(CONF_PATH, "OpenUrl.txt"), DST_RES_PATH)
	shutil.copy(os.path.join(CONF_PATH, "OpenUrl.txt"), DST_WEB_RES_PATH)
	
	os.chdir(dir)

def copy_res():
	dir = os.getcwd()
	os.chdir(ROOT_PATH)

	#copy res code
	#force_del(DST_GAME_EXP_PATH)
	#clear_folder_exclude_type(DST_RES_CODE_PATH, ".svn")
	#copy_folder_type(EXP_RES_CODE_PATH, DST_RES_CODE_PATH, ".cs")

	#clear res
	#recurse_clear_folder(DST_RES_PATH, [".svn"])

	#copy bundle & scene
	#copy_folder_type(EXP_BUNDLE_PATH, DST_RES_PATH, ".bundle")
	#copy_folder_type(EXP_SCENE_PATH, DST_RES_PATH, ".unity3d")
	#copy_folder_type(EXP_SCENE_PATH, DST_RES_PATH, ".txt")

	#copy settings
	copy_setting_only()

	#copy gui code
	#recurse_clear_folder(DST_GUI_CODE_PATH, [".svn"])
	#recurse_copy_folder_to_same_dst(EXP_GUI_PATH, DST_GUI_CODE_PATH, [".svn"])

	#copy script
	#recurse_clear_folder(DST_SCRIPT_PATH, [".svn"])
	#recurse_copy_folder_to_same_dst(EXP_SCRIPT_PATH, DST_SCRIPT_PATH, [".svn"])

	#copy shader
	#recurse_clear_folder(DST_SHADER_PATH, [".svn"])
	#recurse_copy_folder_to_same_dst(EXP_SHADER_PATH, DST_SHADER_PATH, [".svn"])

	#copy game conf code
	#delete_file_folder(DST_GAME_EXP_PATH)
	#os.mkdir(DST_GAME_EXP_PATH)
	if not os.path.exists(DST_GAME_CONF_CODE_PATH):
		os.mkdir(DST_GAME_CONF_CODE_PATH)
	else:
		clear_folder_exclude_type(DST_GAME_CONF_CODE_PATH, ".svn")
	copy_folder_type(EXP_GAME_CONF_CODE_PATH, DST_GAME_CONF_CODE_PATH, ".cs")


	os.chdir(dir)

   #CalculateSize(DST_RES_PATH,[".bundle",".unity3d"])

def CalculateSize(path,type):
	allfile = {}
	for root,dir,files in os.walk(path):
		for onefile in files:
			if os.path.splitext(onefile)[1] == type[0] or os.path.splitext(onefile)[1] == type[1]:
				fname = os.path.join(root,onefile)
				fsize = os.stat(fname).st_size
				if allfile.has_key(fname):
					allfile[fname] = fsize
				else:
					allfile[fname] = fsize
	for x in allfile.keys():
		print (x,allfile[x])

if __name__ == '__main__':
	print "copy res begin..."
	copy_res()
	print "copy res successful"
