using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaTalentCase
{
    public class LongestVowelSubsequence
    {
        /// <summary>
        /// Bher kelimenin içindeki en uzun ardışık sesli harfleri buluyorum.
        /// Sonra kelime, alt dizi ve uzunluğu JSON olarak döndürüyorum.
        /// </summary>
        public static string LongestVowelSubsequenceAsJson(List<string> words)
        {
            // Sesli harfleri belirliyorum (İngilizce)
            var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };

            // Sonuçları tutacağım liste
            var results = new List<object>();

            // Eğer liste boşsa ben direkt [] döndürüyorum
            if (words == null || words.Count == 0)
                return JsonSerializer.Serialize(results);

            // Her kelimeyi tek tek inceliyorum
            foreach (var word in words)
            {
                string longestSeq = "";    // şimdiye kadarki en uzun sesli alt dizi
                string currentSeq = "";  // şu an takip ettiğim sesli alt dizi

                // Kelimenin tüm harflerine bakıyorum
                foreach (char c in word.ToLower())
                {
                    if (vowels.Contains(c))
                    {
                        // Harf sesliyse mevcut alt diziye ekliyorum
                        currentSeq += c;

                        // Eğer bu dizim en uzunsa güncelliyorum
                        if (currentSeq.Length > longestSeq.Length)
                            longestSeq = currentSeq;
                    }
                    else
                    {
                        // Sessiz harfe gelince diziyi sıfırlıyorum
                        currentSeq = "";
                    }
                }

                // Bu kelime için sonucu listeme ekliyorum
                results.Add(new
                {
                    word = word,
                    sequence = longestSeq,
                    length = longestSeq.Length
                });
            }

            // Sonuç listesini JSON formatına çevirip döndürüyorum
            return JsonSerializer.Serialize(results);
        }
    }
}
