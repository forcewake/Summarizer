using System.Collections.Generic;
using System.Linq;
using TextSummarizationLibrary.Controllers;
using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary
{
    internal class Article
    {
        public Article()
        {
            Sentences = new List<Sentence>();
            WordCounts = new List<Word>();
            Concepts = new List<string>();
        }

        public List<Sentence> Sentences { get; private set; }
        public List<string> Concepts { get; set; }
        public Dictionary Rules { get; private set; }

        public List<Word> ImportantWords { get; set; }
        public List<Word> WordCounts { get; private set; }

        public GraderController Grade()
        {
            return new GraderController(this);
        }

        public HighlighterController Highlight()
        {
            return new HighlighterController(this);
        }

        public Article Parse(TextModel text)
        {
            Rules = Dictionary.LoadFromFile(text.Language);
            var textController = new TextController(Rules);
            TextSource source = textController.ParseText(text);
            Sentences = source.Sentences;
            WordCounts = source.WordCounts;
            return this;
        }

        public SummarizedDocument CreateSummarizedDocument()
        {
            var sumDoc = new SummarizedDocument
                {
                    Concepts = Concepts
                };
            foreach (Sentence sentence in Sentences.Where(sentence => sentence.Selected))
            {
                sumDoc.Sentences.Add(sentence.OriginalSentence);
            }
            return sumDoc;
        }
    }
}