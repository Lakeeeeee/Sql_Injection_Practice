using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace prj_sql_injection.App_Start
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        public SqlConnection CreateSqlConnection()
        {
            var entityConnectionString = ConfigurationManager.ConnectionStrings["NorthwindEntities"].ConnectionString;
            var entityConnBuilder = new EntityConnectionStringBuilder(entityConnectionString);
            var sqlConnectionString = entityConnBuilder.ProviderConnectionString;
            return new SqlConnection(sqlConnectionString);
        }
    }
}