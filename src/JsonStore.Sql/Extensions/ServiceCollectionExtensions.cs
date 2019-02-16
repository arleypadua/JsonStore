using System.Data.SqlClient;
using JsonStore.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace JsonStore.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerJsonStore(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IStoreDocuments, SqlServerDocumentStore>(provider =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();

                return new SqlServerDocumentStore(connection);
            });

            return services;
        }
    }
}