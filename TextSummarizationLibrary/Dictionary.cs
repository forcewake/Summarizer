using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TextSummarizationLibrary.Models;
using TextSummarizationLibrary.Properties;

namespace TextSummarizationLibrary
{
    internal class Dictionary
    {
        private Dictionary()
        {
        }

        public List<Word> UnimportantWords { get; set; }
        public List<string> LinebreakRules { get; set; }
        public List<string> NotALinebreakRules { get; set; }
        public List<string> DepreciateValueRule { get; set; }
        public List<string> TermFreqMultiplierRule { get; set; }

        //the replacement rules are stored as KeyValuePair<string,string>s 
        //the Key is the search term. the Value is the replacement term
        public Dictionary<string, string> Step1PrefixRules { get; set; }
        public Dictionary<string, string> Step1SuffixRules { get; set; }
        public Dictionary<string, string> ManualReplacementRules { get; set; }
        public Dictionary<string, string> PrefixRules { get; set; }
        public Dictionary<string, string> SuffixRules { get; set; }
        public Dictionary<string, string> SynonymRules { get; set; }
        public string Language { get; set; }

        public static Dictionary LoadFromFile(string dictionaryLanguage)
        {
            var dict = new Dictionary();
            XElement doc;
            switch (dictionaryLanguage)
            {
                case "ru":
                    {
                        doc = XElement.Parse(Resources.ru);
                        break;
                    }
                case "en":
                    {
                        doc = XElement.Parse(Resources.en);
                        break;
                    }
                default:
                    throw new ArgumentException("Dictionary Language is bad " + dictionaryLanguage);
            }

            dict.Step1PrefixRules = LoadKeyValueRule(doc, "stemmer", "step1_pre");
            dict.Step1SuffixRules = LoadKeyValueRule(doc, "stemmer", "step1_post");
            dict.ManualReplacementRules = LoadKeyValueRule(doc, "stemmer", "manual");
            dict.PrefixRules = LoadKeyValueRule(doc, "stemmer", "pre");
            dict.SuffixRules = LoadKeyValueRule(doc, "stemmer", "post");
            dict.SynonymRules = LoadKeyValueRule(doc, "stemmer", "synonyms");
            dict.LinebreakRules = LoadValueOnlyRule(doc, "parser", "linebreak");
            dict.NotALinebreakRules = LoadValueOnlyRule(doc, "parser", "linedontbreak");
            dict.DepreciateValueRule = LoadValueOnlyRule(doc, "grader-syn", "depreciate");
            dict.TermFreqMultiplierRule = LoadValueOnlySection(doc, "grader-tf");

            dict.UnimportantWords = new List<Word>();
            List<string> unimpwords = LoadValueOnlySection(doc, "grader-tc");
            foreach (string unimpword in unimpwords)
            {
                dict.UnimportantWords.Add(new Word(unimpword));
            }
            return dict;
        }

        private static List<string> LoadValueOnlySection(XElement doc, string section)
        {
            IEnumerable<XElement> step1Pre = doc.Elements(section);
            List<string> loadValueOnlySection = step1Pre
                .Elements()
                .Select(x => x.Value)
                .ToList();
            return loadValueOnlySection;
        }

        private static List<string> LoadValueOnlyRule(XElement doc, string section, string container)
        {
            IEnumerable<XElement> step1Pre = doc
                .Elements(section)
                .Elements(container);
            List<string> loadValueOnlyRule = step1Pre
                .Elements()
                .Select(x => x.Value)
                .ToList();
            return loadValueOnlyRule;
        }

        private static Dictionary<string, string> LoadKeyValueRule(XElement doc, string section, string container)
        {
            var dictionary = new Dictionary<string, string>();
            IEnumerable<XElement> step1Pre = doc
                .Elements(section)
                .Elements(container);
            foreach (var keyvalue in step1Pre.Elements()
                                             .Select(x => x.Value)
                                             .Select(rule => rule.Split('|'))
                                             .Where(keyvalue => !dictionary.ContainsKey(keyvalue[0])))
            {
                dictionary.Add(keyvalue[0], keyvalue[1]);
            }
            return dictionary;
        }
    }
}