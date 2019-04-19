# -*- coding:utf-8 -*-

"""generate res index.
	author: zhang shi liang
"""
from __future__ import print_function
import os
import sys
from config import *
from mako.template import Template
import struct

g_scene_map = {}
g_prefab_map = {}
g_texture_map = {}
g_txt_map = {}
#g_collision_map = {}

def parse_idx_file(file, map):
	fs = open(file, 'rt')
	if not fs:
		raise Exception, "can not open file: %s" % file
		return

	lines = fs.readlines()
	for line in lines:
		key, value = line.split(':')
		key = key.strip()
		value = value.strip()
		if key in map:
			print( 'Parse IDX file: %s' % (file, ), file=sys.stderr )
			print( 'Duplicate file found : %s' % ( key, ), file=sys.stderr )
			# raise Exception, "duplicate file found: %s" % key
		else:
			map[key] = [value,CalculateSize(os.path.join(EXP_BUNDLE_PATH,value))]

def CalculateSize(onefile):
	fsize = 0
	fsize = os.stat(os.path.abspath(onefile)).st_size
	return fsize

def gen_asset_bundle_map(src_folder, ext_name, map):
	if not os.path.exists(src_folder):
		print( "this bundle folder do not exist: %s" % src_folder, file=sys.stderr )
		return
	file_list = os.listdir(src_folder)
	for file in file_list:
		if os.path.splitext(file)[1] == ext_name:
			parse_idx_file(os.path.abspath(os.path.join(src_folder, file)), map)

def gen_scene_map(src_folder, ext_name, map):
	if not os.path.exists(src_folder):
		print( "this scene folder do not exist: %s" % src_folder, file=sys.stderr )
		return
	file_list = os.listdir(src_folder)
	for file in file_list:
		file_name = os.path.split(file)[1]
		name, ext = os.path.splitext(file_name)
		if ext == ext_name:
			if name in map:
				raise Exception, "duplicate scene found: %s" % name
			map[name] = [file_name,CalculateSize(os.path.join(EXP_SCENE_PATH,file_name))]

def exp_src_code(dst_file, template_file):
	if not template_file:
		fs = open(dst_file, "w")
		if not fs:
			raise Exception, "can not open res idx code file: %s" % dst_file
		for key in [g_scene_map,g_prefab_map]:
			for k in key.keys():
				ids = k+"-"+key[k][0]+"-"+str(key[k][1])
				fs.write(ids)
				fs.write("\n")
		fs.flush()
		fs.close()
		# print( 'Write text file : %s' % (dst_file, ) )
		return
	fs = open(dst_file, "wb")
	if not fs:
		raise Exception, "can not open res idx code file: %s" % dst_file

	fs.write(struct.pack('<' + 'i', len(g_scene_map)))
	for key in g_scene_map.keys():
		ids = key+"-"+g_scene_map[key][0]+"-"+str(g_scene_map[key][1])
		if len(ids) > 0:
			fs.write(struct.pack('<' + 'h', len(ids)))
			fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
			fs.flush()
	fs.write(struct.pack('<' + 'i', len(g_prefab_map)))
	for key in g_prefab_map.keys():
		ids = key+"-"+g_prefab_map[key][0]+"-"+str(g_prefab_map[key][1])
		if len(ids) > 0:
			fs.write(struct.pack('<' + 'h', len(ids)))
			fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
			fs.flush()
	fs.write(struct.pack('<' + 'i', len(g_texture_map)))
	for key in g_texture_map.keys():
		ids = key+"-"+g_texture_map[key][0]+"-"+str(g_texture_map[key][1])
		if len(ids) > 0:
			fs.write(struct.pack('<' + 'h', len(ids)))
			fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
			fs.flush()
	fs.write(struct.pack('<' + 'i', len(g_txt_map)))
	for key in g_txt_map.keys():
		ids = key+"-"+g_txt_map[key][0]+"-"+str(g_txt_map[key][1])
		if len(ids) > 0:
			fs.write(struct.pack('<' + 'h', len(ids)))
			fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
			fs.flush()
#	for maps in [g_scene_map,g_prefab_map,g_texture_map,g_txt_map]:
#		if maps is g_scene_map:
#			for key in maps.keys():
#				ids = SCENE_BEGIN+"-"+key+"-"+maps[key]
#				if len(ids) > 0:
#					fs.write(struct.pack('<' + 'h', len(ids)))
#					fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
#					fs.flush()
#			#	fs.write(SCENE_BEGIN+"-"+key+"-"+maps[key]+"\n")
#		elif maps is g_prefab_map:
#			for key in maps.keys():
#				ids = PREFAB_BEGIN+"-"+key+"-"+maps[key]
#				if len(ids) > 0:
#					fs.write(struct.pack('<' + 'h', len(ids)))
#					fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
#					fs.flush()
#			#	fs.write(PREFAB_BEGIN+"-"+key+"-"+maps[key]+"\n")
#		elif maps is g_texture_map:
#			for key in maps.keys():
#				ids = TEXTRUE_BEGIN+"-"+key+"-"+maps[key]
#				if len(ids) > 0:
#					fs.write(struct.pack('<' + 'h', len(ids)))
#					fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
#					fs.flush()
#			#	fs.write(TEXTRUE_BEGIN+"-"+key+"-"+maps[key]+"\n")
#		else:
#			for key in maps.keys():
#				ids = TXT_BEGIN+"-"+key+"-"+maps[key]
#				if len(ids) > 0:
#					fs.write(struct.pack('<' + 'h', len(ids)))
#					fs.write(struct.pack('<' + str(len(ids)) + 's', ids))
#					fs.flush()
			#	fs.write(TXT_BEGIN+"-"+key+"-"+maps[key]+"\n")
		#	txt = key + "--"+maps[key]
		#	if len(txt) > 0:
		#		fs.write(struct.pack('<' + str(len(txt)) + 's', txt))
		#		fs.flush()
	fs.flush()
	fs.close()
	# print( 'Write binary file : %s' % (dst_file, ) )

def gen_res_idx():
	g_prefab_map.clear()
	gen_asset_bundle_map(EXP_RES_IDX_PATH, PREFAB_IDX_EXT_NAME, g_prefab_map)
	print( PREFAB_IDX_EXT_NAME )
	g_texture_map.clear()
	gen_asset_bundle_map(EXP_RES_IDX_PATH, TEXTURE_IDX_EXT_NAME, g_texture_map)
	print( TEXTURE_IDX_EXT_NAME )
	g_txt_map.clear()
	gen_asset_bundle_map(EXP_RES_IDX_PATH, TXT_IDX_EXT_NAME, g_txt_map)
	print( TXT_IDX_EXT_NAME )
	g_scene_map.clear()
	gen_scene_map(EXP_SCENE_PATH, SCENE_EXT_NAME, g_scene_map)

	code_file = os.path.abspath( os.path.join(EXP_RES_CODE_PATH, EXP_RES_IDX_FILE) )
	exp_src_code(code_file, True)

	code_file = os.path.abspath( os.path.join(EXP_RES_CODE_PATH, EXP_RES_IDX_TXT) )
	exp_src_code(code_file, False)

if __name__ == '__main__':
	print( "gen res idx begin..." )
	gen_res_idx()
	print( "gen res idx successful" )
