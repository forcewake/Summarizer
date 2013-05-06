using System.IO;
using System.Linq;
using System.Security.Permissions;

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