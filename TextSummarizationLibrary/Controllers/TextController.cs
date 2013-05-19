using System;
using System.Linq;
using System.Text;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary.Controllers
{
    public class TextController
    {
        private readonly Dictionary _rulesDictionary;
        private readonly TextSource _textSource;

        public TextController(Dictionary rulesDictionary)
        {
            _rulesDictionary = rulesDictionary;
            _textSource = new TextSource();
        }

        public TextSource ParseText(TextModel textModel)
        {
            string[] words = textModel.Text.Split(' ', '\r'); //space and line feed characters are the ends of words.
            var cursentence = new Sentence();
            _textSource.Sentences.Add(cursentence);
            var originalSentence = new StringBuilder();
            foreach (string word in words)
            {
                string locWord = word;
                if (locWord.StartsWith("\n") && word.Length > 2)
                {
                    locWord = locWord.Replace("\n", "");
                }
                originalSentence.AppendFormat("{0} ", locWord);
                cursentence.Words.Add(new Word(locWord));
                AddWordCount(locWord);
                if (!IsSentenceBreak(locWord))
                {
                    continue;
                }
                cursentence.OriginalSentence = originalSentence.ToString();
                cursentence = new Sentence();
                originalSentence = new StringBuilder();
                _textSource.Sentences.Add(cursentence);
            }

            return _textSource;
        }

        private void AddWordCount(string word)
        {
            var stemmerController = new StemmerController(_rulesDictionary);
            Word stemmedWord = stemmerController.StemWord(word);
            if (string.IsNullOrEmpty(word) || word == " " || word == "\n" || word == "\t") return;
            Word foundWord = _textSource.WordCounts.Find(match => match.Stem == stemmedWord.Stem);
            if (foundWord == null)
            {
                _textSource.WordCounts.Add(stemmedWord);
            }
            else
            {
                foundWord.TermFrequency++;
            }
        }

        private bool IsSentenceBreak(string word)
        {
            if (word.Contains("\r") || word.Contains("\n"))
            {
                return true;
            }

            bool shouldBreak =
                _rulesDictionary.LinebreakRules.Any(p => word.EndsWith(p, StringComparison.CurrentCultureIgnoreCase));

            if (shouldBreak == false)
            {
                return false;
            }

            shouldBreak =
                !_rulesDictionary.NotALinebreakRules.Any(
                    p => word.StartsWith(p, StringComparison.CurrentCultureIgnoreCase));
            return shouldBreak;
        }
    }
}