using TextSummarizationLibrary.Extensions;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary.Controllers
{
    public class StemmerController
    {
        private readonly Dictionary _rulesDictionary;

        public StemmerController(Dictionary rulesDictionary)
        {
            _rulesDictionary = rulesDictionary;
        }

        public string StemStrip(string word)
        {
            string originalWord = word;

            word = word
                .StemFormat(_rulesDictionary)
                .ReplaceWord(_rulesDictionary.ManualReplacementRules)
                .StripPrefix(_rulesDictionary.PrefixRules)
                .StripSuffix(_rulesDictionary.SuffixRules)
                .ReplaceWord(_rulesDictionary.SynonymRules);

            if (word.Length <= 2)
            {
                word = originalWord.StemFormat(_rulesDictionary);
            }

            return word;
        }

        public Word StemWord(string word)
        {
            word = word.ToLower();
            var newword = new Word
                {
                    TermFrequency = 1,
                    Value = word.StemFormat(_rulesDictionary),
                    Stem = StemStrip(word)
                };
            return newword;
        }
    }
}