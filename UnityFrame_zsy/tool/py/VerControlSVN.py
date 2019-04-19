# -*- coding:utf-8 -*-
from __future__ import print_function
import unittest
from RuntimeOS import SubProcess, console

class VerControl :
    def __init__( self ):
        pass

    def cleanup( self, path, runDir = '.', line_proc=None ):
        cmd = 'svn cleanup "%s"' % (path, )
        return self.__proc( cmd, runDir, line_proc )
    
    def state( self, path, runDir = '.', line_proc=None ):
        cmd = 'svn st "%s"' % (path, )
        return self.__proc( cmd, runDir, line_proc )

    def add( self, path, runDir = '.' ):
        cmd = 'svn add --force "%s"' % (path, )
        return self.__proc( cmd, runDir )

    def rm( self, path, runDir = '.' ):
        cmd = 'svn rm "%s"' % (path, )
        return self.__proc( cmd, runDir )

    def revert( self, path, runDir = '.' ):
        cmd = 'svn revert -R "%s"' % (path,)
        return self.__proc( cmd, runDir )

    def update( self, path, runDir = '.' ):
        cmd = 'svn up --force --accept theirs-full "%s"' % (path,)
        return self.__proc( cmd, runDir )

    def commit( self, path, msg, runDir = '.' ):
        cmd = 'svn ci %s -m "%s"' % (path, msg,)
        print(cmd)
        return self.__proc( cmd, runDir )

    def info(self, path, runDir = '.'):
        cmd = 'svn info "%s"' % (path, )
        self.strInfo = ''
        def CatchOut( line ):
            self.strInfo += line
        self.__proc( cmd, runDir, CatchOut )
        return self.strInfo
        # line_proc = None
        # proc = SubProcess( cmd, runDir, line_proc)
        # proc.run()
        # strInfo = ''
        # for line in proc.stdout:
        #     strInfo += line
        # return strInfo

    def __proc( self, cmd, runDir, line_proc=None ):
        proc = SubProcess( cmd, runDir, line_proc=line_proc )
        proc.run()
        if line_proc == None:
            for line in proc.stdout:
                console.Info( line )
        return proc.returncode()

# if __name__ == '__main__':
#     svn = VerControl()
#     print( svn.info( '.' ) )
