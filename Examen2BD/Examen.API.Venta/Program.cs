using Examen.API.Venta.ContextoBD;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuartion = new ConfigurationBuilder()
        .AddJsonFile("local.settings.json", optional:true, reloadOnChange:true)
        .AddEnvironmentVariables()
        .Build();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<Contexto>(options => options.UseSqlServer(configuartion.GetConnectionString("cadenaConexion")));
    })
    .Build();

host.Run();