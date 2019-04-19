# -*- coding:utf-8 -*-

from __future__ import print_function
import config as conf
import os, io, re, sys
from RuntimeOS import SubProcess, console
from VerControlSVN import VerControl

class BundleIdxMapDic() :
    def __init__( self ):
        self.dic = {}

    @staticmethod
    def Load( filename ):
        '''Load bundlemap.map.txt in to { name : [final_name, size, md5, ver, cut], }'''
        idxmap = BundleIdxMapDic()
        try:
            with io.open(filename, 'r', encoding='utf-8') as infile:
                for line in infile:
                    name, final_name, size, md5, ver, cut = line.split(':')
                    idxmap.dic[name] = [final_name, size, md5, ver, cut]
                    # print( md5, ' : ', name )
        except :
            print( u'First Build', file=sys.stderr )
        return idxmap

    def __sub__( self, other ):
        ''' A diff B '''
        diff = BundleIdxMapDic()
        diffDic = diff.dic

        for name, value in self.dic.iteritems():
            if name in other.dic:
                final_name, size, md5, ver, cut = other.dic[name]
                if value[2] == md5:
                    continue
            diffDic[name] = value

        return diff

    def __repr__( self ):
        s = ''
        for name, value in self.dic.iteritems():
            if s != '':
                s += '\n'
            final_name, size, md5, ver, cut = value
            s += '%s : %s %s' % (name, md5, ver)
        return s


def DiffBundle( package_func ) :
    bundlemap_res = os.path.abspath( os.path.join(conf.CLIENT_BIN_RES_PATH, conf.BUNDLEMAP_FILE) )
    bundlemap_web = os.path.abspath( os.path.join(conf.CLIENT_BIN_WEB_PATH, conf.BUNDLEMAP_FILE) )

    res_old = BundleIdxMapDic.Load( bundlemap_res )
    web_old = BundleIdxMapDic.Load( bundlemap_web )

    # run unity package
    if package_func != None:
        package_func()

    res_new = BundleIdxMapDic.Load( bundlemap_res )
    web_new = BundleIdxMapDic.Load( bundlemap_web )

    del_res = res_old - res_new
    add_res = res_new - res_old
    
    del_web = web_old - web_new
    add_web = web_new - web_old

    console.Title( u'自动化处理' )
    # console.clear()

    #res
    filePath = conf.CLIENT_BIN_RES_PATH
    rmRes(filePath,del_res)
    addRes(filePath,add_res)
        
    #web
    filePath = conf.CLIENT_BIN_WEB_PATH  
    rmRes(filePath,del_web)
    addRes(filePath,add_web)
    
    # svn.commit( '.', u'自动化构建工具提交.' )
    # print( res_old.__class__.__name__ )
    
    
def rmRes(filePath, res ):
    svn = VerControl()
    currDir = os.getcwd()
    msg = 'auto ver build commit.'
    for name, value in res.dic.iteritems():
        final_name, size, md5, ver, cut = value
        delPath = os.path.join(filePath, final_name)      
        #print("")
        #print("del path " + delPath + "   name: " + name)
        svn.rm(delPath, currDir)
    svn.commit(filePath, msg, currDir)

def addRes(filePath, res ):
    svn = VerControl()
    currDir = os.getcwd()
    msg = 'auto ver build commit.'
    for name, value in res.dic.iteritems():
        final_name, size, md5, ver, cut = value
        addPath = os.path.join(filePath, final_name)
        #print("")
        #print("add path " + addPath + "   name: " + name)
        svn.add(addPath, currDir)
    svn.commit(filePath, msg, currDir)
    

if __name__ == '__main__':
    DiffBundle( None )

