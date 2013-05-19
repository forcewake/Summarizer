using System.Collections.Generic;

namespace TextSummarizationLibrary.Models
{
    public class TextSource
    {
        public TextSource()
        {
            Sentences = new List<Sentence>();
            WordCounts = new List<Word>();
        }

        public List<Sentence> Sentences { get; private set; }
        public List<Word> WordCounts { get; private set; }
    }
}