using System;

namespace TextSummarizationLibrary
{
    internal class Word
    {
        public Word()
        {
        }

        public Word(string word)
        {
            Value = word;
        }

        public string Value { get; set; }
        public string Stem { get; set; }
        public double TermFrequency { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var arg = (Word) obj;
            return Value.Equals(arg.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}