using CoronaVirus.Domain;
using CoronaVirus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace CoronaVirus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("START ! ! !");

            var lastSavedDate = GetLastSavedDate();

            Console.WriteLine($"Last saved date - {lastSavedDate:MM-dd-yyyy}");

            for (var dt = lastSavedDate.AddDays(1); dt.Date < DateTime.UtcNow.Date; dt = dt.AddDays(1))
            {
                try
                {
                    var statisticList = new List<VirusStatisticResponseDto>();

                    using (var client = new HttpClient())
                    {
                        var dtString = dt.ToString("MM-dd-yyyy");

                        Console.WriteLine($"Making call for {dtString}");

                        Thread.Sleep(2000); //Just to leave more time to read

                        var url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + $"{dtString}.csv";
                        var response = client.GetAsync(url).Result;
                        var stringContent = response.Content.ReadAsStringAsync().Result;

                        var stringContentArray = stringContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (var line in stringContentArray)
                        {
                            var lineItems = line.Split(',');

                            if (lineItems.Length < 6)
                                continue;

                            string countryName = lineItems[3];

                            countryName = countryName.TrimStart().TrimEnd().Replace("\"", string.Empty).Replace("*", string.Empty);

                            if (countryName == "Mainland China")
                                countryName = "China";

                            bool isSouthKorea = countryName == "Korea";

                            if (!int.TryParse(lineItems[isSouthKorea ? 8 : 7], out int confirmed))
                                continue;

                            if (!int.TryParse(lineItems[isSouthKorea ? 9 : 8], out int deaths))
                                continue;

                            if (!int.TryParse(lineItems[isSouthKorea ? 10 : 9], out int recovered))
                                continue;

                            statisticList.Add(new VirusStatisticResponseDto
                            {
                                Confirmed = confirmed,
                                Deaths = deaths,
                                Country = countryName,
                                Recovered = recovered
                            });
                        }
                    }

                    Console.WriteLine($"Get {statisticList.Count} records");

                    var groupedList = statisticList.GroupBy(vs => vs.Country)
                        .Select(g => new
                        {
                            Deaths = g.Sum(i => i.Deaths),
                            Recovered = g.Sum(i => i.Recovered),
                            Confirmed = g.Sum(i => i.Confirmed),
                            Country = g.Key
                        });

                    Console.WriteLine($"Grouped into {groupedList.Count()} records");

                    using (var dbContext = new CoronaVirusDbContext())
                    {
                        var countries = dbContext.Countries.ToList();

                        foreach (var vs in groupedList)
                        {
                            var countryId = countries.FirstOrDefault(c => c.Name == vs.Country)?.Id;

                            if (countryId == null)
                            {
                                var country = new Country
                                {
                                    CreatedDateTime = DateTime.UtcNow,
                                    Name = vs.Country
                                };

                                dbContext.Countries.Add(country);

                                dbContext.SaveChanges();

                                countryId = country.Id;
                            }

                            dbContext.VirusStatistics.Add(new VirusStatistic
                            {
                                Confirmed = vs.Confirmed,
                                Recovered = vs.Recovered,
                                CountryId = countryId.Value,
                                Deaths = vs.Deaths,
                                CreatedDateTime = DateTime.UtcNow,
                                ReportDate = dt
                            });

                            dbContext.SaveChanges();

                            Console.WriteLine($"Saved for {vs.Country}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("FINISHED !!!");
        }

        static DateTime GetLastSavedDate()
        {
            using var dbContext = new CoronaVirusDbContext();

            var lastRecord = dbContext.VirusStatistics
                .OrderByDescending(x => x.ReportDate)
                .FirstOrDefault();

            if (lastRecord == null)
                return new DateTime(2020, 01, 22); //The first record in source https://github.com/CSSEGISandData/COVID-19/tree/master/csse_covid_19_data/csse_covid_19_daily_reports

            return lastRecord.ReportDate;
        }
    }
}
