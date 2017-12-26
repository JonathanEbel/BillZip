using BillZip.Provider.JWT;
using Building_Management.Infrastructure;
using Identity.Infrastructure;
using Identity.Infrastructure.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

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

                                 ValidIssuer = UserJwtToken.tokenIssuer,
                                 ValidAudience = UserJwtToken.tokenAudience,
                                 IssuerSigningKey =
                                 JwtSecurityKey.Create(UserJwtToken.secretKey)
                                 
                             };
                        //TODO: turn these off in RELEASE unless they are logging....
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

            
            string authorizationPolicyNamespace = "BillZip.Policies";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == authorizationPolicyNamespace
                    select t;
            var list = q.ToList();
            
            foreach (var policy in list)
            {
                var props = policy.GetFields();
                string policyName = "";
                string requiredClaim = "";
                string[] requiredValues = { };

                policyName = (string)props.Where(prop => prop.Name == "PolicyName").First().GetRawConstantValue();
                requiredClaim = (string)props.Where(prop => prop.Name == "RequireClaim").First().GetRawConstantValue();
                requiredValues = (string[])props.Where(prop => prop.Name == "RequiredValues").First().GetValue(null);
                services.AddAuthorization(options =>
                {
                    if (requiredValues.Length == 0)
                        options.AddPolicy(policyName,
                            p => p.RequireClaim(requiredClaim));
                    else
                        options.AddPolicy(policyName,
                            p => p.RequireClaim(requiredClaim, requiredValues));
                });
            }
            
            services.AddMvc();
            services.AddEntityFrameworkNpgsql().AddDbContext<BuildingManagementContext>(opt => 
                opt.UseNpgsql(Configuration.GetConnectionString("BuildingManagementConnection")));

            services.AddEntityFrameworkNpgsql().AddDbContext<IdentityContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("IdentityConnection")));

            services.Configure<AppSettingsSingleton>(Configuration.GetSection("AppSettings"));

            //Set up any other items in my IoC container....
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

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
            app.UseMvcWithDefaultRoute();
        }
    }
}
