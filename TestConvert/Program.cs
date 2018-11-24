using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestConvert.Dto;

namespace TestConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            RunScraper("transactions");
            //var text = "ביטוח לאומי";
            //var text = "פיוניר אינק";
            //Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");
            //Console.OutputEncoding = new UnicodeEncoding();
            //Console.WriteLine(text);
            //Console.WriteLine(ToUtf8(text));
            Console.ReadKey();
        }

        private static void RunScraper(string action)
        {
            string path = @"C:\Dev\Private\Softway\GoldMountain\CefScraper.Leumi\bin\Debug";
            string webScraperPath = Path.Combine(path, "CefScraper.Leumi.exe");

            ProcessStartInfo startInfo =
                new ProcessStartInfo(webScraperPath)
                {
                    Arguments = "MJAMQ2U lena1501 transactions 567000",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

            using (Process process = Process.Start(startInfo))
            {
                var output = process?.StandardOutput.ReadToEnd();
                string err = process?.StandardError.ReadToEnd();
                process?.WaitForExit();

                var account = output;
                if (!string.IsNullOrEmpty(account))
                {
                    LeumiAccountResponse newAccount = JsonConvert.DeserializeObject<LeumiAccountResponse>(account);
                    Console.WriteLine(newAccount);
                }
            }
        }
    }
}
