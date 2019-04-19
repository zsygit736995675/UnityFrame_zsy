<%def name="read_func(prex, type, posx)">
<%
func = ''
if type.startswith("bool"):
    func = prex + "ReadBool" + posx
elif type.startswith("byte"):
    func = prex + "ReadByte" + posx
elif type.startswith("short"):
    func = prex + "ReadShort" + posx
elif type.startswith("int"):
    func = prex + "ReadInt" + posx
elif type.startswith("long"):
    func = prex + "ReadLong" + posx
elif type.startswith("float"):
    func = prex + "ReadFloat" + posx
elif type.startswith("double"):
    func = prex + "ReadDouble" + posx
elif type.startswith("string"):
    func = prex + "ReadString" + posx
%>
${func}
</%def>

<%def name="is_arr(arr)">
<%
tp = arr.rfind('[')
%>
${tp}
</%def>

<%doc>
<%def name="get_func(type, posx)">
<%
gfunc = ''
if type.startswith("bool"):
	if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "False;"
elif type.startswith("byte"):
	if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0;"
elif type.startswith("short"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0;"
elif type.startswith("int"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0;"
elif type.startswith("long"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0;"
elif type.startswith("float"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0.0f;"
elif type.startswith("double"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "0;"
elif type.startswith("string"):
    if ${is_arr(type)} != -1:
		gfunc = "new "+type+posx
	else:
    	gfunc = "string.Empty"+";"
%>
${gfunc}
</%def>
</%doc>

<%def name="type_of_arr(arr,bhave)">
<%
st = ""
tp = arr.rfind('[')
if tp != -1:
	if bhave:
		st = arr[0:tp]+"[]"
	else:
		st = arr[0:tp]
else:
	st = arr
%>
${st}
</%def>

<%
last_item_index = 0
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        last_item_index = i + 1
%>

<%
item_count = 0
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        item_count += 1
%>

<%
has_array_item = False
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        if arr_list[i]:
            has_array_item = True
            break
%>

using System;
using System.Collections.Generic;
using UnityEngine;


public class ${class_name} : BaseConfig
{
    public static bool resLoaded = false;
    private static Dictionary<int, ${class_name}> dic = new Dictionary<int, ${class_name}>();
    public static List<${class_name}> array = new List<${class_name}>();

    public static void Init()
    {
        LoadRes();
    }

    % for i in range(len(client_list)):
        % if client_list[i].count('c') > 0:
    public readonly ${type_of_arr(type_list[i],True)} ${name_list[i]};
        % endif
    % endfor

    public ${class_name}(\
        % for i in range(last_item_index - 1):
             % if client_list[i].count('c') > 0:
${type_of_arr(type_list[i],True)} ${name_list[i]}, \
             % endif
        % endfor
${type_of_arr(type_list[last_item_index - 1],True)} ${name_list[last_item_index - 1]}\
)
    {
        % for i in range(last_item_index):
             % if client_list[i].count('c') > 0:
        this.${name_list[i]} = ${name_list[i]};
             % endif
        % endfor
    }

    private static void OnLoadFile( byte[] data)
    { 
		ReadStream rs = new ReadStream(data);
        /*int file_len = */rs.ReadInt();
        string flag = rs.ReadString();
        if(flag != "${class_name}")
        {
            LogWarning("invalid file flag" + flag);
            return;
        }

        int col_cnt = rs.ReadShort();
        if(col_cnt != ${item_count})
        {
            LogWarning("col cnt invalid" + col_cnt + " : " + ${item_count});
            return;
        }

        int row_cnt = rs.ReadInt();
        for(int i = 0; i < row_cnt; i++)
        {
            Add_Item(rs);
        }

        resLoaded = true;
    }

    private static void Add_Item(ReadStream rs)
    {
        % if has_array_item:
            int arr_item_len_${class_name};
        % endif

        % for i in range(last_item_index):
            % if client_list[i].count('c') > 0:
                % if arr_list[i]:

        arr_item_len_${class_name} = rs.ReadShort();
        ${type_of_arr(type_list[i],True)} ${name_list[i]} = new ${type_of_arr(type_list[i],False)} [arr_item_len_${class_name}];

        for(int i = 0; i < arr_item_len_${class_name}; ++i)
            ${name_list[i]}[i] = ${read_func("rs.", type_list[i], "();")}
                % else:
        ${type_of_arr(type_list[i],True)} ${name_list[i]} = ${read_func("rs.", type_list[i], "();")}
                % endif
            % endif
        % endfor

        ${class_name} new_obj_${class_name} = new ${class_name}(\
        % for i in range(last_item_index - 1):
             % if client_list[i].count('c') > 0:
${name_list[i]}, \
             % endif
        % endfor
${name_list[last_item_index - 1]}\
);
        
        if(dic.ContainsKey(${name_list[0]}))
        {
            LogWarning("duplicate key: " + ${name_list[0]});
            return;
        }

        dic.Add(${name_list[0]}, new_obj_${class_name});
        array.Add(new_obj_${class_name});
    }

    private static void LoadRes()
    {
        if(resLoaded) return;
        byte[] data = GetAsset("${bin_file_name}");
		OnLoadFile(data);
    }

    public static ${class_name} GetConfig( int id )
    {
    	${class_name} config;
    	if ( dic.TryGetValue(id, out config ) )
    	{
    		return config;
    	}
    	else
    	{
    		return null;
    	}
    }
}

