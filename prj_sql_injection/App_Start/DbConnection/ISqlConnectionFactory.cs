using System.Data.SqlClient;

namespace prj_sql_injection.App_Start
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateSqlConnection();
    }
}
