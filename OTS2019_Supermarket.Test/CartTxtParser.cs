using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    internal class CartTxtParser
    {
        public static IEnumerable<TestCaseData> GetTestCaseData(string fileName)
        {
            String path = $@"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCases = new List<TestCaseData>();
            bool isFirstLine = true;

            foreach(string line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                else if (string.IsNullOrWhiteSpace(line))
                    continue;

                String[] chars = line.Split('\t');
                int days = int.Parse(chars[0]);
                bool isWeekend = bool.Parse(chars[1]);
                int amount = int.Parse(chars[2]);
                int cartSize = int.Parse(chars[3]);
                int laptopCount = int.Parse(chars[4]);
                int monitorCount = int.Parse(chars[5]);
                int computerCount = int.Parse(chars[6]);
                int chairCount = int.Parse(chars[7]);
                int keyboardCount = int.Parse(chars[8]);
                double discount = double.Parse(chars[9]);

                DateTime targetData = GetStableTestDate(days, isWeekend);
                String stringDate = targetData.ToString("yyyy-MM-dd");

                testCases.Add(new TestCaseData(stringDate, amount, cartSize, laptopCount, monitorCount, computerCount, chairCount, keyboardCount, discount));
            }
            return testCases;
        }

        public static DateTime GetStableTestDate(int expectedDaysRange, bool isWeekend)
        {
            DateTime today = DateTime.Today;
            int startRange = (expectedDaysRange <= 3) ? -10 : 4;
            int endRange = (expectedDaysRange <= 3) ? 3 : 7;

            for(int i = startRange; i <= endRange; i++)
            {
                if (i == 0) continue;

                DateTime checkDate = today.AddDays(i);
                bool checkIsWeekend = (checkDate.DayOfWeek == DayOfWeek.Saturday ||
                    checkDate.DayOfWeek == DayOfWeek.Sunday);

                if (checkIsWeekend == isWeekend) return checkDate;
            }
            return today.AddDays(expectedDaysRange);
        }
    }
}
