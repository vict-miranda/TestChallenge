using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServerCustomers.Tests
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //var dbContextDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbContextOptions<ApplicationDbContext>));

                //services.Remove(dbContextDescriptor);

                //var dbConnectionDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbConnection));

                //services.Remove(dbConnectionDescriptor);

                //services.AddSingleton<DbConnection>(container =>
                //{
                //    var connection = new SqliteConnection("DataSource=:memory:");
                //    connection.Open();

                //    return connection;
                //});

                //services.AddDbContext<ApplicationDbContext>((container, options) =>
                //{
                //    var connection = container.GetRequiredService<DbConnection>();
                //    options.UseSqlite(connection);
                //});
            });

            builder.UseEnvironment("Development");
        }
    }
}
