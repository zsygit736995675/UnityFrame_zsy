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

def build():

	doPackHelp()

def doPackHelp():
	print "excute PackHelper begin..."
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackEntry.PackHelper"
	os.system(cmd + RES_OTHER_PATH + param)
	print "",cmd + RES_OTHER_PATH + param
	print "excute PackHelper success"

if __name__ == '__main__':
	argv = sys.argv

	build()