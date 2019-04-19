# -*- coding:utf-8 -*-
# Author : yangtaibao

import os, io, re
import ConfigParser
import copy_res
import diff_bundle
from path_util import *
from config import *
from datetime import *
from hashlib import md5
from VerControlSVN import VerControl
from RuntimeOS import console
from RuntimeOS import RTXRobot


#To get or set conf file with no case-insensitive
class ConfigParserNoCase(ConfigParser.ConfigParser):
    def __init__(self, defaults = None):
        ConfigParser.ConfigParser.__init__(self, defaults = None);
    def optionxform(self, optionstr):
        return optionstr;

def outputLog():
    logFile = open(os.path.join(TOOL_PATH, "pack.log"), "r")
    logData = logFile.read()
    logFile.close()
    console.Error(logData)

class Publish(object):

    def __init__(self):
        dateTime  = datetime.now();
        self.releaConf = ConfigParserNoCase()
        self.releaConf.read(os.path.join(ROOT_PATH, 'releaseConf.txt'))

        wwwFolderPrefix = self.releaConf.get("RELEASE_CONF", 'WWWFolderNamePrefix') 
        self.wwwNewdirName = wwwFolderPrefix + dateTime.strftime("%Y_%m_%d_%H_%M_%S")
        self.wwwNewdirPath = os.path.join(ROOT_PATH, "../"+self.wwwNewdirName)
        self.wwwTmplDirName =  self.releaConf.get("RELEASE_CONF", 'TmplFolderName')
        self.wwwTmplDirPath = os.path.join(ROOT_PATH, "../"+self.wwwTmplDirName);
        self.miniDirName = self.releaConf.get("RELEASE_CONF", 'MiniFolderName')

        self.iniFileName = self.releaConf.get("INI", 'IniFileName')
        self.iniRelaPath = self.miniDirName + "/"+self.iniFileName
        self.iniFilePath = os.path.join(self.wwwNewdirPath, self.iniRelaPath);

        self.zipFilename = self.releaConf.get("INI", 'ZipName')
        zipSaveDir = self.releaConf.get("RELEASE_CONF", 'ZipRelaPath')
        self.clientZipPath = os.path.join(ROOT_PATH, zipSaveDir);

        self.binResDir =  os.path.join(BIN_PATH, "res");
        self.binwebResDir = os.path.join(BIN_PATH, "webres");
        self.wwwNewBinResDir = os.path.join(self.wwwNewdirPath, "res");
        self.wwwNewWebResDir = os.path.join(self.wwwNewdirPath, "webres")

        webSaveDir = self.releaConf.get("RELEASE_CONF", 'WebRelaPath')
        self.webPublishPath = os.path.join(ROOT_PATH, webSaveDir);
        self.webPublishName = self.releaConf.get("RELEASE_CONF", 'WebPublishName')

        self.elementRelaPath = self.releaConf.get("RELEASE_CONF", 'ElementRelaPath')

        self.bundlemapFileName = 'bundlemap.map.txt'
        self.bundlemapWebFileName = 'bundlemapweb.map.txt'

        HttpSrcRelaPath = self.releaConf.get("RELEASE_CONF", 'HttpResRelaPath')
        self.HttpSrcPath = os.path.join(ROOT_PATH, HttpSrcRelaPath)
        self.versionNum = ''
        
    #Copy wwwtmpl folder to new folder name www_x_x_x_x_x_x x is date number
    def CopyTmpldirToNewdir(self):
        if os.path.exists(self.wwwTmplDirPath):
            recurse_copy_folder(self.wwwTmplDirPath, self.wwwNewdirPath, '');
        else:
            console.Error("Error, there is no wwwtmpl folder in " + os.path.join(ROOT_PATH, '../') + "\n");
            sys.exit(-100)

    def callUnity(self, projectPah, methodName):
        cmd = "Unity.exe -quit -batchmode "
        param = "-projectPath "+projectPah + " -executeMethod " + methodName  + " -logFile " + os.path.join(TOOL_PATH, "pack.log")+ "  ";
        ret = os.system(cmd + param);
        return ret;

    #Compress for PC version
    def compressFolder(self, Path, dst):
        cmd = "7z.exe ";
        zipFilePath = dst
        param = "a " + zipFilePath + " " + Path+"/*";
        
        if os.path.exists(zipFilePath):
            os.remove(zipFilePath);

        ret =  os.system(cmd + param);
        if ret == 0:
            print("7z compress success...");
        else:
            print("7z cautch a error...");
            sys.exit();
    
    def copyAndRename(self):
        #Copy "bh.zip" file to www_x_x folder
        bhZipPath = os.path.join(self.clientZipPath,self.zipFilename);
        wwwMiniPath = os.path.join(self.wwwNewdirPath, self.miniDirName);
        zipFileMD5 = self.getFileMD5(bhZipPath);
        try:
            shutil.copy(bhZipPath, wwwMiniPath + "/"+zipFileMD5 +".zip");
            #Copy pwngs.unity to www_x_x folder and rename it with "MD5".unity
            webPublishSrcfilePath = os.path.join(self.webPublishPath, self.webPublishName);
            webPublishMD5 = self.getFileMD5(webPublishSrcfilePath );
            webPublishDstPath = self.wwwNewdirPath +"/" +webPublishMD5+".unity3d";
            shutil.copy(webPublishSrcfilePath, webPublishDstPath);
            #Edit www_x_x index.html "pwngs.unity3d" word to "%MD5%.unity3d"
            indexPath = os.path.join(self.wwwNewdirPath, "index.html");
            indexHtmlFile = open(indexPath, "r");
            indexHtmlTxt = indexHtmlFile.read();
            indexHtmlFile.close();
            indexHtmlTxt = indexHtmlTxt.replace("pwngs.unity3d",webPublishMD5+".unity3d");
            indexHtmlFile = open(indexPath, "w");
            indexHtmlFile.write(indexHtmlTxt);
            indexHtmlFile.close();
            self.webPublishMD5 = webPublishMD5;

            iniFile = ConfigParserNoCase();
            iniFile.read(self.iniFilePath);
            sectionVersion = "VERSION";
            keyText = "Text";
            keyGameVersion = "GameVersion"
            keyFileName = "FileName";
            #Update www_tmpl and www_x_x bh.ini values
            oldIniTextVal = iniFile.get(sectionVersion, keyText);
            oldIniGameVersionVal = iniFile.get(sectionVersion, keyGameVersion);
            newInitTextVal = str(int(oldIniTextVal) + 1 );
            newInitGameVersionVal = str(int(oldIniGameVersionVal) + 1 );
            self.versionNum = newInitTextVal;
            iniFile.set(sectionVersion, keyText, newInitTextVal);
            iniFile.set(sectionVersion, keyGameVersion, newInitGameVersionVal);
            iniFile.set(sectionVersion, keyFileName, zipFileMD5 + ".zip");
            iniFile.write(open(self.iniFilePath, 'w') );
            iniStr = open(self.iniFilePath,'r').read();
            
            tmpIniFilePath = self.wwwTmplDirPath + "/" + self.iniRelaPath;
            tmplIniFile = ConfigParserNoCase();
            tmplIniFile.read(tmpIniFilePath);
            tmplIniFile.set(sectionVersion, keyText, newInitTextVal);
            tmplIniFile.set(sectionVersion, keyGameVersion, newInitGameVersionVal);
            tmplIniFile.write(open(tmpIniFilePath, 'w'));
            
            #write date to version file in element folder of www_x_x..
            elemVersionPath = os.path.join(self.wwwNewdirPath, self.elementRelaPath);
            dateTime = datetime.now();
            elemVersionFile = open(elemVersionPath,'w+');
            elemVersionFile.write(dateTime.strftime("%Y%m%d%H%M%S"));
            elemVersionFile.close();

            console.Debug("Start to copy bin/(res | webres) to "+self.wwwNewdirName +'\n');
            recurse_copy_folder(self.binResDir,  self.wwwNewBinResDir, '');
            recurse_copy_folder(self.binwebResDir, self.wwwNewWebResDir, '');
            console.Debug("Copy success \n");
        except  Exception, e:
            print(e);
            sys.exit(-100);

    def removeFolder(self, path):
        shutil.rmtree(path);

    def getFileMD5(self, filePath):
        oMD5 = md5();
        try:
            file = open(filePath, "rb");
            oMD5.update(file.read());
        except Exception, e:
            print(e);
            raise e
            # sys.exit(-100);
        return oMD5.hexdigest();

    #--*--Call unity to pack resDep like "guidep.txt, CreatureSkillDep.txt ..."--*--
    def packResDep(self):
        ret = self.callUnity(RES_OTHER_PATH, "PackEntry.PackResDep");
        copy_res.copy_setting_only()
        if ret == 0:
            console.Info("Pack resdep success...\n");
        else:
            console.Error("Pack resdep failed...\n");
            outputLog();
            sys.exit(-100);

    def writeSignature(self):
        svn = VerControl()
        strSvnRetInfo = str(svn.info(os.path.join(CLIENT_BIN_RES_PATH, self.bundlemapFileName) ))
        if len(strSvnRetInfo) == 0:
            console.Error('Svn info ' + self.bundlemapWebFileName + ' error \n')
            return
        revisionWord = 'Revision:'
        startIdx = strSvnRetInfo.find(revisionWord);
        endIdx = strSvnRetInfo.find('\r', startIdx);
        version = strSvnRetInfo[startIdx + len(revisionWord): endIdx]
        versionPath = os.path.join(SRC_PROJ_PATH, 'EngineX/Engine/PackVersion.cs')
        versionTxt = 'public class PackVersion {public const string Version = \"' +\
                     datetime.now().strftime('%m/%d/%Y %H:%M:%S %p') + "\";" +\
                     'public const string SVNVersion = \"' + version.lstrip() + "\";}"

        fileTxt = open(versionPath, 'w');
        fileTxt.write(versionTxt);
        fileTxt.close();

    @staticmethod
    def updateResConfFile(folderPath, iniPath):

        def updateConfIImpl(confFilePath, toEditKeyValList):
            confFile = open(confFilePath, "r")
            lines = confFile.readlines()
            write_lines = []
            for line in lines:
                tmpLine = line
                line = line.split('=')[0].strip()
                for key, value in toEditKeyValList:  
                    if line == key:
                        tmpLine = key + ' = ' + value + '\n'
                        break;
                write_lines.append(tmpLine);

            confFile.close()
            confFile = open(confFilePath, "w")
            confFile.writelines(write_lines)
            confFile.close()

        console.Info("Start to update content of res/config.txt..\n ")
        if os.path.exists(folderPath):
            confUpdateFile = ConfigParserNoCase()
            confUpdateFile.read(os.path.join(ROOT_PATH, 'releaseConf.txt'))

            confFilePath = os.path.join(folderPath, 'res/config.txt');
            if not os.path.exists(confFilePath):
                console.Error('res/config.txt not exist...')
                return;
            toEditKeyValList = confUpdateFile.items('CONF_UPDATE_LIST')
            updateConfIImpl(confFilePath, toEditKeyValList)
            #To update webres/config.txt file
            confFilePath = os.path.join(folderPath, 'webres/config.txt');
            if not os.path.exists(confFilePath):
                console.Error('res/config.txt not exist...\n')
                return;
            updateConfIImpl(confFilePath, toEditKeyValList)

            #--*-- To Edit mini/bh.ini file
            toEditKeyValList = confUpdateFile.items('INI_UPDATE_LIST')
            iniFile = ConfigParserNoCase();
            iniFilePath = os.path.join(folderPath, iniPath)
            iniFile.read(iniFilePath);
            for key, value in toEditKeyValList:
                iniFile.set('VERSION', key, value)
            iniFile.write(open(iniFilePath, 'w'));

            gameName = confUpdateFile.get('GAME', 'GameName')
            console.Info('Update success...')
            return gameName;
        else:
            console.Error('Update failed, directory ' + folderPath + ' not exist...')



            
    def doPublish(self, versionName = ''):
        #--*--Call untiy to get creature resource dependency that save in client/Assets/ResDep/--*--
        gameName = ''
        try:
            ret = self.callUnity(SRC_PATH, "FirePack.PublishGame.GetCreatureDep"+versionName);
            if ret == 0:
                console.Info("Get creature dependcy resource success..\n");
            else:
                outputLog()
                console.Error("Get creature dependcy resource failed..\n");
        
            #--*-- Use svn to commit client/bin/res|webres directory  
            diff_bundle.DiffBundle(self.packResDep);
            currDir = os.getcwd()
            msg = 'Auto ver publish commit.'
            svn = VerControl()
            svn.add(os.path.join(DST_RES_PATH, BUNDLEMAP_FILE),currDir)
            svn.commit(os.path.join(DST_RES_PATH, BUNDLEMAP_FILE),msg,currDir)
            svn.add(os.path.join(DST_WEB_RES_PATH, BUNDLEMAP_FILE),currDir)
            svn.commit(os.path.join(DST_WEB_RES_PATH, BUNDLEMAP_FILE),msg,currDir)

            #to update PackVersion.cs file --*--
            self.writeSignature()   

            #--*--Call unity to build game--*--
            ret = self.callUnity(SRC_PATH, "FirePack.PublishGame.DoPublishing");
            if ret == 0:
                console.Info("Build game success...\n");
            else:
                console.Error("Build game failed.\n");
                outputLog()
                sys.exit(-100);

            self.CopyTmpldirToNewdir();
            dstZipPath = os.path.join(self.clientZipPath, self.zipFilename)
            self.compressFolder(self.clientZipPath, dstZipPath);
            self.copyAndRename();

            #Remove this folder created by unity buildPipline
            self.removeFolder(self.clientZipPath)
            self.removeFolder(self.webPublishPath)

            gameName = self.updateResConfFile(self.wwwNewdirPath, self.iniRelaPath)
        except Exception, e:
            console.Error(e)
            raise e
        self.gameName = gameName;
        
if __name__ == '__main__':
    versionName = '';
    if len(sys.argv) > 1:
        versionName = sys.argv[1];
    p = Publish();
    p.doPublish(versionName);
    msg = p.gameName + (u'项目，机器人自动打版完成， 版本号' + p.versionNum).encode('gbk')
    '''
    confUpdateFile = ConfigParserNoCase()
    confUpdateFile.read(os.path.join(ROOT_PATH, 'releaseConf.txt'))
    gameName = confUpdateFile.get('GAME', 'GameName')
    versionNum = u'1070'
    msg = gameName + (u'项目，机器人自动打版完成， 版本号' +versionNum).encode('gbk')
    
    '''
    dstPath = p.wwwNewdirPath + '.zip'
    srcPath = p.wwwNewdirPath
    #p.compressFolder(srcPath, dstPath)
    title = u'N-game丨unity小组 - RTX 会话'.encode('gbk') 
    rtxRobot = RTXRobot()
    rtxRobot.SendMsg(title, msg)
    
    
    
