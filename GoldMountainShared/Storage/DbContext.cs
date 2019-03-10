using GoldMountainShared.Storage.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoldMountainShared.Storage
{
    public class DbContext
    {
        public readonly IMongoDatabase _database = null;

        public DbContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public DbContext(DbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<UserDoc> Users =>
            _database.GetCollection<UserDoc>("Users");

        public IMongoCollection<BankAccountDoc> BankAccounts =>
            _database.GetCollection<BankAccountDoc>("BankAccounts");

        public IMongoCollection<CreditCardDoc> CreditAccounts =>
            _database.GetCollection<CreditCardDoc>("CreditAccounts");

        public IMongoCollection<SeInsurAccountDoc> InsurAccounts =>
            _database.GetCollection<SeInsurAccountDoc>("InsurAccounts");

        public IMongoCollection<ProvidentFundAccountDoc> LifeInsurAccounts =>
            _database.GetCollection<ProvidentFundAccountDoc>("LifeInsurAccounts");
        
        public IMongoCollection<PensionFundAccountDoc> PensionAccounts =>
            _database.GetCollection<PensionFundAccountDoc>("PensionAccounts");

        public IMongoCollection<MortgageInsurAccountDoc> MortgageInsurAccounts =>
            _database.GetCollection<MortgageInsurAccountDoc>("MortgageInsurAccounts");

        public IMongoCollection<StudyFundAccount> EfundAccounts =>
            _database.GetCollection<StudyFundAccount>("EfundAccounts");

        public IMongoCollection<ProviderDoc> Providers =>
            _database.GetCollection<ProviderDoc>("Providers");
        
        public IMongoCollection<InstitutionDoc> Institutions =>
            _database.GetCollection<InstitutionDoc>("Institutions");

        public IMongoCollection<TransactionDoc> Transactions =>
            _database.GetCollection<TransactionDoc>("Transactions");

        public IMongoCollection<MortgageDoc> Mortgages =>
            _database.GetCollection<MortgageDoc>("Mortgages");

        public IMongoCollection<MortgageDoc> Loans =>
            _database.GetCollection<MortgageDoc>("Loans");

        public IMongoCollection<ContactMessageDoc> Messages =>
            _database.GetCollection<ContactMessageDoc>("Messages");

        public IMongoCollection<ExclusiveLockStorageDoc> ExclusiveLock =>
            _database.GetCollection<ExclusiveLockStorageDoc>("ExclusiveLock");
    }
}
