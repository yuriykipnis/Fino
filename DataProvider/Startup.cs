using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataProvider.ErrorHandling;
using DataProvider.Providers;
using DataProvider.Providers.Interfaces;
using DataProvider.Services;
using GoldMountainShared.Models;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;
using GoldMountainShared.Models.Provider;
using GoldMountainShared.Models.Shared;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RawTransaction = DataProvider.Providers.Models.Transaction;
using Transaction = GoldMountainShared.Storage.Documents.Transaction;
using RawBankAccount = DataProvider.Providers.Models.BankAccount;
using RawCreditAccount = DataProvider.Providers.Models.CreditAccount;

namespace DataProvider
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
            services.AddMvc();
            services.Configure<DbSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });
            services.AddCors(o => o.AddPolicy("DevPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSingleton<IInstitutionRepository, InstitutionRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<ICreditAccountRepository, CreditAccountRepository>();
            services.AddSingleton<IProviderFactory, ProviderFactory>();
            services.AddTransient<IAccountService, AccountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevPolicy");
            }
            else
            {
                app.UseErrorWrapping();
                
                app.UseExceptionHandler(options =>
                {
                    options.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Ooops... something went wrong");
                    });
                });
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Institution, InstitutionDto>();
                    
                cfg.CreateMap<RawBankAccount, BankAccountCreatingDto>();
                cfg.CreateMap<RawBankAccount, BankAccountDto>();

                cfg.CreateMap<RawCreditAccount, CreditAccountCreatingDto>();
                cfg.CreateMap<RawCreditAccount, CreditAccountDto>();

                cfg.CreateMap<BankAccountCreatingDto, BankAccount>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));
                cfg.CreateMap<CreditAccountCreatingDto, CreditAccount>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));
                
                cfg.CreateMap<BankAccount, BankAccountDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
                cfg.CreateMap<CreditAccount, CreditAccountDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
                
                cfg.CreateMap<RawTransaction, TransactionDto>();
                cfg.CreateMap<RawTransaction, Transaction>()
                    .ForSourceMember(src => src.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));

                cfg.CreateMap<Transaction, TransactionDto>();

                cfg.CreateMap<ProviderCreatingDto, Provider>();

                cfg.CreateMap<Provider, ProviderDto>();
            });
            app.UseMvc();
        }
    }

   
}
