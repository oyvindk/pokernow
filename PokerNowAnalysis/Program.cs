using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PokerNowAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            // var spades = "═";
            // var hearts = "╚";
            // var diamonds = "╝";
            // var clubs = "║";
            var lines = System.IO.File.ReadAllLines("log.csv");
            var allText = System.IO.File.ReadAllText("log.csv");
            var results = new Dictionary<string, List<decimal>>();
            var allNamesRegex = new Regex(@"(?:"""""")(.*?)""""");
            var allNames = allNamesRegex.Matches(allText);
            var distinctNames = allNames.OfType<Match>().Select(m => m.Groups[1].Value).Distinct().ToArray();
            foreach (var mat in distinctNames)
            {
                if (!results.ContainsKey(mat))
                {
                    results[mat] = new List<decimal>();
                }
            }

            foreach (var line in lines.Reverse())
            {
                var hand = new Dictionary<string, decimal>();
                var regex = new Regex(@"(?:#[0-9]*?) """"(.*?)"""" \(([0-9]*?)\)");
                if (!regex.IsMatch(line)) continue;
                var matches = regex.Matches(line);
                foreach (Match m in matches)
                {
                    var name = m.Groups[1].Value;
                    var chips = decimal.Parse(m.Groups[2].Value);
                    if (!hand.ContainsKey(name))
                    {
                        hand[name] = chips / 30000;
                    }
                }

                foreach (var name in distinctNames)
                {
                    if (hand.ContainsKey(name))
                    {
                        results[name].Add(hand[name]);
                    }
                    else
                    {
                        results[name].Add(0);
                    }
                }
            }
            
           
            var sb = new StringBuilder();
            foreach (var res in results)
            {
                sb.Append($"{res.Key};");
                foreach (var val in res.Value)
                {
                    sb.Append(val + ";");    
                }

                sb.AppendLine();
            }
            System.IO.File.WriteAllText("testtest.csv", sb.ToString());
        }
    }
}
