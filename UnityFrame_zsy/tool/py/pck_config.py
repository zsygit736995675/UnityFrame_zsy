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
import string
from path_util import *
from mako.template import Template
from RuntimeOS import console

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
	book = xlrd.open_workbook(file_name)
	if book.nsheets <= 0:
		return

	nl, tl, cl, al = __init_prop(book)
	dl = __init_data(book)

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

	idlist = []
	for data in cfg_data.data_list:
		try:
			val = __parse_data(data, cfg_data,class_name)

			for i in range(len(cfg_data.cl_list)):
			    if(cfg_data.cl_list[i].count('c') <= 0):
			        continue

			    if i == 0:
			        if str(val[i]).startswith('#'):
			            continue
			        if not val[i] in idlist:
			            idlist.append(val[i])
			        else:
			            raise Exception, "class : "+class_name+" ,has dumplicate id: " + str(val[i])
			    item = val[i]
			    if cfg_data.arr_list[i]:
			        bf.write(struct.pack(BYTE_ORDER + 'h', len(item)))
			        for elem in item:
			            __write_item(bf, cfg_data.type_list[i], elem)
			    else:
			        __write_item(bf, cfg_data.type_list[i], item)

			    bf.flush()
		except  Exception, e:
		    console.Error("Parse Sheet :" +  file_name + " error" + "\n")
		    raise e

	length = bf.tell()
	bf.seek(0)
	bf.write(struct.pack(BYTE_ORDER + 'i', length))
	bf.flush()
	bf.close()

def exp_content_file(file_name, cfg_data, class_name):
	bf = open(file_name, 'wb')
	idlist = []
	for data in cfg_data.data_list:
		try:
			val = __parse_data(data, cfg_data,class_name)
		except ParseException, pe:
			print( "parse err: " + str(pe) + ", classname: "+class_name, file=sys.stderr )
			raise pe.native_except

		for i in range(len(cfg_data.cl_list)):
			if(cfg_data.cl_list[i].count('c') <= 0):
				continue

			item = val[i]
			if cfg_data.arr_list[i]:
				bf.write(" [")
				for elem in item:
					__write_txt(bf, cfg_data.name_list[i], elem)
				bf.write("] ")
			else:
				__write_txt(bf, cfg_data.name_list[i], item)
			bf.write(", ")
			bf.flush()
		bf.write("\n")
		bf.write("\n")
	length = bf.tell()
	bf.seek(0)
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
				if item == '':
					item = 0
				line = line + str(int(item)) + '\t'
			elif type == "bool":
				item = str(item)
				if item == "FALSE" or item == "0" or item == "false":
					line = line + "0" + '\t'
				else:
					line = line + "1" + '\t'
			elif type == "long":
				if item == '':
					item = 0
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
	sheet = book.sheet_by_index(0)

	#row 0 is comment，1。名字列，2.类型列，3.标志列
	name_list = [str(t.value).strip() for t in sheet.row(1)]
	type_list = [str(t.value).strip().lower() for t in sheet.row(2)]
	client_list = [str(t.value).strip().lower() for t in sheet.row(3)]
	arr_list = [__isArrList(t) for t in type_list]

	return name_list, type_list, client_list, arr_list

def __isArrList(alist):
	if alist.rfind('[') != -1 & alist.rfind(']') != -1:
		return True
	return False

def __init_data(book):
	data_list = []
	for sheet in book.sheets():
		if sheet.name.startswith('~'):
			continue

		for rx in range(4, sheet.nrows):
			item_list = [data.value for data in sheet.row(rx)]
			for i in range(len(item_list)):
				if type(item_list[i]) is unicode:
					item_list[i] = item_list[i].encode('utf-8')

			if str(item_list[0]).startswith('#'):
				continue
			if str(item_list[0]):
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

def __write_txt(file,type,value):
	file.write(type+" : "+str(value))

def __convert_item(type, value):
	str_val = str(value).lower().strip()
	result = 0
	if type.startswith('bool'):
		if str_val == 'false' or str_val == '0.0' or str_val == '0' or not str_val:
			return 0
		elif str_val == 'true' or str_val == '1.0' or str_val == '1':
			return 1
		else:
			raise Exception, "invalid bool data: " + str_val
	elif type.startswith('byte'):
		if not str_val:
			value = 0
		result = int(value)
	elif type.startswith('short'):
		if not str_val:
			value = 0
		result = int(value)
	elif type.startswith('int'):
		if not str_val:
			value = 0
		try:
			result = int(value)
		except ValueError:
			raise Exception, "data is not digit in (short) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('long'):
		if not str_val:
			value = 0
		result = long(value)
	elif type.startswith('float'):
		if not str_val:
			value = 0
		try:
			result = float(value)
		except ValueError:
			raise Exception, "data is not digit in (short) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('double'):
		if not str_val:
			value = 0
		result = float(value)
	elif type.startswith('string'):
		return str(value)
	else:
		raise Exception, "unknown type to convert: " + type

#	if not result.isdigit():
#		raise Exception, "data is not digit in (short) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
#	else:
	return result

def __parse_data(elem_list, cfg_data,class_name):
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
				#	istyperight(cfg_data.type_list[i], item,i,class_name,cfg_data.name_list[i])
					value = __convert_item(cfg_data.type_list[i], item)
					arr.append(value)
				else:
					#print "is str: ",item
					number = __getarrcount(cfg_data.type_list[i])
					#print "number: ",number
					y = 0
					for elem in item.split(','):
						if not elem.strip():
							continue
						if y >= number & number != -1:
							continue
					#	istyperight(cfg_data.type_list[i], elem.strip(),i,class_name,cfg_data.name_list[i])
						value = __convert_item(cfg_data.type_list[i], elem.strip())
						arr.append(value)
						y += 1
				value_list.append(arr)
			else:
				value = __convert_item(cfg_data.type_list[i], item)
				value_list.append(value)
		except Exception, inst:
			raise ParseException(str(elem_list[0]), cfg_data.name_list[i], inst)

	return value_list

def __getarrcount(arlist):

	s = -1
	lindex = arlist.rfind('[')
	rindex = arlist.rfind(']')
#	print "lindex: ",lindex,"rindex: ",rindex,arlist[lindex+1:rindex]
	if arlist.endswith('[]'):
		s = -1
	elif lindex != -1 & rindex != -1:
		ns = arlist[lindex+1:rindex]
#		print "ns: ",ns
		if ns.isdigit():
			s = string.atoi(ns)
#			print "getcount: ",s
	return s

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

def istyperight(type,value,line,class_name,dataname):
	str_val = str(value).lower().strip()
	if type.startswith('bool'):
		pass
	elif type.startswith('byte'):
		pass
	elif type.startswith('short'):
		if not str_val:
			str_val = 0
		if not (str_val).isdigit():
			raise Exception, "data is not digit in (short) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('int'):
		if not str_val:
			str_val = 0
		if not (str_val).isdigit():
			print( str_val+" class "+class_name )
			raise Exception, "data is not digit in (int) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('long'):
		if not str_val:
			str_val = 0
		if not (str_val).isdigit():
			raise Exception, "data is not digit in (long) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('float'):
		if not str_val:
			str_val = 0
		if not (str_val).isdigit():
			raise Exception, "data is not digit in (float) type in line: " + str(line)+" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('double'):
		if not str_val:
			str_val = 0
		if not (str_val).isdigit():
			raise Exception, "data is not digit in (double) type in line: " + str(line) +" of name: "+dataname + " , classname: "+class_name
	elif type.startswith('string'):
		pass
	else:
		raise Exception, "unknown type to convert: " + type

def isclientxls(cfg_data):
	isclient = False
	for i in range(len(cfg_data.cl_list)):
	#	print "client: "+cfg_data.cl_list[i]
		if(cfg_data.cl_list[i].count('c') > 0):
			isclient = True
		#	print "is--client: "
			break
	return isclient

def pack_config_dir(src_dir, code_dir, bin_dir, csv_dir):
	clear_folder_type(code_dir, ".cs")
	clear_folder_type(bin_dir,".bytes")
#	clear_folder_type(csv_dir,".csv")
	cls_name_list = []
	for file in os.listdir(src_dir):
		if os.path.isfile(os.path.join(src_dir, file)):
			if os.path.splitext(file)[1] == '.xls':
				#--print( "pack file: " + file + " Prepare" )
				file_name = os.path.splitext(file)[0]
				class_name = file_name
				code_file = file_name + '.cs'
				bin_file = file_name + '.bytes'
				csv_file = file_name + '.csv'
				txt_file = file_name + '.txt'

				cfg_data = parse_xls(os.path.join(src_dir, file))
				if isclientxls(cfg_data):
					#--print( "pack file: "+file )
					exp_bin_file(os.path.join(bin_dir, bin_file), cfg_data, class_name)
					exp_code_file(os.path.join(code_dir, code_file),
							os.path.join(MAKO_PATH, GAME_CONF_MAKO_CS),
							cfg_data, class_name, bin_file)
					#exp_content_file(os.path.join(csv_dir, txt_file), cfg_data, class_name)
				#	exp_csv_file(os.path.join(csv_dir, csv_file), cfg_data)
					cls_name_list.append(class_name)

			else:
				if os.path.splitext(file)[0] == 'Curve':
					print("go Curve.txt" )
					import pak_curve
					pak_curve.pack_config_dir(src_dir, code_dir, bin_dir, csv_dir,file)
					cls_name_list.append(os.path.splitext(file)[0])
#	print "conffact:",cls_name_list
	exp_mgr_code_file(os.path.join(code_dir, "ConfFact.cs"),
					os.path.join(MAKO_PATH, GAME_CONF_FACT_MAKO_CS),
					cls_name_list)

from config import *
if __name__ == '__main__':
	print( "pack conf begin..." )
	pack_config_dir(SRC_GAME_CONF_PATH,
					EXP_GAME_CONF_CODE_PATH,
					EXP_GAME_CONF_BIN_PATH,
					EXP_GAME_CONF_CSV_PATH)
	print( "pack conf successful" )
