using System;
using AutoMapper;
using DataProvider.Providers;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Mapping;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
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

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<String, DateTime>().ConvertUsing(new DateTimeTypeConverter());

                cfg.CreateMap<InstitutionDoc, InstitutionDto>();
                    
                cfg.CreateMap<BankAccount, BankAccountCreatingDto>();
                cfg.CreateMap<BankAccount, BankAccountDto>();

                cfg.CreateMap<CreditCard, _CreditCardCreatingDto>();
                cfg.CreateMap<_CreditCardCreatingDto, CreditCardDoc>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));

                cfg.CreateMap<CreditCard, CreditCardDto>();
                cfg.CreateMap<CreditCard, CreditCardDoc>();
                cfg.CreateMap<CreditCardDoc, CreditCardDto>();
                cfg.CreateMap<CreditCardDoc, CreditCard>();
                cfg.CreateMap<CreditCardDoc, CreditCardDescriptor>()
                    .ForMember(dest => dest.CardId, opt => opt.MapFrom(o => o.Id));

                cfg.CreateMap<BankAccountCreatingDto, BankAccountDoc>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));
                cfg.CreateMap<BankAccountDoc, BankAccountDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
                
                cfg.CreateMap<BankTransaction, TransactionDto>();
                cfg.CreateMap<BankTransaction, TransactionDoc>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(o => Guid.NewGuid()));

                cfg.CreateMap<Mortgage, MortgageDto>();
                cfg.CreateMap<Mortgage, MortgageDoc>()
                    .ForMember(dest => dest.InterestAmount, opt => opt.MapFrom(src => CalculateHelper.CalculateInterest(src)));

                cfg.CreateMap<Loan, LoanDto>();
                cfg.CreateMap<Loan, LoanDoc>();

                cfg.CreateMap<TransactionDoc, TransactionDto>();

                cfg.CreateMap<ProviderCreatingDto, ProviderDoc>();
                cfg.CreateMap<ProviderDoc, ProviderDto>();

                cfg.CreateMap<LeumiMortgageResponse, Mortgage>();
                cfg.CreateMap<LeumiLoanResponse, Loan>();

                CalMapping(cfg);
                HapoalimMapping(cfg);
            });
            app.UseMvc();
        }

        private static void CalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CalAccountResponse, CreditCardBankAccount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId));

            cfg.CreateMap<CalBankDebit, CreditCardDebitPeriod>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.CardLastDigits, opt => opt.MapFrom(src => src.CardLast4Digits));

            cfg.CreateMap<CalTransactionResponse, CreditCardTransaction>()
                .ForMember(dest => dest.DealAmount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.DebitAmount.Value));
        }

        private static void HapoalimMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<HapoalimAccountResponse, BankAccount>()
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.ProductLabel));
        }
    }
}
