using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class LearnResult
    {
        public string Id { get; set; }
        public Dictionary<Word, bool> LearnedWords { get; set; }
        public int CorrectWords { get; set; }
        public int TotalWords { get; set; }
        public Skill? Skill { get; set; }

        public LearnResult()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}