using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Dal.Repos;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace BE.Service
{
    public class Startup
    {

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(System.Environment.GetCommandLineArgs()
                    .Skip(1).ToArray());
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                    //.AllowCredentials();
                });
            });

            string connectionString = Configuration["ConnectionStrings:Identity"];

            services.AddDbContext<BEIdentityContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Identity"]));

            services.AddIdentity<AppUser, IdentityRole>(opts => {
                    opts.Password.RequiredLength = 4;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<BEIdentityContext>();

            services.AddControllersWithViews();

            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ISupplierRepo, SupplierRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<ICartLineRepo, CartLineRepo>();

            services.AddControllersWithViews()
                .AddJsonOptions(opts => {
                    opts.JsonSerializerOptions.IgnoreNullValues = true;
            }).AddNewtonsoftJson();
            services.AddRazorPages();

            //Disable for production
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "BE API", Version = "v1" });
            });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName = "dbo";
                options.TableName = "SessionData";
            });


                services.AddSession(options =>
                {
                    options.Cookie.Name = "BESession";
                    options.IdleTimeout = System.TimeSpan.FromHours(48);
                    options.Cookie.HttpOnly = false;
                    options.Cookie.IsEssential = true;
                });

            string key = Configuration[""];

            if ((Configuration["INITDB"] ?? "false") == "true")
            {
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(opts =>
                {
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["jwtSecret"])),
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                    opts.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async ctx =>
                        {
                            var usrmgr = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
                            var signinmgr = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                            string username = ctx.Principal.FindFirst(ClaimTypes.Name)?.Value;
                            AppUser idUser = await usrmgr.FindByNameAsync(username);
                            ctx.Principal = await signinmgr.CreateUserPrincipalAsync(idUser);
                        }
                    };
                });
                services.AddMvc();
            }
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services, IHostApplicationLifetime lifetime) {

            app.UseCors("AllowAll");
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //For development
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "dbo API");
            });

            //For deployment
            if ((Configuration["INITDB"] ?? "false") == "true")
            {
                System.Console.WriteLine("Preparing Database...");
                SeedData.SeedDataBase(services.GetRequiredService<BEIdentityContext>());
                System.Console.WriteLine("Database Preparation Complete");
                //Enable to deploy
                //lifetime.StopApplication();
            }

            //To create default admin and user
            BEIdentityContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            BEIdentityContext.CreateUserAccount(app.ApplicationServices, Configuration).Wait();

            //For development
            //app.UseSpa(spa =>
            //{
            //    string strategy = Configuration
            //        .GetValue<string>("DevTools:ConnectionStrategy");
            //    if (strategy == "proxy")
            //    {
            //        //spa.UseProxyToSpaDevelopmentServer("http://127.0.0.1:4200");
            //        spa.UseProxyToSpaDevelopmentServer("http://127.0.0.1:8100");
            //        //spa.UseProxyToSpaDevelopmentServer("http://192.168.2.4:8100");
            //    }
            //    else if (strategy == "managed")
            //    {
            //        spa.Options.SourcePath = "../ClientApp";
            //        spa.UseAngularCliServer("start");
            //    }
            //});
        }
    }
}
