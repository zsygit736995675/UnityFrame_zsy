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

	pck_msg.pack_msg_dir(DST_GAME_CONF_MSG_PATH,
					DST_GAME_MSG_FACT_PATH,
					DST_GAME_MSG_HANDLER_PATH,
					DST_GAME_MSG_HELPER_PATH)

if __name__ == '__main__':
	argv = sys.argv

	build()
