using System;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;
using SdaAutocompleteApi.Middlewares;
using SdaAutocompleteApi.Repositories;
using SdaAutocompleteApi.Services;
using SdaCommon.Models;

namespace SdaAutocompleteApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo {Title = "SdaAutocompleteApi", Version = "v1"});
			});

			var elasticConfig = GetElasticConfigOptions();
			var connectionPool = new StaticConnectionPool(new[] {new Uri(elasticConfig.Endpoint) });

			services.Configure<ElasticConfigOptions>(
				Configuration.GetSection(ElasticConfigOptions.ElasticConfig)
			);

			services.AddSingleton<IElasticClient>(
				new ElasticClient(
					new ConnectionSettings(connectionPool)
						.BasicAuthentication(elasticConfig.UserName, elasticConfig.Password)
						.RequestTimeout(elasticConfig.RequestTimeout)
						.MaximumRetries(elasticConfig.MaximumRetries)
				)
			);

			services.AddScoped<ISearchService, SearchService>();
			services.AddScoped<ISearchRepository, SearchRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SdaAutocompleteApi v1"));
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseMiddleware<ExceptionHandlerMiddleware>();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}

		private ElasticConfigOptions GetElasticConfigOptions() => 
			Configuration.GetSection(ElasticConfigOptions.ElasticConfig).Get<ElasticConfigOptions>();
	}
}