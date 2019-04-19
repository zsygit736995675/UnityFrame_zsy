@ECHO OFF

rem build py data
cd py
python build_config_byte.py CN
::拷贝代码到工程
python copy_configcode_to_proj.py
cd ..
