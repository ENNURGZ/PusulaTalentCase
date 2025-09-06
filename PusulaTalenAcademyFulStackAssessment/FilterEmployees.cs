using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaTalentCase
{
    public class FilterEmployeesHelper
    {
        /// <summary>
        /// çalışan listesini filtreliyorum ve özet istatistikleri JSON döndürüyorum.
        /// Koşullar:
        /// - Yaş: 25..40 (dahil)
        /// - Departman: IT veya Finance
        /// - Maaş: 5000..9000 (dahil)
        /// - İşe giriş: 2017'den sonra
        /// Sonra isimleri uzunluğa göre azalan, sonra alfabetik sıralıyorum.
        /// </summary>
        public static string FilterEmployees(
            IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
        {
            // Boş gelirse ben boş bir sonuç döndürüyorum.
            static string Empty() => JsonSerializer.Serialize(new
            {
                Names = new List<string>(),
                TotalSalary = 0m,
                AverageSalary = 0m,
                MinSalary = 0m,
                MaxSalary = 0m,
                Count = 0
            });

            if (employees == null) return Empty();

            var after2017 = new DateTime(2017, 12, 31);

            // Koşullara uyanları ayırıyorum.
            var filtered = employees
                .Where(e =>
                    e.Age >= 25 && e.Age <= 40 &&
                    (e.Department == "IT" || e.Department == "Finance") &&
                    e.Salary >= 5000m && e.Salary <= 9000m &&
                    e.HireDate > after2017)
                // İsimleri: uzunluğa göre azalan, sonra alfabetik
                .OrderByDescending(e => e.Name.Length)
                .ThenBy(e => e.Name)
                .ToList();

            if (filtered.Count == 0) return Empty();

            // İsimleri ve maaş hazırlıyorum.
            var names = filtered.Select(e => e.Name).ToList();
            var total = filtered.Sum(e => e.Salary);
            var avg = filtered.Average(e => e.Salary);
            var min = filtered.Min(e => e.Salary);
            var max = filtered.Max(e => e.Salary);

            // JSON olarak geri döndürüyorum.
            return JsonSerializer.Serialize(new
            {
                Names = names,
                TotalSalary = total,
                AverageSalary = Math.Round(avg, 2),
                MinSalary = min,
                MaxSalary = max,
                Count = filtered.Count
            });
        }
    }
}
