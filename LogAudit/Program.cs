using LogAudit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.Extensions.Logging;

namespace LogAudit
{

    internal class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            //Configure necesary services
        
            var builder = new HostBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 //Add the necesary connector for your database engine
                 services.AddDbContext<KiwiSyslogContext>(options =>
                 {
                     options.UseSqlServer("");
                 });
             });


            var host = builder.Build();

            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            using (var serviceScope = host.Services.CreateScope())
            {
               var services = serviceScope.ServiceProvider;
               var logg = services.GetRequiredService<ILogger<Form1>>();

                //Create a Database Context and passing to the main form
               var kiwiSyslogContext = services.GetRequiredService<KiwiSyslogContext>();
               Application.Run(new Form1(kiwiSyslogContext));
 
               
            }
 
        }


    }
}
