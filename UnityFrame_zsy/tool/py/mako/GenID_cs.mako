using UnityEngine;
using System.Collections.Generic;

namespace ClientCore
{
class ${class_name}
{
		public static string Convert(int id)
		{
            if(id < 0 || id >= ID_COUNT) return null;
            else return names[id];
		}

        % for name in name_list:
        public const int ID_${name.replace('.', '_').upper()} = ${name_list.index(name)};
        % endfor
        public const int ID_COUNT = ${len(name_list)};
        
		private static readonly string[] names = new string[]
		{
            %for item in name_list:
            "${item}",
            %endfor
		};
}
}
