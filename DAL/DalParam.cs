using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DalParam
    {
        public string Name { get; set; }

        public SqlDbType ParamType { get; set; }

        public int Size { get; set; }
        
        public object Value { get; set; }

        public DalParam(string name, SqlDbType type, int size, object value)
        {
            Name = name;
            ParamType = type;
            Size = size;
            Value = value;
        }
    }
}
