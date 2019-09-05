using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using DataAccess.Persistence;
using DataAccess.Persistence.Log;
using DataAccess.Persistence.RetailLink;
using DataAccess.Persistence.UnitOfWork;
using DataAccess.Persistence.Consolidate;
using DataAccess.Persistence.CashReceipt;
using DataAccess.Services;
using Model.Model;
using Business.RetailLink;
using Business.SynapseItemGenerator;
using Business.Consolidate;
using Business.CashReceipt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace rjwtoolapi
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
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IRetailLinkRepository, RetailLinkRepository>();
            services.AddScoped<IConsolidateRepository, ConsolidateRepository>();
            services.AddScoped<ICashReceiptRepository, CashReceiptRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<RJWDataDbContext>(
                options => options.UseSqlServer(Configuration["SQLP01_RJWData"]
                /*,
                sqlServerOptions => sqlServerOptions.CommandTimeout(Int32.Parse(Configuration["SQLServerTimeOut"]))*/
                )
            );
            services.Configure<Config>(Configuration);
            //services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });          

            services.AddMvc();

            // configure jwt authentication            
            var key = Encoding.ASCII.GetBytes(Configuration["UserServiceConfig:Key"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRetailLinkService, RetailLinkService>();
            services.AddScoped<ISynapseItemGeneratorService, SynapseItemGeneratorService>();
            services.AddScoped<ICashReceiptService, CashReceiptService>();
            services.AddScoped<IConsolidateService, ConsolidateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseMvc();            
        }
    }
}
