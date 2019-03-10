using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Models;

namespace DataProvider.Services
{
    public class CalculateHelper
    {
        public static Decimal CalculateInterest(Mortgage mortgage)
        {
            var rEff = Math.Pow((double)(1 + mortgage.InterestRate / (100 * 12)), 12) - 1;
            var rEffMonthly = Math.Pow(1 + rEff, (double)1 / 12) - 1;
            var n = CalculateHelper.CalculateNumberOfPayments(mortgage);
            var pMonthly = (mortgage.DeptAmount * (decimal)rEffMonthly) / (decimal)(1 - Math.Pow(1 / (1 + rEffMonthly), n));

            var result = Decimal.Round(pMonthly * n - mortgage.DeptAmount, 2, MidpointRounding.AwayFromZero);

            return result;
        }

        public static int CalculateNumberOfPayments(Mortgage mortgage)
        {
            var payoffYear = mortgage.EndDate.Year;
            var payoffMonth = mortgage.EndDate.Month;

            var now = DateTime.Now;
            var months = (payoffYear - now.Year) * 12;
            months -= now.Month + 1;
            months += payoffMonth;

            return months <= 0 ? 0 : months;
        }

        public static int ConvertDaysToMonthes(int days)
        {
            const double daysToMonths = 30.4368499;
            int months = (int)(days / daysToMonths);
            return months;
        }
    }
}
