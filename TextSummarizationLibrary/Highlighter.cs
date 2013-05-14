using System.Linq;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary
{
    internal class Highlighter
    {
        public Highlighter(Article article)
        {
            Article = article;
        }

        private Article Article { get; set; }

        internal Article SelectNumberOfSentences(int lineCount)
        {
            int loopCounter = 0;
            foreach (Sentence sentence in Article.Sentences
                                                 .OrderByDescending(p => p.Score)
                                                 .Select(p => p)
                                                 .Where(sentence => sentence.OriginalSentence != null))
            {
                sentence.Selected = true;
                loopCounter++;
                if (loopCounter >= lineCount)
                {
                    break;
                }
            }
            return Article;
        }
    }
}