using System.Collections.Generic;
using System.Linq;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary
{
    internal class Grader
    {
        public Grader(Article article, int wordsCount = 5)
        {
            WordsCount = wordsCount;
            Article = article;
        }

        private Article Article { get; set; }
        public int WordsCount { get; private set; }

        internal Grader CreateImportantWordsList()
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

        internal Grader GradeSentences()
        {
            foreach (Sentence sentence in from sentence in Article.Sentences
                                          from importantWord in
                                              sentence.Words.Select(word => Stemmer.StemStrip(word.Value, Article.Rules))
                                                      .Select(
                                                          wordstem =>
                                                          Article.ImportantWords.Find(match => match.Stem == wordstem))
                                                      .Where(importantWord => importantWord != null)
                                          select sentence)
            {
                sentence.Score++;
            }

            return this;
        }

        internal Grader ExtractArticleConcepts()
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