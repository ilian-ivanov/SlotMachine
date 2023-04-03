using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlotMachine.Models;
using SlotMachine.Services;

namespace SlotMachine
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScoped<IGameService, GameService>();
                    services.AddScoped<IInputOutputService, ConsoleInputOutputService>();
                    services.AddScoped<ISlotMachine, Models.SlotMachine>();
                    services.AddScoped<IRandomWithProbabilities, RandomWithProbabilities>();
                    services.Configure<SlotMachineConfiguration>(options => config.GetSection("Settings").Bind(options));
                })
                .Build();

            var game = host.Services.GetService<IGameService>();
            game.Start();
        }
    }
}