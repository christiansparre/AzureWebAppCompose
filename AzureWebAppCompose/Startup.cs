using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AzureWebAppCompose
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabase>(s =>
            {
                var mongoUrl = new MongoUrl(_configuration["MongoConnectionString"]);
                return new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMongoDatabase mongoDatabase)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (!(await mongoDatabase.ListCollectionNames().ToListAsync()).Contains("testcollection"))
                {
                    await mongoDatabase.CreateCollectionAsync("testcollection");
                }

                
                var list = await mongoDatabase.ListCollectionNames().ToListAsync();


                var builder = new StringBuilder();

                builder.AppendLine($"Hello World! Found {list.Count} collections");
                builder.AppendLine();
                builder.AppendLine("Environment variables");

                foreach (DictionaryEntry kv in Environment.GetEnvironmentVariables())
                {
                    builder.AppendLine($"{kv.Key} = '{kv.Value}'");
                }

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(builder.ToString());
            });
        }
    }
}
