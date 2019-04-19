# -*- coding:utf-8 -*-

import sys, os

dirname = __path__[0]
if sys.platform == 'win32':
    __path__.insert( 0, os.path.join( dirname, 'Windows' ) )
elif sys.platform == 'darwin':
    __path__.insert( 0, os.path.join( dirname, 'Mac' ) )
else:
    __path__.insert( 0, os.path.join( dirname, 'Linux' ) )

from .Console import Console, console
from .SubProcess import SubProcess
from .RTXRobot import RTXRobot

__all__ = ['Console', 'console', 'SubProcess', 'RTXRobot', ]

