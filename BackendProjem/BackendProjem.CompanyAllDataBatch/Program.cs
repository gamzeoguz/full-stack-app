using BackendProjem.CompanyAllDataBatch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackendProjem.Infrastructure;
using BackendProjem.Infrastructure.Services;
using BackendProjem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BackendProjem.CompanyAllDataBatch
{
    public class Program
    {
        static async Task Main(string[] args)
        {
                Console.WriteLine("Initialize..");

                IServiceCollection services = new ServiceCollection();

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);

                services.AddDbContext<CompanyDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("CompanyDb"));
                });
                services.AddTransient<ITestCompanyRepository, TestCompanyRepository>();
                services.AddTransient<ITestCompanyService, TestCompanyService>();
                services.AddIocContainer();

                await CompanyBootstrapper.Run();

                Console.WriteLine("Completed!");
                Console.ReadLine();
            }
        }
    }
