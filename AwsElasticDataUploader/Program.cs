using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace SDAElasticAutoComplete
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
				.UseSerilog((context, configuration) =>
				{
					configuration.Enrich.FromLogContext()
						.WriteTo.Console()
						.WriteTo.Elasticsearch(
							new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfig:Endpoint"]))
							{
								IndexFormat = $"{Assembly.GetEntryAssembly()!.GetName().Name}-logs-{DateTime.UtcNow:yyyy-MM-dd}",
								AutoRegisterTemplate = true,
								NumberOfShards = 2,
								NumberOfReplicas = 1,
								ModifyConnectionSettings = x => 
									x.BasicAuthentication(
										context.Configuration["ElasticConfig:UserName"], 
										context.Configuration["ElasticConfig:Password"])
							});
				});
	}
}