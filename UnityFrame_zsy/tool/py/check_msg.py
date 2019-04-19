# -*- coding:utf-8 -*-

"""pack game msg to cs and java
   author: gx
"""

import os
import sys
import struct
import re
from path_util import *


def parsecs(file_dir):
	handler_list = {}
	helper_list = []
	file_open = open(file_dir)
	try:
		beginstr = False
		mname = ""
		item_list = file_open.readlines()
		for name in item_list:
			index = name.find("message")
			if index != -1:
				handler_list[name] = helper_list
				mname = name
			else:
				index1 = name.find("{")
				if index1 != -1:
					beginstr = True
				else:
					index2 = name.find("}")
					if index2 != -1:
						beginstr = False
					if beginstr:
						DoNameStr(name,mname)
	finally:
		file_open.close()

def DoNameStr(name,namelist):
	strs = name.split()
	for nstr in strs:
		if nstr.lower() == "item":
			raise Exception, namelist + ": msg name have Item string cant build by unity,please change the name!"

def check_msg_dir(src_dir):
	if os.path.isfile(os.path.join(src_dir,"worldsrv.proto")):
		parsecs(os.path.join(src_dir,"worldsrv.proto"))

from config import *
if __name__ == '__main__':
	print "check msg begin..."
	check_msg_dir(SHARED_PWMSG_PATH)
	print "check msg successful"
