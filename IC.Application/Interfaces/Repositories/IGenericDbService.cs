namespace IC.Application.Interfaces.Repositories
{
    public interface IGenericDbService
    {
        Task<string> ExecQuerySqlReturnScalar(string connectionString, string sql, params object[] parameters);
    }
}
