# -*- coding:utf-8 -*-

"""pack game msg to cs and java
   author: gx
"""

from __future__ import print_function
import os
import sys
import xlrd
import struct
import re
from config import *
from path_util import *
from mako.template import Template

BYTE_ORDER = '<'
ARR_SPLIT = ','
g_CfgData_List = []

def exp_handler_common_file(file_name,template,class_name,msg_name):
    MakeDir(file_name)

    #file = open(file_name, 'a')
    templ = Template(filename = template)
    txt = templ.render(class_name = class_name,msg_name = msg_name)
    return txt
#file.write(txt)

    #file.flush()
    #file.close()

def exp_handler_file(file_name, template,class_name,msg_name):
    MakeDir(file_name)

    file = open(file_name, 'w')
    templ = Template(filename = template)
    txt = templ.render(class_name = class_name,msg_name = msg_name)
    #txt = txt.replace('\n', '')
    file.write(txt)

    file.flush()
    file.close()

def exp_helper_common_file(file_name,template,class_name,msg_name):
    MakeDir(file_name)

    #file = open(file_name, 'a')
    templ = Template(filename = template)
    txt = templ.render(class_name = class_name,msg_name = msg_name)
    return txt
#file.write(txt)

    #file.flush()
    #file.close()

def exp_helper_file(file_name, template,class_name,msg_name):
    MakeDir(file_name)

    file = open(file_name, 'w')
    templ = Template(filename = template)
    txt = templ.render(class_name = class_name,msg_name = msg_name)
    #txt = txt.replace('\n', '')
    file.write(txt)

    file.flush()
    file.close()

def exp_mgr_code_file(file_name, template, cls_name_list):
    MakeDir(file_name)

    file = open(file_name, 'w')
    templ = Template(filename = template)
    txt = templ.render(class_list = cls_name_list)
    #txt = txt.replace('\n', '')
    file.write(txt)

    file.flush()
    file.close()

def parsecs(file_dir):
    handler_list = []
    helper_list = []
    file_open = open(file_dir)
    try:
        item_list = file_open.readlines()
        for name in item_list:
            index = name.find("msgid_SC")
            if index != -1:
                handler_list.append(name[name.find("msgid_SC")+6:name.find("=")])
            index1 = name.find("msgid_CS")
            if index1 != -1:
                helper_list.append(name[name.find("msgid_CS")+6:name.find("=")])
    finally:
        file_open.close()
    return handler_list,helper_list

def isFileExist(desname):
    if os.path.isfile(desname):
        return True
    return False

def MakeDir(file_name):
    dir, name = os.path.split(file_name)
    if (dir) and (not os.path.exists(dir)):
        os.mkdir(dir)
    if os.path.isfile(file_name):
        os.chmod(file_name, stat.S_IREAD | stat.S_IWRITE)

def writetext(file_name,txt):
    MakeDir(file_name)

    print( 'Write : ', os.path.abspath(file_name), file=sys.stderr )
    # print( txt, file=sys.stderr )
    with open(file_name, 'w') as f:
        f.write(txt)
        f.flush()
        f.close()

def DoCheckNameWithoutItemStr(file_name):
    count = file_name.lower().find("item")
    if count != -1:
        return True
    return False

def pack_msg_dir(src_dir, fact_dir, handler_dir, helper_dir):
    # defFilePath = os.path.join( '../protobuf', 'msg.def.cs' )
    defFilePath = os.path.join( src_dir, 'msg.def.cs' )
    # os.path.join(src_dir,"msg.def.cs")
    if os.path.isfile( defFilePath ):
        hlist,helist = parsecs( defFilePath )
        msghlname = os.path.join(handler_dir, "MsgCommonHandler.cs")
        msghename = os.path.join(helper_dir, "MsgCommonHelper.cs")

        force_del(msghlname)
        force_del(msghename)

        txt = ""
        txt += exp_handler_common_file(msghlname, os.path.join(MAKO_PATH, GAME_MSG_BEGIN_MAKO_CS),"","")
        for file in hlist:
            #    if DoCheckNameWithoutItemStr(file):
            #   raise Exception, file + ": msg name have Item string cant build by unity,please change the name!"

            class_name = file+"Handler"
            code_file = class_name + '.cs'
            code_path = os.path.join(handler_dir, code_file)
            mako_path = os.path.join(MAKO_PATH, GAME_MSG_COMMON_HANDLER_MAKO_CS)
            txt += exp_handler_common_file(msghlname, mako_path, class_name,file)
            if not isFileExist(code_path):
                # 不要覆盖已有的文件
                mako_path = os.path.join(MAKO_PATH, GAME_MSG_HANDLER_MAKO_CS)
                exp_handler_file(code_path, mako_path, class_name, file)
        mako_path = os.path.join(MAKO_PATH, GAME_MSG_END_MAKO_CS)
        txt += exp_handler_common_file(msghlname, mako_path, '', '')
        writetext( msghlname, txt )

        txt = ""
        txt += exp_handler_common_file(msghename, os.path.join(MAKO_PATH, GAME_MSG_BEGIN_MAKO_CS),"","")
        for file in helist:
            #    if DoCheckNameWithoutItemStr(file):
            #        raise Exception, file + " : msg name have Item string cant build by unity,please change the name!"
            class_name = file+"Helper"
            code_file = class_name + '.cs'
            code_path = os.path.join(helper_dir, code_file)
            mako_path = os.path.join(MAKO_PATH, GAME_MSG_COMMON_HELPER_MAKO_CS)
            txt += exp_helper_common_file(msghename, mako_path, class_name, file)
            # if not isFileExist(code_path):
            #     # 使用 AllInOne 的解决方案空代码还是不生成了吧
            #     mako_path = os.path.join(MAKO_PATH, GAME_MSG_HELPER_MAKO_CS)
            #     exp_helper_file(code_path, mako_path, class_name, file)
        mako_path = os.path.join(MAKO_PATH, GAME_MSG_END_MAKO_CS)
        txt += exp_handler_common_file(msghename, mako_path, "", "")
        writetext(msghename,txt)

        exp_mgr_code_file(os.path.join(fact_dir, "MsgHandlerFact.cs"),
                os.path.join(MAKO_PATH, GAME_MSG_FACT_MAKO_CS),
                hlist)

if __name__ == '__main__':
    print( "pack msg begin..." )
    pack_msg_dir(DST_GAME_CONF_MSG_PATH,
            DST_GAME_MSG_FACT_PATH,
            DST_GAME_MSG_HANDLER_PATH,
            DST_GAME_MSG_HELPER_PATH)
    print( "pack msg successful" )
