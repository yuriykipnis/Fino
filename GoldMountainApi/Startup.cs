using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoldMountainApi.Controllers.Helper;
using GoldMountainApi.Services;
using GoldMountainShared.Models;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Insur;
using GoldMountainShared.Models.Provider;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GoldMountainApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //options.RequireHttpsMetadata = false;
                options.Authority = Configuration["Authentication:Authority"];
                options.Audience = Configuration["Authentication:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Authentication:Authority"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:ClientSecret"]))
                };
            });

            services.Configure<DbSettings>(options =>
            {
                options.ConnectionString = Configuration["MongoConnection:ConnectionString"];
                options.Database = Configuration["MongoConnection:Database"];
            });

            services.AddCors(o => o.AddPolicy("DevPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddCors(o => o.AddPolicy("ProdPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => origin.Equals(
                        Configuration.GetSection("Host:Name").Value))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages",
            //        Configuration["Authentication:Authority"])));
            //});
            //services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<ICreditAccountRepository, CreditAccountRepository>();
            services.AddTransient<IInsurAccountRepository, InsurAccountRepository>();
            services.AddTransient<ILifeInsurAccountRepository, LifeInsurAccountRepository>();
            services.AddTransient<IEfundAccountRepository, EfundAccountRepository>();
            services.AddTransient<IPensionAccountRepository, PensionAccountRepository>();
            services.AddTransient<IMortgageInsurAccountRepository, MortgageInsurAccountRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IInstitutionRepository, InstitutionRepository>();
            services.AddTransient<IDataService, DataService>();
            services.AddSingleton<IValidationHelper, ValidationHelper>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevPolicy");
                app.UseAuthentication();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Ooops... something wrong happened");
                    });
                });

                app.UseCors("ProdPolicy");
                app.UseAuthentication();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
                cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
                cfg.CreateMap<Guid?, string>().ConvertUsing(g => g?.ToString("N"));
                cfg.CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));

                cfg.CreateMap<BankAccountCreatingDto, BankAccount>()
                    .ForMember(dest => dest.Id, _ => Guid.NewGuid());

                cfg.CreateMap<Transaction, TransactionDto>();
                cfg.CreateMap<TransactionDto, Transaction>();

                cfg.CreateMap<Loan, LoanDto>();

                cfg.CreateMap<BankAccount, BankAccountDto>();
                cfg.CreateMap<CreditAccount, CreditAccountDto>();
                cfg.CreateMap<BankAccountDto, BankAccount>();
                cfg.CreateMap<CreditAccountDto, CreditAccount>();

                cfg.CreateMap<SeInsurAccount, SeInsurAccountDto>();
                cfg.CreateMap<ProvidentFundAccount, ProvidentFundAccountDto>();
                cfg.CreateMap<StudyFundAccount, StudyFundAccountDto>();
                cfg.CreateMap<PensionFundAccount, PensionFundAccountDto>();
                cfg.CreateMap<MortgageInsurAccount, MortgageInsurAccountDto>();
            });

            app.UseMvc();
        }
    }
}
