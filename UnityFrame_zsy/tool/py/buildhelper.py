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

	clear_folder_type(RES_HELP_TXT_PATH,TXT_EXT_NAME)
	recurse_copy_folder_type_to_same_dst(SRC_HELP_PATH,RES_HELP_TXT_PATH,TXT_EXT_NAME,".svn")

	print "excute help begin..."
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackRes_GameHelp.Execute"
	os.system(cmd + RES_HELP_PATH + param)
	print "excute help success"

if __name__ == '__main__':
	argv = sys.argv

	build()