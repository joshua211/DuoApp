using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Entities;

namespace Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new DuolingoClient(new AuthenticationEntity()
            {
                DistinctId = "782244993",
                Timezone = "Europe/Berlin",
                FromLanguage = "en",
                LearningLanguage = "pt",
                LandingUrl = "https://www.duolingo.com/",
                InitialReferrer = "https://www.duolingo.com/learn",
                LastReferrer = "https://www.google.com/"
            }, new FilePersistence());

            await client.Authenticate("JoshuaHillm");
            var skills = (await client.GetSkillsAsync()).Where(s => s.Lexemes.Any());

            var skill = skills.First();
            foreach (var lexeme in skill.Lexemes)
            {
                var word = await client.GetWordAsync(lexeme);
                System.Console.WriteLine(word.Value + ": " + word.Translation);
            }
        }
    }
}
