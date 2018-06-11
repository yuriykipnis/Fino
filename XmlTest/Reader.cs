using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using GoldMountainShared.Storage.Repositories;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MaslekaReader
{
    public class Reader
    {
        private readonly IInsurAccountRepository _insureAccountRepository;
        private readonly ILifeInsurAccountRepository _lifeInsureAccountRepository;
        private readonly IEfundAccountRepository _efundAccountRepository;
        private readonly IPensionAccountRepository _pensionAccountRepository;
        private readonly IMortgageInsurAccountRepository _mortgageInsurAccountRepository;

        public Reader()
        {
            DbSettings dbSettings = new DbSettings {
                ConnectionString = "mongodb://admin:abc123!@localhost",
                Database = "DataProviderDb"
            };

            _insureAccountRepository = new InsurAccountRepository(dbSettings);
            _lifeInsureAccountRepository = new LifeInsurAccountRepository(dbSettings);
            _efundAccountRepository = new EfundAccountRepository(dbSettings);
            _pensionAccountRepository = new PensionAccountRepository(dbSettings);
            _mortgageInsurAccountRepository = new MortgageInsurAccountRepository(dbSettings);
        }

        public async void GenerateEfundAccounts(String id)
        {
            var data = ReadFromFiles(id);

            var builder = new StudyFundBuilder();
            var accounts = builder.CreateAccounts(data, id);

            await _efundAccountRepository.AddAccounts(accounts);
        }

        public async void GenerateInsurAccounts(String id)
        {
            var data = ReadFromFiles(id);

            var builder = new SeInsurBuilder();
            var accounts = builder.CreateAccounts(data, id);

            await _insureAccountRepository.AddAccounts(accounts);
        }

        public async void GenerateLifeInsurAccounts(String id)
        {
            var data = ReadFromFiles(id);

            var builder = new ProvidentFundBuilder();
            var accounts = builder.CreateAccounts(data, id);

            await _lifeInsureAccountRepository.AddAccounts(accounts);
        }


        public async void GeneratePensionAccounts(String id)
        {
            var data = ReadFromFiles(id);

            var builder = new PensionFundBuilder();
            var accounts = builder.CreateAccounts(data, id);

            await _pensionAccountRepository.AddAccounts(accounts);
        }

        public async void GenerateMortgageInsurAccounts(String id)
        {
            var data = ReadFromFiles(id);

            var builder = new MortgageInsurBuilder();
            var accounts = builder.CreateAccounts(data, id);

            await _mortgageInsurAccountRepository.AddAccounts(accounts);
        }

        public static DateTime ConvertStringToDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return DateTime.MaxValue;
            }

            var year = date.Substring(0, 4);
            var month = date.Substring(4, 2);
            var day = date.Substring(6, 2);
            return new DateTime(Convert.ToInt16(year), Convert.ToInt16(month), Convert.ToInt16(day));
        }

        public IEnumerable<Mimshak> ReadFromFiles(String id)
        {
            //var path = Directory.GetCurrentDirectory() + @"\Data\SwiftNess_303954861_520023185_INP_201712010806_1.xml";
            //var path = Directory.GetCurrentDirectory() + @"\Data\SwiftNess_303954861_513611509_KGM_201712010806_2.xml";
            //var path = Directory.GetCurrentDirectory() + @"\Data\SwiftNess_303954861_512245812_PNN_201712010806_8.xml";
            IList<Mimshak> result = new List<Mimshak>();

            try
            {
                var folderPath = Path.Combine(@"C:\Dev\Private\Softway\GoldMountain\XmlTest", Path.Combine("Data", id));
                foreach (var file in Directory.GetFiles(folderPath)
                    .Where(fileName => Path.GetExtension(fileName).Equals(".xml")))
                {
                    var mimshak = DeserializeXmlFileToObject<Mimshak>(Path.Combine(folderPath, file));
                    result.Add(mimshak);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        private T DeserializeXmlFileToObject<T>(string xmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xmlFilename)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(xmlFilename);

                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlSerializer serializer = new XmlSerializer(typeof(T), overrides);
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return returnObject;
        }
    }
}
