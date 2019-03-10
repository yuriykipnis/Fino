using System;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Mapping;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage.Documents;

namespace DataProvider.Test.Controllers
{
    public class TestBase
    {
        public static object MapperInitLock = new object();

        protected void InitializeMapper()
        {
            lock (MapperInitLock)
            {
                AutoMapper.Mapper.Reset();
                AutoMapper.Mapper.Initialize(cfg =>
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

                    cfg.CreateMap<CalAccountResponse, CreditCardBankAccount>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId));

                    cfg.CreateMap<CalBankDebit, CreditCardDebitPeriod>()
                        .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                        .ForMember(dest => dest.CardLastDigits, opt => opt.MapFrom(src => src.CardLast4Digits));

                    cfg.CreateMap<CalTransactionResponse, CreditCardTransaction>()
                        .ForMember(dest => dest.DealAmount, opt => opt.MapFrom(src => src.Amount.Value))
                        .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.DebitAmount.Value));

                    cfg.CreateMap<HapoalimAccountResponse, BankAccount>()
                        .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.ProductLabel));
                });
            }
        }
    }
}
