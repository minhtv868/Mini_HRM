using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Application.DTOs
{
    public class StoredProcedureParameter
    {

        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType Type { get; set; }
        public ParameterDirection Direction { get; set; }

        public StoredProcedureParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            Name = name;
            Type = type;
            Direction = direction;
        }
    }
}
