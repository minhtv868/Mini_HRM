using IC.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Application.Interfaces.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task<List<T>> ExecuteStoredProcedureGetAsync<T>(string storedProcedureName, Dictionary<string, object> parameters, List<StoredProcedureParameter> outputParameters=null) where T : class, new();
    }
}
