using Building_Management.Infrastructure;
using Identity.Infrastructure;
using Identity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;

namespace BillZip
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.TokenValidationParameters =
                             new TokenValidationParameters
                             {
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,

                                 ValidIssuer = "Test.Security.Bearer",
                                 ValidAudience = "Test.Security.Bearer",
                                 IssuerSigningKey =
                                 Provider.JWT.JwtSecurityKey.Create("Test-secret-key-1234")
                             };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                        };

                    });

            //TODO: change these to be real Policies....
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Employee",
                    policy => policy.RequireClaim("EmployeeNumber"));
                options.AddPolicy("Hr",
                    policy => policy.RequireClaim("EmployeeNumber"));
                options.AddPolicy("Founder",
                    policy => policy.RequireClaim("EmployeeNumber", "1", "2", "3", "4", "5"));
            });

            services.AddMvc();
            services.AddEntityFrameworkNpgsql().AddDbContext<BuildingManagementContext>(opt => 
                opt.UseNpgsql(Configuration.GetConnectionString("BuildingManagementConnection")));

            services.AddEntityFrameworkNpgsql().AddDbContext<IdentityContext>(opt =>
                    opt.UseNpgsql(Configuration.GetConnectionString("IdentityConnection")))
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
#if RELEASE
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
#endif
                // User settings
                options.User.RequireUniqueEmail = true;
            });

#if DEBUG
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BillZip API", Version = "v1" });
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
#if DEBUG
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BillZip API V1");
            });
#endif
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
