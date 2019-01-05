using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataProvider.Providers;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Services;
using DistibutedLocking.Interfaces;
using DistributedLock;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RawMortgage = DataProvider.Providers.Models.Mortgage;
using Mortgage = GoldMountainShared.Storage.Documents.Mortgage;
using RawLoan = DataProvider.Providers.Models.Loan;
using Loan = GoldMountainShared.Storage.Documents.Loan;
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
            services.AddCors(o => o.AddPolicy("ProdPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => origin.Equals(
                        Configuration.GetSection("Host:Name").Value))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSingleton<IInstitutionRepository, InstitutionRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<ICreditAccountRepository, CreditAccountRepository>();
            services.AddSingleton<IProviderFactory, ProviderFactory>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<IExclusiveLockRepository, ExclusiveLockRepository>();
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
                app.UseExceptionHandler(options =>
                {
                    options.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Ooops... something went wrong");
                    });
                });
                app.UseCors("ProdPolicy");
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

                cfg.CreateMap<RawMortgage, MortgageDto>();
                cfg.CreateMap<RawMortgage, Mortgage>()
                    .ForMember(dest => dest.InterestAmount, opt => opt.MapFrom(src => CalculateInterest(src)));

                cfg.CreateMap<RawLoan, LoanDto>();
                cfg.CreateMap<RawLoan, Loan>();

                cfg.CreateMap<Transaction, TransactionDto>();

                cfg.CreateMap<ProviderCreatingDto, Provider>();

                cfg.CreateMap<Provider, ProviderDto>();

                cfg.CreateMap<LeumiMortgageResponse, RawMortgage>();
                cfg.CreateMap<LeumiLoanResponse, RawLoan>();
            });
            app.UseMvc();
        }

        public static Decimal CalculateInterest(RawMortgage mortgage)
        {
            var rEff = Math.Pow((double) (1 + mortgage.InterestRate / (100 * 12)), 12) - 1;
            var rEffMonthly = Math.Pow(1 + rEff, (double)1/12) - 1;
            var n = CalculateNumberOfPayments(mortgage);
            var pMonthly = (mortgage.DeptAmount * (decimal) rEffMonthly) / (decimal) (1 - Math.Pow(1 / (1 + rEffMonthly), n));

            var result = Decimal.Round(pMonthly * n - mortgage.DeptAmount, 2, MidpointRounding.AwayFromZero);
            
            //var monthes = ConvertDaysToMonthes((int)(mortgage.EndDate - mortgage.StartDate).TotalDays);
            //var interest = mortgage.DeptAmount * mortgage.InterestRate / 100 * monthes / 12;
            //return Decimal.Round(interest, 2, MidpointRounding.AwayFromZero);
            return result;
        }

        private static int CalculateNumberOfPayments(RawMortgage mortgage)
        {
            var payoffYear = mortgage.EndDate.Year;
            var payoffMonth = mortgage.EndDate.Month;

            var now = DateTime.Now;
            var months = (payoffYear - now.Year) * 12;
            months -= now.Month + 1;
            months += payoffMonth;

            return months <= 0 ? 0 : months;
        }

        private static int ConvertDaysToMonthes(int days)
        {
            const double daysToMonths = 30.4368499;
            int months = (int)(days / daysToMonths);
            return months;
        }
    }
}
