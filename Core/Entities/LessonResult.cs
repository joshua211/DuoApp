using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class LessonResult
    {
        public string Id { get; set; }
        public List<LearnedWord> LearnedWords;
        public int CorrectWords { get; set; }
        public int TotalWords { get; set; }
        public Skill? Skill { get; set; }

        public LessonResult()
        {
            Id = Guid.NewGuid().ToString();
            LearnedWords = new List<LearnedWord>();
        }
    }

    public class LearnedWord
    {
        public Word Word { get; set; }
        public bool Learned { get; set; }
        public bool UsedHint { get; set; }

        public LearnedWord(Word word, bool learned, bool usedHint)
        {
            Word = word;
            Learned = learned;
            UsedHint = usedHint;
        }
    }
}