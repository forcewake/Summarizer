using System.Collections.Generic;
using System.Linq;

namespace TextSummarizationLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceWord(this string word, Dictionary<string, string> replacementRule)
        {
            foreach (var rule in replacementRule.Where(rule => word == rule.Key))
            {
                return rule.Value;
            }

            return word;
        }

        public static string StemFormat(this string word, Dictionary rules)
        {
            word = StripPrefix(word, rules.Step1PrefixRules);
            word = StripSuffix(word, rules.Step1SuffixRules);
            return word;
        }


        public static string StripSuffix(this string word, Dictionary<string, string> suffixRule)
        {
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (var rule in suffixRule)
            {
                if (word != null && word.EndsWith(rule.Key))
                {
                    word = word.Substring(0, word.Length - rule.Key.Length) + rule.Value;
                }
            }
            return word;
        }

        public static string StripPrefix(this string word, Dictionary<string, string> prefixRule)
        {
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (var rule in prefixRule)
            {
                if (word != null && word.StartsWith(rule.Key))
                {
                    word = rule.Value + word.Substring(rule.Key.Length);
                }
            }
            return word;
        }
    }
}