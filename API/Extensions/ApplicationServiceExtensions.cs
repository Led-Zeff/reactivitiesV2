using System;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public const string CORS_POLICY = "CorsPolicy";

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(opt =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string connString;

                if (env == "Production") {
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
                        .Replace("postgres://", string.Empty);
                    
                    var userPass = connUrl.Split("@")[0];
                    var hostPortDb = connUrl.Split("@")[1];
                    var hostPort = hostPortDb.Split("/")[0];
                    var db = hostPortDb.Split("/")[1];
                    var user = userPass.Split(":")[0];
                    var pass = userPass.Split(":")[1];
                    var host = hostPort.Split(":")[0];
                    var port = hostPort.Split(":")[1];

                    connString = $"Server={host}; Port={port}; User Id={user}; Password={pass}; Database={db}; SSL Mode=Require; Trust Server Certificate=true";
                } else {
                    connString = config.GetConnectionString("DefaultConnection");
                }

                opt.UseNpgsql(connString);
            });

            services.AddCors(opt => {
                opt.AddPolicy(CORS_POLICY, policy => {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate", "Pagination")
                        .WithOrigins("http://localhost:3000");
                });
            });

            services.AddMediatR(typeof(ListAll.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddSignalR();

            return services;
        }
    }
}