# -*- coding:utf-8 -*-

from __future__ import print_function
import os, sys
from SubProcess import SubProcess

curPath = os.path.split(os.path.realpath(__file__))[0]

class RTXRobot() :
    def __init__( self ):
        pass

    def SendMsg( self, title, msg ):
        rtxmsg =  '%s \"%s\" \"%s\"' % (os.path.join( curPath, 'rtxmsg.exe' ), title, msg) 
        rtxmsg = [ os.path.join( curPath, 'rtxmsg.exe' ), '%s' % (title, ), '%s'%(msg,) ]

        def outline( line ):
            print( line )

        p = SubProcess( rtxmsg, curPath, line_proc=outline, shell=False )
        p.run()
        p.wait()
        if p.returncode() != 0 :
            print( 'RTX 消息发送失败.', file=sys.stderr )
        # print( p.returncode() )

if __name__ == '__main__':
    robot = RTXRobot()
    robot.SendMsg('赵寅 - RTX 会话'.decode('utf-8').encode('gbk'), 'PY Robot test.')

