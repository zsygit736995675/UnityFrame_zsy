# -*- coding:utf-8 -*-
from __future__ import print_function
import sys
import traceback

def _excepthook(exctype, value, tb):
    print( "Exception Caught!", file=sys.stderr)
    print( "type: " + str(exctype), file=sys.stderr)
    print( "value: " + str(value), file=sys.stderr)
    #print 'you get an unknown exception, contact author zhang shi liang!'
    traceback.print_tb(tb)
    #os.system('pause')
    raw_input("press Enter to continue...")
    exit()

def _error_and_exit(msg):
    print( "Error! %s\n" % msg, file=sys.stderr )
    raw_input("press Enter to exit...")
    exit()
    
def _error_and_continue(msg):
    print( "Error! %s\n" % msg, file=sys.stderr )
    raw_input("press Enter to continue...")
    
