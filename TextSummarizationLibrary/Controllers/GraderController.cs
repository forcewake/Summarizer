using System.Collections.Generic;
using System.Linq;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary.Controllers
{
    internal class GraderController
    {
        public GraderController(Article article, int wordsCount = 3)
        {
            WordsCount = wordsCount;
            Article = article;
        }

        private Article Article { get; set; }
        private int WordsCount { get; set; }

        internal GraderController CreateImportantWordsList()
        {
            var wordsArray = new Word[Article.WordCounts.Count()];
            Article.WordCounts.CopyTo(wordsArray);
            Article.ImportantWords = new List<Word>(wordsArray);

            foreach (Word foundWord in Article.Rules.UnimportantWords
                                              .Select(
                                                  word =>
                                                  Article.ImportantWords.Find(
                                                      match => match.Value.ToLower() == word.Value.ToLower()))
                                              .Where(foundWord => foundWord != null))
            {
                Article.ImportantWords.Remove(foundWord);
            }

            Article.ImportantWords.Sort(CompareWordsByFrequency);
            return this;
        }

        internal GraderController GradeSentences()
        {
            var stemmerController = new StemmerController(Article.Rules);
            foreach (Sentence sentence in
                Article.Sentences.SelectMany(sentence => sentence.Words
                                                                 .Select(word => stemmerController.StemStrip(word.Value))
                                                                 .Select(
                                                                     wordstem =>
                                                                     Article.ImportantWords.Find(
                                                                         match => match.Stem == wordstem))
                                                                 .Where(importantWord => importantWord != null),
                                             (sentence, importantWord) => sentence))
            {
                sentence.Score++;
            }

            return this;
        }

        internal GraderController ExtractArticleConcepts()
        {
            Article.Concepts = new List<string>();
            if (Article.ImportantWords.Count > WordsCount)
            {
                double baseFreq = Article.ImportantWords[WordsCount].TermFrequency;
                Article.Concepts = Article.ImportantWords
                                          .Where(p => p.TermFrequency >= baseFreq)
                                          .Select(p => p.Value)
                                          .ToList();
            }
            else
            {
                foreach (Word word in Article.ImportantWords)
                {
                    Article.Concepts.Add(word.Value);
                }
            }
            return this;
        }

        internal Article ApplySentenceFactors()
        {
            //grade the first sentence of the article higher.
            Article.Sentences[0].Score *= 1.5;

            //grade first sentence of new paragraphs (denoted by two \n in a row) higher
            foreach (Sentence sentence in Article.Sentences
                                                 .Where(sentence => sentence.Words.Count >= 2)
                                                 .Where(
                                                     sentence =>
                                                     sentence.Words[0].Value.Contains('\n') &&
                                                     sentence.Words[1].Value.Contains('\n')))
            {
                sentence.Score *= 1.3;
            }
            return Article;
        }

        private static int CompareWordsByFrequency(Word lhs, Word rhs)
        {
            return rhs.TermFrequency.CompareTo(lhs.TermFrequency);
        }
    }
}