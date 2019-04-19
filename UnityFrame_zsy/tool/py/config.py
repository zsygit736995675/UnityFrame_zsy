# -*- coding:utf-8 -*-
"""path config
"""

import os

#gobal path config
CWD = os.path.normpath(os.path.abspath(os.getcwd()))

ROOT_PATH = os.path.normpath(os.path.abspath(os.path.join(CWD, '../../')))
SRC_PATH = os.path.join(ROOT_PATH, 'Frame_zsy')
CONF_PATH = os.path.join(ROOT_PATH, 'shared')
BIN_PATH = os.path.join(ROOT_PATH, 'bin')
RES_PATH = os.path.join(ROOT_PATH, '../res')
#MAX_EFFECT_PATH = os.path.join(ROOT_PATH, 'max_effect')

#CLIENT_BIN_PATH = os.path.join(ROOT_PATH, 'bin')
#CLIENT_BIN_RES_PATH = os.path.join(CLIENT_BIN_PATH, 'res')
#CLIENT_BIN_WEB_PATH = os.path.join(CLIENT_BIN_PATH, 'webres')

RES_GUI_PATH = os.path.join(RES_PATH, 'gui')

RES_SCENE_PATH = os.path.join(RES_PATH, 'scene')
RES_NAVMESH_PATH = os.path.join(RES_PATH, 'navmesh')

RES_AVATAR_PATH = os.path.join(RES_PATH, 'avatar')
RES_CHARACTER_PATH = os.path.join(RES_PATH, 'character')
RES_OBJECT_PATH = os.path.join(RES_PATH, 'object')
RES_EFFECT_PATH = os.path.join(RES_PATH, 'effect')

RES_TEXTURE_PATH = os.path.join(RES_PATH, 'texture')
RES_SOUND_PATH = os.path.join(RES_PATH, 'sound')

RES_GAME_CONF_PATH = os.path.join(RES_PATH, "game_conf")

RES_MAX_EFFECT_PATH = os.path.join(RES_PATH, "max_effect")
RES_PROJECT_PATH = os.path.join(RES_PATH,"project")
RES_INDEXMAP_PATH = os.path.join(RES_PATH,"IndexMap")
RES_HELP_PATH = os.path.join(RES_PATH,"game_helper")
RES_OTHER_PATH = os.path.join(ROOT_PATH,"Other_Pack")
#RES_PROJECT_BUILD_PATH = os.path.join(RES_PATH,"project/
SRC_PROJ_PATH = os.path.join(SRC_PATH, 'Assets')
SHARED_PWMSG_PATH = os.path.join(CONF_PATH,'pwmsg')

#EXP_PATH = os.path.join(ROOT_PATH, '..\exp')
EXP_PATH = os.path.join(ROOT_PATH, 'Frame_zsy')

EXP_GUI_PATH = os.path.join(EXP_PATH, 'exp_gui')
EXP_RES_IDX_PATH = os.path.join(EXP_PATH, 'exp_res_idx')
EXP_BUNDLE_PATH = os.path.join(EXP_PATH, 'exp_bundle')
EXP_SCENE_PATH = os.path.join(EXP_PATH, 'exp_scene')
EXP_SCRIPT_PATH = os.path.join(EXP_PATH, 'exp_script')
EXP_SHADER_PATH = os.path.join(EXP_PATH, 'exp_shader')
EXP_RES_CODE_PATH = os.path.join(EXP_PATH, 'exp_res_code')

#EXP_WEB_PATH = os.path.join(ROOT_PATH, '..\expweb')

TOOL_PATH = os.path.join(ROOT_PATH, 'tool')
PYTHON_TOOL_PATH = os.path.join(TOOL_PATH, 'py')
MAKO_PATH = os.path.join(PYTHON_TOOL_PATH, 'mako')

#res idx
SCENE_EXT_NAME = ".unity3d"
BUNDLE_EXT_NAME = '.bundle'
PREFAB_IDX_EXT_NAME = ".pfb_idx"
TEXTURE_IDX_EXT_NAME = ".tex_idx"
TXT_IDX_EXT_NAME = ".txt_idx"
TXT_EXT_NAME = ".txt"
SCENE_BEGIN = "SCENE"
PREFAB_BEGIN = "PREFAB"
TEXTRUE_BEGIN = "TEXTURE"
TXT_BEGIN = "TXT"
XLS_EXT = ".xls"

#res id
INDEX_DST_EXT_NAME = '.bytes'
EXP_RES_ID_FILE = "AssetID.cs"
EXP_BUNDLE_ID_FILE = "BundleID.bytes"
EXP_RES_IDX_FILE = 'ResIdxMapFact.bytes'
EXP_BUNDLE_ID_TXT = "BundleID.txt"
EXP_RES_IDX_TXT = 'ResIdxMapFact.txt'
GEN_ID_MAKO_CS = 'GenID_cs.mako'
RES_IDX_MAP_MAKO_CS = 'ResIdxMapFact_cs.mako'
BUNDLEMAP_FILE = 'bundlemap.map.txt'

#game conf
SRC_GAME_CONF_PATH = os.path.join(CONF_PATH, "config")
RES_GAME_CONF_FILE_PATH = os.path.join(RES_GAME_CONF_PATH, "Assets/conf")
EXP_GAME_CONF_CODE_PATH = os.path.join(EXP_PATH, "Assets\exp\exp_conf_code")
EXP_GAME_CONF_BIN_PATH = os.path.join(EXP_PATH, "Assets\exp\exp_bytes")
EXP_GAME_CONF_CSV_PATH = os.path.join(EXP_PATH, "Assets\exp\exp_conf_csv")
DST_GAME_CONF_CODE_PATH = os.path.join(SRC_PROJ_PATH, 'exp/conf')

DST_GAME_CONF_MSG_PATH = os.path.join(SRC_PROJ_PATH,'exp/msg')
DST_GAME_EXP_PATH = os.path.join(SRC_PROJ_PATH, 'exp')
DST_GAME_MSG_FACT_PATH = os.path.join(SRC_PROJ_PATH,'game/fact')
DST_GAME_MSG_HANDLER_PATH = os.path.join(SRC_PROJ_PATH,'game/msg')
DST_GAME_MSG_HELPER_PATH = os.path.join(SRC_PROJ_PATH,'game/MsgHelper')

#EXP_WEB_GAME_CONF_CODE_PATH = os.path.join(EXP_WEB_PATH, "exp_conf_code")
#EXP_WEB_GAME_CONF_BIN_PATH = os.path.join(EXP_WEB_PATH, "exp_conf_bin")
#EXP_WEB_GAME_CONF_CSV_PATH = os.path.join(EXP_WEB_PATH, "exp_conf_csv")

GAME_CONF_MAKO_CS = "Config_cs.mako"
GAME_CONF_FACT_MAKO_CS = "ConfFact_cs.mako"
GAME_MSG_FACT_MAKO_CS = "MsgFact_cs.mako"
GAME_MSG_HANDLER_MAKO_CS = "MsgHandler_cs.mako"
GAME_MSG_HELPER_MAKO_CS = "MsgHelpr_cs.mako"
GAME_MSG_COMMON_HANDLER_MAKO_CS = "MsgCommonHandler_cs.mako"
GAME_MSG_COMMON_HELPER_MAKO_CS = "MsgCommonHelpr_cs.mako"
GAME_MSG_BEGIN_MAKO_CS = "MsgBegin_cs.mako"
GAME_MSG_END_MAKO_CS = "MsgEnd_cs.mako"

#max effect
MAX_EFFECT_SRC_EXT_NAME = '.eft'
MAX_EFFECT_DST_EXT_NAME = '.bytes'
EXP_MAX_EFFECT_PATH = os.path.join(EXP_PATH, "exp_max_effect")
RES_MAX_EFFECT_FILE_PATH = os.path.join(RES_MAX_EFFECT_PATH, "Assets/effect")

#copy res path config
DST_SCRIPT_PATH = os.path.join(SRC_PROJ_PATH, 'script')
DST_SHADER_PATH = os.path.join(SRC_PROJ_PATH, 'Resources/exp_shader')
DST_RES_CODE_PATH = os.path.join(SRC_PROJ_PATH, 'exp/res')
DST_GUI_CODE_PATH = os.path.join(SRC_PROJ_PATH, 'exp/gui')

DST_RES_PATH = os.path.join(BIN_PATH, 'res')
DST_WEB_RES_PATH = os.path.join(BIN_PATH, 'webres')

#RES_INDEX_MAP_PATH = os.path.join(RES_INDEXMAP_PATH,'Assets/indexmap')
#RES_HELP_TXT_PATH = os.path.join(RES_HELP_PATH,'Assets/help')
#SRC_HELP_PATH = os.path.join(SRC_PATH,'Assets/EditorExp')
#RES_OTHER_INDEXMAP_PATH = os.path.join(RES_OTHER_PATH,'Assets/IndexMap')
#RES_OTHER_HELP_PATH = os.path.join(RES_OTHER_PATH,'Assets/Helper')
#RES_OTHER_CONFIG_PATH = os.path.join(RES_OTHER_PATH,'Assets/Config')
