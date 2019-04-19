# -*- coding:utf-8 -*-
# Author : yangtaibao
import os
from path_util import *
from config import *
from RuntimeOS import console
from Publish import ConfigParserNoCase

class Winscp(object):
    def __init__(self):
        self.i = 0;
        self.curDir = ''
        self.protocal = 'sftp'
        self.cmdLines = []
    
    def chgLocalDir(self, slocalPath):
        self.cmdLines.append('lcd ' + localPath + '\n')

    def chgRemoteDir(self,remotePath):
        self.cmdLines.append('cd ' + remotePath + '\n')

    def upload(self, fileName, localPath = '', remotePath = ''):   
        if not len(localPath) == 0:
            self.chgLocalDir(localPath)
        if not len(remotePath) == 0:
            self.chgRemoteDir(remotePath)

        fileName = os.path.join(localPath, fileName)
        if not os.path.exists(fileName):
            cmdLines = []
            return cmdLines;
        self.cmdLines.append('put ' + fileName + '\n')

    def uncompress(self, filename, distFolderName):
        cmdLine = 'call unzip ' + filename + ' -d ' +distFolderName+ ' \n'
        self.cmdLines.append(cmdLine)

    def editSoftLink(self, srcPath, linkFilePath):
        cmdLine = 'call rm -f ' + linkFilePath + '\n'
        cmdLine += 'call ln -sf ' + srcPath + ' ' + linkFilePath + '\n'
        self.cmdLines.append(cmdLine) 

    def login(self, username, pwd, host, protocal = 'sftp'):
        cmdLine = ''
        cmdLine += ('open '+ protocal + '://')
        cmdLine += (username+':'+pwd+'@'+ host + '\n')
        self.cmdLines.append(cmdLine)

    def execute(self, winscpPath):
        cmd = ''
        if os.path.exists(winscpPath):
            self.cmdLines += 'close \n'
            self.cmdLines += 'exit \n'
            scriptFile = open(os.path.join(winscpPath, 'script.txt'), 'w')
            scriptFile.writelines(self.cmdLines)
            scriptFile.close()

            cmd += winscpPath
            cmd += ('/WinSCP.com /script='+ winscpPath + '/script.txt')
            ret = os.system(cmd)

            force_del(os.path.join(winscpPath, 'script.txt') )
            return ret
        else:
            console.Error(winscpPath + 'does not exist, please set the value in client/releaseConf.txt ')
            os.sys.exit(-100)


if __name__ == '__main__':
    if len(sys.argv) > 1: 
        confUpdateFile = ConfigParserNoCase()
        confUpdateFile.read(os.path.join(ROOT_PATH, 'releaseConf.txt'))
        uploadfolderName = os.path.splitext(sys.argv[1])[0]
        dstPath = os.path.join(ROOT_PATH, "..\\") + uploadfolderName + '.zip'
        #remote work directory
        rwd = confUpdateFile.get('NET_PUBLISHING_CONF', 'rUploadDir')
        winscap = Winscp()
        winscap.login('bh', 'bhsj()%', '10.68.43.10')
        winscap.chgRemoteDir(rwd)
        winscap.upload(dstPath)
        winscap.uncompress(uploadfolderName + '.zip', uploadfolderName)
        rlinkpath = confUpdateFile.get('NET_PUBLISHING_CONF', 'rLinkFilePath')
        winscap.editSoftLink(rwd + uploadfolderName, rlinkpath)
        winscpPath = confUpdateFile.get('NET_PUBLISHING_CONF', 'WinSCPPath')
        winscap.execute(winscpPath)

            
        




