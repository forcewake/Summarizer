using TextSummarizationLibrary.Models;

namespace TextSummarizationLibrary.Controllers
{
    public class SummarizerController
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