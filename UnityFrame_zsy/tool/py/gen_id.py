# -*- coding:utf-8 -*-

"""generate res index.
	author: zhang shi liang
"""
from __future__ import print_function
import os
import sys
from config import *
from path_util import *
from mako.template import Template
import struct

def parse_anim_name_file(file, name_list):
	fs = open(file, 'rt')
	if not fs:
		raise Exception, "can not open file: %s" % file
		return

	lines = fs.readlines()
	for line in lines:
		line = line.strip()
		if line in name_list:
			raise Exception, "duplicate anim name found: %s in file: %s" % (line, file)
		name_list.append(line)

def parse_res_idx_file(file, name_list):
	fs = open(file, 'rt')
	if not fs:
		raise Exception, "can not open file: %s" % file
		return

	lines = fs.readlines()
	for line in lines:
		key = line.split(':')[0]
		key = key.strip()
		if key in name_list:
			raise Exception, "duplicate res file found: %s" % key
		name_list.append(key)

def gen_anim_id(src_folder, ext_name, name_list):
	if not os.path.exists(src_folder):
		print( "this anim name folder do not exist: %s" % src_folder, file=sys.stderr )
		return

	file_list = os.listdir(src_folder)
	for file in file_list:
		if os.path.splitext(file)[1] == ext_name:
			parse_anim_name_file(os.path.join(src_folder, file), name_list)

def gen_asset_res_id(src_folder, ext_name, name_list):
	if not os.path.exists(src_folder):
		print( "this res folder do not exist: %s" % src_folder, file=sys.stderr )
		return

	file_list = os.listdir(src_folder)
	for file in file_list:
		if os.path.splitext(file)[1] == ext_name:
			parse_res_idx_file(os.path.join(src_folder, file), name_list)

def gen_raw_res_id(src_folder, ext_name, name_list):
	if not os.path.exists(src_folder):
		print( "this res folder do not exist: %s" % src_folder, file=sys.stderr )
		return

	file_list = os.listdir(src_folder)
	for file in file_list:
		name = os.path.split(file)[1]
		if os.path.splitext(name)[1] == ext_name:
			if name in name_list:
				raise Exception, "duplicate raw file found: %s" % name
			name_list.append(file)

def gen_scene_res_id(src_folder, ext_name, name_list):
	if not os.path.exists(src_folder):
		print( "this scene folder do not exist: %s" % src_folder, file=sys.stderr )
		return

	file_list = os.listdir(src_folder)
	for file in file_list:
		file_name = os.path.split(file)[1]
		name, ext = os.path.splitext(file_name)
		if ext == ext_name:
			if name in name_list:
				raise Exception, "duplicate scene found: %s" % name
			name_list.append(name)

def gen_bundle_id(src_folder, ext_name, name_list):
	if not os.path.exists(src_folder):
		print( "this res folder do not exist: %s" % src_folder, file=sys.stderr )
		return

	file_list = os.listdir(src_folder)
	for file in file_list:
		name = os.path.split(file)[1]
		if os.path.splitext(name)[1] == ext_name:
			if name in name_list:
				raise Exception, "duplicate bundle file found: %s" % name
			name_list.append(file)

def exp_src_code(file, name_list, template):
	if template:
		fs = open(file, "wb")
		if not fs:
			raise Exception, "can not open file: %s" % file
			return

		#template = Template(filename = template)
		#name = os.path.splitext(os.path.split(file)[1])[0]
		#txt = template.render(class_name = name, name_list=name_list)
		#txt = txt.replace('\n', '')
		fs.write(struct.pack('<' + 'i', len(name_list)))
		for txt in name_list:
			ids = "ID_"+txt.replace('.', '_').upper()+"-"+ txt
		#	fs.write(ids+"\n")
			if len(ids) > 0:
				fs.write(struct.pack('<' + 'h', len(ids)))
				fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
				fs.flush()
		fs.flush()
		fs.close()
	else:
		fs = open(file, "w")
		if not fs:
			raise Exception, "can not open file: %s" % file
			return

		for txt in name_list:
			ids = "ID_"+txt.replace('.', '_').upper()+"-"+ txt
			fs.write(ids)
			fs.write("\n")
		fs.flush()
		fs.close()

def copyindexfile(srcpath,despath,ext_name):
	clear_folder_type(despath,ext_name)
	copy_folder_type(srcpath,despath,ext_name)

def gen_id():
#    anim_name_list = []
#    gen_anim_id(EXP_ANIM_NAME_PATH, ANIM_NAME_EXT_NAME, anim_name_list)

	res_name_list = []
	gen_scene_res_id(EXP_SCENE_PATH, SCENE_EXT_NAME, res_name_list)
	gen_asset_res_id(EXP_RES_IDX_PATH, PREFAB_IDX_EXT_NAME, res_name_list)
	gen_asset_res_id(EXP_RES_IDX_PATH, TEXTURE_IDX_EXT_NAME, res_name_list)
	gen_asset_res_id(EXP_RES_IDX_PATH, TXT_IDX_EXT_NAME, res_name_list)

	bundle_name_list = []
	gen_bundle_id(EXP_BUNDLE_PATH, BUNDLE_EXT_NAME, bundle_name_list)

#    code_file = os.path.join(EXP_RES_CODE_PATH, EXP_ANIM_ID_FILE)
#    exp_src_code(code_file, anim_name_list, os.path.join(MAKO_PATH, GEN_ID_MAKO_CS))

	code_file = os.path.join(EXP_RES_CODE_PATH, EXP_RES_ID_FILE)
	exp_src_code(code_file, res_name_list, True)

	code_file = os.path.join(EXP_RES_CODE_PATH, EXP_BUNDLE_ID_FILE)
	exp_src_code(code_file, bundle_name_list,True)

	#code_file = os.path.join(EXP_RES_CODE_PATH, EXP_RES_ID_FILE)
	#exp_src_code(code_file, res_name_list, os.path.join(MAKO_PATH, GEN_ID_MAKO_CS))

	code_file = os.path.join(EXP_RES_CODE_PATH, EXP_BUNDLE_ID_TXT)
	exp_src_code(code_file, bundle_name_list, False)
#	copyindexfile(EXP_RES_CODE_PATH,RES_INDEX_MAP_PATH,INDEX_DST_EXT_NAME)

if __name__ == '__main__':
	print( "gen id begin..." )
	gen_id()
	print( "gen id successful" )
