# -*- coding:utf-8 -*-

"""pack game config xls to cs and java
   author: zhangshiliang
"""

from __future__ import print_function
import os
import sys
import xlrd
import struct
import re
from mako.template import Template

BYTE_ORDER = '<'
ARR_SPLIT = ','
g_CfgData_List = []

class ParseException(Exception):
	def __init__(self, id, name, native_except):
		self.id = id
		self.name = name
		self.native_except = native_except

	def __str__(self):
		return "id: " + str(self.id) + ", name: " + str(self.name)

class CfgData:
	def __init__(self, name_list, type_list, cl_list, arr_list, data_list):
		self.name_list = name_list
		self.type_list = type_list
		self.cl_list = cl_list
		self.arr_list = arr_list
		self.data_list = data_list

def parse_xls(file_name):
	book = open(file_name,'r')
	if not book:
		raise Exception, "can not open file: %s" % file_name
		return
	# print("parse_xls" )
	nl, tl, cl, al = __init_prop(book)
	dl = __init_data(book)
	# print("parse_xls",dl )
	data = CfgData(nl, tl, cl, al, dl)
	return data

def exp_code_file(file_name, template, cfg_data, class_name, bin_file):
	dir, name = os.path.split(file_name)
	if (dir) and (not os.path.exists(dir)):
		os.mkdir(dir)

	file = open(file_name, 'w')
	templ = Template(filename = template)
	txt = templ.render(type_list = cfg_data.type_list, name_list = cfg_data.name_list,
						client_list = cfg_data.cl_list, arr_list = cfg_data.arr_list,
						class_name = class_name, bin_file_name = bin_file)
	txt = txt.replace('\n', '')
	file.write(txt)

	file.flush()
	file.close()

def exp_bin_file(file_name, cfg_data, class_name):
	bf = open(file_name, 'wb')
	bf.write(struct.pack(BYTE_ORDER + 'i', 0))
	bf.write(struct.pack(BYTE_ORDER + 'h', len(class_name)))
	bf.write(struct.pack(BYTE_ORDER + str(len(class_name)) + 's', class_name))
	bf.write(struct.pack(BYTE_ORDER + 'h', __col_cnt(cfg_data)))
	bf.write(struct.pack(BYTE_ORDER + 'i', len(cfg_data.data_list)))

	for data in cfg_data.data_list:
		try:
			val = __parse_data(data, cfg_data)
		except ParseException, pe:
			print( "parse err: " + str(pe), file=sys.stderr )
			raise pe.native_except

		for i in range(len(cfg_data.cl_list)):
			if(cfg_data.cl_list[i].count('c') <= 0):
				continue

			item = val[i]
			if cfg_data.arr_list[i]:
				bf.write(struct.pack(BYTE_ORDER + 'h', len(item)))
				for elem in item:
					__write_item(bf, cfg_data.type_list[i], elem)
			else:
				__write_item(bf, cfg_data.type_list[i], item)

			bf.flush()

	length = bf.tell()
	bf.seek(0)
	bf.write(struct.pack(BYTE_ORDER + 'i', length))
	bf.flush()
	bf.close()

def exp_csv_file(file_name, cfg_data):
	file = open(file_name, 'wt')

	lines = []
	line = ""
	for name in cfg_data.name_list:
		line = line + name + '\t'
	line = line + '\n'
	lines.append(line)

	for data in cfg_data.data_list:
		line = ""
		index = 0
		for item in data:
			type = cfg_data.type_list[index]
			if type == "int" or type == "byte" or type == "short":
				line = line + str(int(item)) + '\t'
			elif type == "bool":
				item = str(item)
				if item == "FALSE" or item == "0":
					line = line + "0" + '\t'
				else:
					line = line + "1" + '\t'
			elif type == "long":
				line = line + str(long(item)) + '\t'
			else:
				line = line + str(item) + '\t'
			index = index + 1
		line = line + '\n'
		lines.append(line)

	file.writelines(lines)
	file.flush()
	file.close()

def __init_prop(book):
	#sheet = book.sheet_by_index(0)

	#row 0 is comment
	#name_list = [str(t.value).strip() for t in sheet.row(1)]
	#type_list = [str(t.value).strip().lower() for t in sheet.row(2)]
	#client_list = [str(t.value).strip().lower() for t in sheet.row(3)]
	#arr_list = [t.endswith('[]') for t in type_list]
	name_list = ['id','name','movetime','rotatotime','keyframes',]
	type_list = ['int','string','float','float','string',]
	client_list = ['cs','c','c','c','c',]
	arr_list = [t.endswith('[]') for t in type_list]
	return name_list, type_list, client_list, arr_list

def __init_data(book):
	data_list = []
	lines = book.readlines()
	for line in lines:
		line = line.strip()
		item_list = line.split('#')
		for i in range(len(item_list)):
			if type(item_list[i]) is unicode:
				item_list[i] = item_list[i].encode('utf-8')
		data_list.append(item_list)

	return data_list

def __col_cnt(cfg_data):
	cnt = 0
	for item in cfg_data.cl_list:
		if item.count('c') > 0:
			cnt += 1
	return cnt

def __write_item(file, type, value):
	if type.startswith('bool'):
		file.write(struct.pack(BYTE_ORDER + 'b', value))
	elif type.startswith('byte'):
		file.write(struct.pack(BYTE_ORDER + 'B', value))
	elif type.startswith('short'):
		file.write(struct.pack(BYTE_ORDER + 'h', value))
	elif type.startswith('int'):
		file.write(struct.pack(BYTE_ORDER + 'i', value))
	elif type.startswith('long'):
		file.write(struct.pack(BYTE_ORDER + 'q', value))
	elif type.startswith('float'):
		file.write(struct.pack(BYTE_ORDER + 'f', value))
	elif type.startswith('double'):
		file.write(struct.pack(BYTE_ORDER + 'd', value))
	elif type.startswith('string'):
		file.write(struct.pack(BYTE_ORDER + 'h', len(value)))
		if len(value) > 0:
			file.write(struct.pack(BYTE_ORDER + str(len(value)) + 's', value))
	else:
		raise Exception, "unknown type to write: " + type

def __convert_item(type, value):
	str_val = str(value).lower().strip()
	if type.startswith('bool'):
		if str_val == 'false' or str_val == '0' or not str_val:
			return 0
		elif str_val == 'true' or str_val == '1':
			return 1
		else:
			raise Exception, "invalid bool data: " + str_val
	elif type.startswith('byte'):
		if not str_val:
			value = 0
		return int(value)
	elif type.startswith('short'):
		if not str_val:
			value = 0
		return int(value)
	elif type.startswith('int'):
		if not str_val:
			value = 0
		return int(value)
	elif type.startswith('long'):
		if not str_val:
			value = 0
		return long(value)
	elif type.startswith('float'):
		if not str_val:
			value = 0
		return float(value)
	elif type.startswith('double'):
		if not str_val:
			value = 0
		return float(value)
	elif type.startswith('string'):
		return str(value)
	else:
		raise Exception, "unknown type to convert: " + type

def __parse_data(elem_list, cfg_data):
	value_list = []
	for i in range(len(cfg_data.cl_list)):
		try:
			if(cfg_data.cl_list[i].count('c') <= 0):
				value_list.append(elem_list[i])
				continue

			item = elem_list[i]
			if(cfg_data.arr_list[i]):
				arr = []
				if not str(item).strip():
					pass # do nothing
				if type(item) is not str:
					value = __convert_item(cfg_data.type_list[i], item)
					arr.append(value)
				else:
					for elem in item.split(','):
						if not elem.strip():
							continue

						value = __convert_item(cfg_data.type_list[i], elem.strip())
						arr.append(value)
				value_list.append(arr)
			else:
				value = __convert_item(cfg_data.type_list[i], item)
				value_list.append(value)
		except Exception, inst:
			raise ParseException(str(elem_list[0]), cfg_data.name_list[i], inst)

	return value_list

def exp_mgr_code_file(file_name, template, cls_name_list):
	dir, name = os.path.split(file_name)
	if (dir) and (not os.path.exists(dir)):
		os.mkdir(dir)

	file = open(file_name, 'w')
	templ = Template(filename = template)
	txt = templ.render(class_list = cls_name_list)
	txt = txt.replace('\n', '')
	file.write(txt)

	file.flush()
	file.close()

def pack_config_dir(src_dir, code_dir, bin_dir, csv_dir, file):
			cls_name_list = []

			print( "pack curve file: ",file )
			file_name = os.path.splitext(file)[0]
			class_name = os.path.splitext(file)[0]
			code_file = file_name + '.cs'
			bin_file = file_name + '.bytes'
			csv_file = file_name + '.csv'
			cfg_data = parse_xls(os.path.join(src_dir, file))

			exp_bin_file(os.path.join(bin_dir, bin_file), cfg_data, class_name)
			exp_code_file(os.path.join(code_dir, code_file),
						os.path.join(MAKO_PATH, GAME_CONF_MAKO_CS),
						cfg_data, class_name, bin_file)
		#	exp_csv_file(os.path.join(csv_dir, csv_file), cfg_data)

			#cls_name_list.append(class_name)

		#	exp_mgr_code_file(os.path.join(code_dir, "ConfFact.cs"),
		 #           os.path.join(MAKO_PATH, GAME_CONF_FACT_MAKO_CS),
		 #           cls_name_list)

from config import *
if __name__ == '__main__':
	print( "pack curve conf begin..." )
	pack_config_dir(SRC_GAME_CONF_PATH,
					EXP_GAME_CONF_CODE_PATH,
					EXP_GAME_CONF_BIN_PATH,
					EXP_GAME_CONF_CSV_PATH)
	print( "pack curve conf successful" )
