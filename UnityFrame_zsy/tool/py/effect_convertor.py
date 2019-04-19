# -*- coding:utf-8 -*-

import os
import sys
import struct
from xml.dom import minidom

byte_order = '<'

#one byte flag
mat_flag = 0x01
mesh_flag = 0x02

vx_flag = 0x01
uv_flag = 0x02
col_flag = 0x04

class Vx:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

class Uv:
    def __init__(self, u, v):
        self.u = u
        self.v = v

# class Col:
#     def __init__(self, r, g, b):
#         self.r = r
#         self.g = g
#         self.b = b

class Face:
    def __init__(self, v0, v1, v2):
        self.v0 = v0
        self.v1 = v1
        self.v2 = v2

# class SubMesh:
#     def __init__(self, mat_name, face_list):
#         self.mat_name = mat_name
#         self.face_list = face_list

class Frame:
    def __init__(self, time, vx_list, uv_list):
        self.time = time
        self.vx_list = vx_list
        self.uv_list = uv_list
#         self.col_list = col_list

class Mesh:
    def __init__(self, name, mat_name, vx_list, uv_list, face_list, frame_list):
        self.name = name
        self.mat_name = mat_name
        self.vx_list = vx_list
        self.uv_list = uv_list
#         self.col_list = col_list
        self.face_list = face_list
        self.frame_list = frame_list

class Tex:
    def __init__(self, name, alpha):
        self.name = name
        self.alpha = alpha

class Mat:
    def __init__(self, name, tex_list):
        self.name = name
        self.tex_list = tex_list

def __parse(src_file):
    doc = minidom.parse(src_file)
    root = doc.documentElement
    assert root.tagName == "pwngs_effect", "this is not effect file"
    
    mat_list_root = root.getElementsByTagName("material_list")[0]
    mesh_list_root = root.getElementsByTagName("mesh_list")[0]

    mat_list = __proc_mat_list(mat_list_root)
    mesh_list = __proc_mesh_list(mesh_list_root)

    return mat_list, mesh_list

def __export(dst_file, mat_list, mesh_list):
    file = open(dst_file, 'wb')
    __exp_start(file)
    __exp_mat_list(file, mat_list)
    __exp_mesh_list(file, mesh_list)
    __exp_end(file)

def __proc_tex(elem_list):
    tex_list = []
    for elem in elem_list:
        name = __conv_tex_name(elem.getAttribute("name"))
        alpha = __conv_bool(elem.getAttribute("alpha"))
        tex_list.append(Tex(name, alpha))
    return tex_list

def __proc_mat_list(root):
    mat_list = []
    for elem in root.getElementsByTagName("material"):
        name = __conv_str(elem.getAttribute("name"))
        tex_list = __proc_tex(elem.getElementsByTagName("texture"))
        mat_list.append(Mat(name, tex_list))
    return mat_list

def __proc_mesh_list(root):
    mesh_list = []
    for mesh in root.getElementsByTagName("mesh"):
        name = __conv_str(mesh.getAttribute("name"))
        mat_name = __conv_str(mesh.getAttribute("material"))

        vx_list = []
        vx_elem_list = mesh.getElementsByTagName("vertex_list")
        if vx_elem_list:
            for vx_elem in vx_elem_list[0].getElementsByTagName("vertex"):
                vx_list.append(__conv_vx(vx_elem.getAttribute("x"),
                                         vx_elem.getAttribute("y"),
                                         vx_elem.getAttribute("z")))

        uv_list = []
        uv_elem_list = mesh.getElementsByTagName("texcoord_list")
        if uv_elem_list:
            for uv_elem in uv_elem_list[0].getElementsByTagName("texcoord"):
                uv_list.append(__conv_uv(uv_elem.getAttribute("u"),
                                         uv_elem.getAttribute("v")))

#         col_list = []
#         col_elem_list = mesh.getElementsByTagName("color_list")
#         if col_elem_list:
#             for col_elem in col_elem_list[0].getElementsByTagName("color"):
#                 col_list.append(__conv_col(col_elem.getAttribute("r"),
#                                            col_elem.getAttribute("g"),
#                                            col_elem.getAttribute("b")))

        face_list = []
        face_elem_list = mesh.getElementsByTagName("face_list")
        if face_elem_list:
            for face_elem in face_elem_list[0].getElementsByTagName("face"):
                face_list.append(__conv_face(face_elem.getAttribute("v0"),
                                             face_elem.getAttribute("v1"),
                                             face_elem.getAttribute("v2")))

#         submh_list = []
#         submh_elem_list = mesh.getElementsByTagName("submesh_list")
#         if submh_elem_list:
#             for sm_elem in submh_elem_list[0].getElementsByTagName("submesh"):
#                 mat_name = __conv_str(sm_elem.getAttribute("material_name"))
#                 face_list = []
#                 for face_elem in sm_elem.getElementsByTagName("face"):
#                     face_list.append(__conv_face(face_elem.getAttribute("v0"),
#                                                  face_elem.getAttribute("v1"),
#                                                  face_elem.getAttribute("v2")))
#                 submh_list.append(SubMesh(mat_name, face_list))

        frame_list = []
        frame_elem_list = mesh.getElementsByTagName("frame_list")
        if frame_elem_list:
            for fm_elem in frame_elem_list[0].getElementsByTagName("frame"):
                time = __conv_float(fm_elem.getAttribute("time"))

                fm_vx_list = []
                vx_elem_list = fm_elem.getElementsByTagName("vertex_list")
                if vx_elem_list:
                    for vx_elem in vx_elem_list[0].getElementsByTagName("vertex"):
                        fm_vx_list.append(__conv_vx(vx_elem.getAttribute("x"),
                                                    vx_elem.getAttribute("y"),
                                                    vx_elem.getAttribute("z")))

                fm_uv_list = []
                uv_elem_list = fm_elem.getElementsByTagName("texcoord_list")
                if uv_elem_list:
                    for uv_elem in uv_elem_list[0].getElementsByTagName("texcoord"):
                        fm_uv_list.append(__conv_uv(uv_elem.getAttribute("u"),
                                                    uv_elem.getAttribute("v")))

#                 col_list = []
#                 col_elem_list = fm_elem.getElementsByTagName("color_list")
#                 if col_elem_list:
#                     for col_elem in col_elem_list[0].getElementsByTagName("color"):
#                         col_list.append(__conv_col(col_elem.getAttribute("r"),
#                                                    col_elem.getAttribute("g"),
#                                                    col_elem.getAttribute("b")))
            
                frame_list.append(Frame(time, fm_vx_list, fm_uv_list))

        mesh_list.append(Mesh(name, mat_name, vx_list, uv_list, face_list,
                              frame_list))

    return mesh_list

def __conv_bool(val):
    return 1 if str(val) == "true" else 0

def __conv_float(val):
    return float(val)

def __conv_str(unicode):
    return str(unicode)

def __conv_tex_name(tex_name):
    return os.path.split(str(tex_name))[1]

def __conv_vx(x, y, z):
    return Vx(float(x), float(z), float(y))

def __conv_uv(u, v):
    return Uv(float(u), float(v))

def __conv_col(r, g, b):
    return Col(float(r), float(g), float(b))

def __conv_face(v0, v1, v2):
    return Face(int(v0), int(v1), int(v2))

def __exp_start(file):
    file.write(struct.pack(byte_order + 'h', len("effect")))
    file.write(struct.pack(byte_order + str(len("effect")) + 's', "effect"))
    file.flush()

def __exp_mat_list(file, mat_list):
    file.write(struct.pack(byte_order + 'B', mat_flag))
    p0 = file.tell()
    file.write(struct.pack(byte_order + 'i', 0))
    file.write(struct.pack(byte_order + 'i', len(mat_list)))
    for mat in mat_list:
        file.write(struct.pack(byte_order + 'h', len(mat.name)))
        file.write(struct.pack(byte_order + str(len(mat.name)) + 's', mat.name))
        file.write(struct.pack(byte_order + 'i', len(mat.tex_list)))
        for tex in mat.tex_list:
            file.write(struct.pack(byte_order + 'h', len(tex.name)))
            file.write(struct.pack(byte_order + str(len(tex.name)) + 's', tex.name))
            file.write(struct.pack(byte_order + 'B', tex.alpha))

    p1 = file.tell()
    length = p1 - p0 + 1 # 1 is 'B' length
    file.seek(p0)
    file.write(struct.pack(byte_order + 'i', length))
    file.seek(p1)
    file.flush()

def __exp_mesh_list(file, mesh_list):
    file.write(struct.pack(byte_order + 'B', mesh_flag))
    p0 = file.tell()
    file.write(struct.pack(byte_order + 'i', 0))

    flag0 = 0
    flag1 = 0
    for mesh in mesh_list:
        if mesh.vx_list:
            flag0 |= vx_flag
        if mesh.uv_list:
            flag0 |= uv_flag

        if mesh.frame_list:
            for frame in mesh.frame_list:
                if frame.vx_list:
                    flag1 |= vx_flag
                if frame.uv_list:
                    flag1 |= uv_flag

    file.write(struct.pack(byte_order + 'B', flag0))
    file.write(struct.pack(byte_order + 'B', flag1))

    file.write(struct.pack(byte_order + 'i', len(mesh_list)))
    for mesh in mesh_list:
        file.write(struct.pack(byte_order + 'h', len(mesh.name)))
        file.write(struct.pack(byte_order + str(len(mesh.name)) + 's', 
                               mesh.name))

        file.write(struct.pack(byte_order + 'h', len(mesh.mat_name)))
        file.write(struct.pack(byte_order + str(len(mesh.mat_name)) + 's',
                               mesh.mat_name))

        file.write(struct.pack(byte_order + 'i', len(mesh.face_list)))
        for face in mesh.face_list:
            file.write(struct.pack(byte_order + '3h', face.v0, face.v1, face.v2))
        file.flush()

        file.write(struct.pack(byte_order + 'i', len(mesh.vx_list)))
        for vx in mesh.vx_list:
            file.write(struct.pack(byte_order + '3f', vx.x, vx.y, vx.z))
        for uv in mesh.uv_list:
            file.write(struct.pack(byte_order + '2f', uv.u, uv.v))
#         for col in mesh.col_list:
#             file.write(struct.pack(byte_order + '3f', col.r, col.g, col.b))
        file.flush()

        file.write(struct.pack(byte_order + 'i', len(mesh.frame_list)))
        for frame in mesh.frame_list:
            file.write(struct.pack(byte_order + 'f', frame.time))
            for vx in frame.vx_list:
                file.write(struct.pack(byte_order + '3f', vx.x, vx.y, vx.z))
            for uv in frame.uv_list:
                file.write(struct.pack(byte_order + '2f', uv.u, uv.v))
#             for col in frame.col_list:
#                 file.write(struct.pack(byte_order + '3f', col.r, col.g, col.b))
            file.flush()

    p1 = file.tell()
    length = p1 - p0 + 1 # 1 is 'B' length
    file.seek(p0)
    file.write(struct.pack(byte_order + 'i', length))
    file.seek(p1)
    file.flush()    

def __exp_end(file):
    file.flush()
    file.close()

def convert(src_file, dst_file):
    if not os.path.isfile(src_file):
        raise Exception, "invalid src file: " + src_file

    sf = os.path.abspath(os.path.normpath(src_file))
    df = os.path.abspath(os.path.normpath(dst_file))
    mat_list, mesh_list = __parse(sf)
    __export(df, mat_list, mesh_list)

def convert_dir(src_dir, dst_dir, src_ext_name, dst_ext_name):
    if os.path.isfile(src_dir):
        if os.path.splitext(src_dir)[1] == src_ext_name:
            print 'conv file: ' + path
            convert(src_dir, 
                    os.path.join(dst_dir,
                                 os.path.splitext(os.path.split(src_dir)[1])[0]
                                 + dst_ext_name))
        return
        
    for path in os.listdir(src_dir):
        name, ext = os.path.splitext(path)
        abspath = os.path.join(src_dir, path)
        dstpath = os.path.join(dst_dir, name + dst_ext_name)
        if os.path.isfile(abspath):
            if ext == src_ext_name:
                print 'conv file: ' + path
                convert(abspath, dstpath)
        else:
            dst_path = os.path.join(dst_dir, path)
            if not os.path.exists(dst_path):
                os.mkdir(dst_path)
            convert_dir(abspath, dst_path, src_ext_name, dst_ext_name)

from config import *
if __name__ == '__main__':
	convert_dir(MAX_EFFECT_PATH, EXP_MAX_EFFECT_PATH,
                    MAX_EFFECT_SRC_EXT_NAME, MAX_EFFECT_DST_EXT_NAME)
