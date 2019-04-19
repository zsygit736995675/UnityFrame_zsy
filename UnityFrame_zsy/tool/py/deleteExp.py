import os
import shutil
from config import *
from path_util import *

def remove():
	delete_file_folder(DST_GAME_EXP_PATH)
	if not os.path.isdir(DST_GAME_EXP_PATH):
		os.mkdir(DST_GAME_EXP_PATH)
	if not os.path.exists(DST_GAME_CONF_MSG_PATH):
		os.mkdir(DST_GAME_CONF_MSG_PATH)

if __name__ == '__main__':
	print "remove exp folder begin..."
	remove()
	print "remove exp folder successful"