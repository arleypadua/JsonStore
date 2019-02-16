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
                new SqlServerDocumentStore(new SqlConnection(connectionString)));

            return services;
        }
    }
}