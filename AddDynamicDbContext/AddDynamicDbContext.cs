using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbContextExtensions
{
    public static class AddDynamicDbContext
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

        private static readonly string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private static readonly string environment = string.IsNullOrEmpty(ASPNETCORE_ENVIRONMENT) ? "Production" : ASPNETCORE_ENVIRONMENT;

        /// <summary>This method adds dynamic ConnectionString to your DbContext for Development and Production environments.</summary>
        /// <param name="connectionStringKey">The name of ConnectionString in apsettings.json</param>
        /// <param name="provider">Select context provider.</param>
        /// <param name="debug">Use the Production environment while in Debug mode.</param>
        public static IServiceCollection AddDDbContext<TContext>(this IServiceCollection context, string connectionStringKey, Provider provider = Provider.SqlServer, bool debug = false) where TContext : DbContext
        {
            string connectionString = Configuration.GetConnectionString($"{connectionStringKey}:{environment}");

            // check key exists
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"{connectionStringKey}:{environment} key was not found in the ConnectionString section in the appsettings.json");
            }

            // check debug/production
#if DEBUG
            if (environment == "Production" && debug == false)
            {
                throw new Exception("You cannot use the Production database in Debug mode.");
            }
#endif


            // check connectionString is valid
            DbConnectionStringBuilder dbConnectionString = new DbConnectionStringBuilder();

            try
            {
                dbConnectionString.ConnectionString = connectionString;
            }
            catch (Exception exp)
            {
                throw exp;
            }

            Dictionary<Provider, Action<DbContextOptionsBuilder>> optionsAction = new Dictionary<Provider, Action<DbContextOptionsBuilder>>()
            {
                { Provider.SqlServer, options => { options.UseSqlServer(connectionString); } },
                { Provider.MySQLServer, options => { options.UseMySQL(connectionString); } },
                
            };


            return context.AddDbContext<TContext>(optionsAction[provider]);
        }
    }

    public enum Provider
    {
        SqlServer = 1,
        MySQLServer = 2,
    }
}
