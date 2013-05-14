using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary
{
    public class Summarizer
    {
        public SummarizedDocument Summarize(TextModel text)
        {
            return new Article().Parse(text)
                                .Grade()
                                .CreateImportantWordsList()
                                .GradeSentences()
                                .ExtractArticleConcepts()
                                .ApplySentenceFactors()
                                .Highlight()
                                .SelectNumberOfSentences(text.DisplayLines)
                                .CreateSummarizedDocument();
        }
    }
}