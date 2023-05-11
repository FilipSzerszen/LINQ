using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

// ReSharper disable UseFormatSpecifierInInterpolation

namespace FirstProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = @"C:\Users\Lenovo\source\repos\FirstProject\googleplaystore2.csv";
            var googleApps = LoadGoogleAps(csvPath);

            //Display(googleApps);
            //GetData(googleApps);
            //ProjectData(googleApps);
            //DivideData(googleApps);
            //OrderData(googleApps);
            //DataSetOperation(googleApps);
            DataVeryfication(googleApps);

        }

        static void DataVeryfication(IEnumerable<GoogleApp> googleApps)
        {
            var allOperatorResult = googleApps.Where(w => w.Category == Category.WEATHER);
            Display(allOperatorResult);
        }

        static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
        {
            var paidAppCategories = googleApps
                .Where(p => p.Type == Type.Paid)
                .Select(e => e.Category)
                .Distinct();
            //Console.WriteLine(string.Join(";\r\n", paidAppCategories));

            var setA = googleApps
                .Where(r => r.Rating > 4.7 && r.Type == Type.Paid &&  r.Reviews>1000);

            var setB = googleApps
                .Where(r => r.Rating > 4.6 && r.Name.Contains("Pro")  && r.Reviews > 10000);

            var setC = setA.Intersect(setB); //. (...Union...)   (...( (..Intersect..) )...)    (..Except..( (...)
            Display(setC);
        }

        static void OrderData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps
                .Where(r => r.Rating > 4.4 && r.Category == Category.BEAUTY)
                .OrderByDescending(e=>e.Rating)
                .ThenByDescending(r=>r.Reviews);
            Display(highRatedApps);
        }

        static void DivideData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.TakeWhile(o => o.Reviews > 100).Skip(5);
            Display(highRatedApps);
        }

        static void ProjectData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps
                .Where(r => r.Rating > 4.6 && r.Category == Category.BEAUTY);
            var highRatedBeautyApps = highRatedApps.Select(r => r.Name);
            Console.WriteLine(string.Join(", ", highRatedApps));


            var dtos = highRatedApps.Select(r => new GooglAppDto() { Name = r.Name, Reviews = r.Reviews });
            foreach (var dto in dtos)
            {
                Console.WriteLine($"{dto.Name}: {dto.Reviews}");
            }

            var Genres = highRatedApps.SelectMany(g => g.Genres);  //Select dla listy utworzy kolekcje list - trzeba wybrać SelectMany
            Console.WriteLine(string.Join("; ", Genres));
        }

        static void GetData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps
                .Where(r => r.Rating > 4.6 && r.Category == Category.BEAUTY && r.Reviews < 30).LastOrDefault();
            Display(highRatedApps);
        }

        static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }

        }
        static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }

        static List<GoogleApp> LoadGoogleAps(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GoogleAppMap>();
                var records = csv.GetRecords<GoogleApp>().ToList();
                return records;
            }

        }

    }


}

