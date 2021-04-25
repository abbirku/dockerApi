using Autofac;
using Autofac.Extensions.DependencyInjection;
using Docker.Infrastructure;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.Seed;
using Docker.Infrastructure.SettingsModels;
using Docker.Membership.Contexts;
using Docker.Membership.Entities;
using Docker.Membership.Services;
using Docker.Membership.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Docker.WebApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Configuration = builder;

            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public static ILifetimeScope AutofacContainer { get; private set; }

        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var connectionStringName = "DockerApiDb";
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Startup).Assembly.FullName;

            builder.RegisterModule(new InfrastructureModule(connectionString, migrationAssemblyName));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStringName = "DockerApiDb";
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Startup).Assembly.FullName;

            //Binding appsettings.json EmailSettings properties with EmailSettings class
            services.AddSingleton(Configuration.GetSection("EmailSettings")
                .Get<EmailSettings>(options => options.BindNonPublicProperties = true));

            services.AddDbContext<ApiContext>(options =>
                       options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssemblyName)));

            services.AddDbContext<ApplicationDbContext>(options =>
                       options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssemblyName)));

            // Identity customization started here
            services
                .AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            //Configure password, username & email
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            //Authentication scheme
            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"]
                };
            });

            //Authorization policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Config.Admin);
                });

                options.AddPolicy("CommonPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Config.Admin, Config.User);
                });
            });

            services.AddCors();
            services.AddMvc();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebCamSeed webCamSeed)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            webCamSeed.MigrateAsync().Wait();
            webCamSeed.SeedAsync().Wait();
        }
    }
}
