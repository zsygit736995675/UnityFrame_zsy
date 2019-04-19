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

	doPackResDep()
	
	print "copy setting begin..."
	copy_res.copy_setting_only()
	print "copy setting success"

def doPackResDep():
	print "excute PackResDep begin..."
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackEntry.PackResDep"
	print "",cmd + RES_OTHER_PATH + param
	ret = os.system(cmd + RES_OTHER_PATH + param)
	if ret == 0 :
		print "excute PackResDep success."
	else :
		print "excute PackResDep fail. maybe some file is open by another process. try kill unity editor."

if __name__ == '__main__':
	argv = sys.argv

	build()