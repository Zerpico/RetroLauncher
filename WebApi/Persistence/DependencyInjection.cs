using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
               /* var cons = configuration.GetConnectionString("DbConnection");
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(cons);
               
                con.Open();
                var com = con.CreateCommand();
                
                com.CommandText = "create table sample ( id int )";
                com.ExecuteNonQuery();
                con.Close();*/
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DbConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
    }
}
