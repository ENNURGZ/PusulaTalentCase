using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace PusulaTalentCase
{
    public class FilterPeople
    {
        /// <summary>
        /// XML içindeki kişileri filtreliyorum:
        /// - Yaş 30'dan büyük
        /// - Departman IT
        /// - Maaş 5000’den fazla
        /// - İşe giriş tarihi 2019’dan önce
        /// Sonuçta JSON olarak isimler, maaş bilgileri ve kişi sayısını döndürüyorum.
        /// </summary>
        public static string FilterPeopleFromXml(string xmlData)
        {
            // Eğer bana boş XML gelirse direkt boş sonuç döndürüyorum
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                return JsonSerializer.Serialize(new
                {
                    Names = new List<string>(),
                    TotalSalary = 0,
                    AverageSalary = 0,
                    MaxSalary = 0,
                    Count = 0
                });
            }

            // Önce XML verisini parse ediyorum
            var doc = XDocument.Parse(xmlData);

            // Her Person içinden bilgileri alıyorum
            var people = doc.Descendants("Person")
                .Select(p => new
                {
                    Name = p.Element("Name")?.Value ?? "",
                    Age = int.Parse(p.Element("Age")?.Value ?? "0"),
                    Department = p.Element("Department")?.Value ?? "",
                    Salary = decimal.Parse(p.Element("Salary")?.Value ?? "0"),
                    HireDate = DateTime.Parse(p.Element("HireDate")?.Value ?? DateTime.MinValue.ToString())
                })
                // filtreleri burada uyguluyorum
                .Where(p => p.Age > 30
                            && p.Department == "IT"
                            && p.Salary > 5000
                            && p.HireDate < new DateTime(2019, 1, 1))
                .OrderBy(p => p.Name) // İsimleri alfabetik sıralıyorum
                .ToList();

            // Maaş hesaplamalarını yapıyorum
            var totalSalary = people.Sum(p => p.Salary);
            var averageSalary = people.Count > 0 ? people.Average(p => p.Salary) : 0;
            var maxSalary = people.Count > 0 ? people.Max(p => p.Salary) : 0;
            var count = people.Count;

            // JSON çıktısını hazırlıyorum
            var result = new
            {
                Names = people.Select(p => p.Name).ToList(),
                TotalSalary = totalSalary,
                AverageSalary = Math.Round(averageSalary, 2),
                MaxSalary = maxSalary,
                Count = count
            };

            // sonucu JSON string olarak döndürüyorum
            return JsonSerializer.Serialize(result);
        }
    }
}
