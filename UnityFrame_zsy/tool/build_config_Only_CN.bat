@ECHO OFF

rem build py data
cd py
python build_config_byte.py CN
::�������뵽����
python copy_configcode_to_proj.py
cd ..
