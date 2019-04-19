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

def build():
	cmd = "Unity.exe -batchmode -quit -projectPath "
	param = " -executeMethod PackEntry.GenElement"
	os.system(cmd + RES_OTHER_PATH + param)
	print "gen element success"

if __name__ == '__main__':
	argv = sys.argv

	build()