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

        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");

        public IMongoCollection<BankAccount> BankAccounts =>
            _database.GetCollection<BankAccount>("BankAccounts");

        public IMongoCollection<CreditAccount> CreditAccounts =>
            _database.GetCollection<CreditAccount>("CreditAccounts");

        public IMongoCollection<SeInsurAccount> InsurAccounts =>
            _database.GetCollection<SeInsurAccount>("InsurAccounts");

        public IMongoCollection<ProvidentFundAccount> LifeInsurAccounts =>
            _database.GetCollection<ProvidentFundAccount>("LifeInsurAccounts");
        
        public IMongoCollection<PensionFundAccount> PensionAccounts =>
            _database.GetCollection<PensionFundAccount>("PensionAccounts");

        public IMongoCollection<MortgageInsurAccount> MortgageInsurAccounts =>
            _database.GetCollection<MortgageInsurAccount>("MortgageInsurAccounts");

        public IMongoCollection<StudyFundAccount> EfundAccounts =>
            _database.GetCollection<StudyFundAccount>("EfundAccounts");

        public IMongoCollection<Provider> Providers =>
            _database.GetCollection<Provider>("Providers");
        
        public IMongoCollection<Institution> Institutions =>
            _database.GetCollection<Institution>("Institutions");

        public IMongoCollection<Transaction> Transactions =>
            _database.GetCollection<Transaction>("Transactions");

        public IMongoCollection<Loan> Loans =>
            _database.GetCollection<Loan>("Loans");

        public IMongoCollection<ContactMessage> Messages =>
            _database.GetCollection<ContactMessage>("Messages");
    }
}
