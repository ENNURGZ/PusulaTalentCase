using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaTalentCase
{
    public class MaxIncreasingSubArray
    {
        /// <summary>
        /// Listedeki artan alt dizilerden toplamı en büyük olanı bulup JSON döndürür.
        /// </summary>
        public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
        {
            // Liste boşsa ben direkt [] döndürüyorum.
            if (numbers == null || numbers.Count == 0)
                return JsonSerializer.Serialize(new List<int>());

            // current: şu an takip ettiğim artan dizi
            // best   : şimdiye kadar bulduğum en iyi dizi
            var current = new List<int>();
            var best = new List<int>();

            // currentSum: current dizisinin toplamı
            // bestSum   : best dizisinin toplamı
            int currentSum = 0;
            int bestSum = 0;

            // Tüm sayıları sırayla geziyorum.
            for (int i = 0; i < numbers.Count; i++)
            {
                // Eğer ilk elemanı ekleyeceksem ya da artış devam ediyorsa devam ederim.
                if (current.Count == 0 || numbers[i] > current.Last())
                {
                    current.Add(numbers[i]);
                    currentSum += numbers[i];
                }
                else
                {
                    // Artış bitti. mevcut diziyi en iyiyle karşılaştırıyorum.
                    if (currentSum > bestSum)
                    {
                        best = new List<int>(current);
                        bestSum = currentSum;
                    }

                    // Yeni bir diziye başlıyorum.
                    current.Clear();
                    current.Add(numbers[i]);
                    currentSum = numbers[i];
                }
            }

            // Döngü bitince son current dizisini de kontrol ediyorum.
            if (currentSum > bestSum)
                best = current;

            // Sonucu JSON olarak döndürüyorum.
            return JsonSerializer.Serialize(best);
        }
    }
}
