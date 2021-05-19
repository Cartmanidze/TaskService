using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Auth.Common.Configurations;
using Auth.Common.Extensions;
using Auth.Common.Handlers;
using GrpcFind;
using GrpcText;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TaskService.Context;
using TaskService.Profiles;
using TaskService.Repositories;
using TaskService.Services;

namespace TaskService
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
            services.AddDbContext<TaskContext>(opt =>
            {
                var connectionString = Configuration.GetConnectionString("AppConnectionString");
                opt.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });
            services.AddJwtAuth(Configuration);
            services.Configure<TokenConfiguration>(Configuration.GetSection(nameof(TokenConfiguration)));
            services.AddAutoMapper(typeof(TextTaskProfile));
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskResultRepository, TaskResultRepository>();
            services.AddTransient<ITaskService, Services.TaskService>();
            services.AddTransient<ITaskRunnerService, TaskRunnerService>();
            services.TryAddTransient<AuthHttpClientHandler>();
            services.AddGrpcClient<Find.FindClient>(opt => opt.Address = new Uri("https://localhost:5002")).ConfigurePrimaryHttpMessageHandler<AuthHttpClientHandler>();
            services.AddGrpcClient<Text.TextClient>(opt => opt.Address = new Uri("https://localhost:5001")).ConfigurePrimaryHttpMessageHandler<AuthHttpClientHandler>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
