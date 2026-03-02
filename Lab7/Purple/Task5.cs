using System.Runtime;
using System.Xml.Linq;

using System;
namespace Lab7.Purple
{
    public class Task5
    {
        public struct Response
        {
            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _characterTrait;
            public string Concept => _concept;

            public Response(string animal, string characterTrait, string concept)
            {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            // Метод подсчета совпадающих ответов на выбранный вопрос
            public int CountVotes(Response[] responses, int questionNumber)
            {
                int count = 0;
                string currentAnswer = null;

                if (questionNumber == 1)
                    currentAnswer = _animal;

                else if (questionNumber == 2)
                    currentAnswer = _characterTrait;

                else if (questionNumber == 3)
                    currentAnswer = _concept;

                if (currentAnswer == null)
                    return 0;

                foreach (var response in responses)
                {
                    string otherAnswer = "";

                    if (questionNumber == 1)
                        otherAnswer = response._animal;

                    else if (questionNumber == 2)
                        otherAnswer = response._characterTrait;

                    else if (questionNumber == 3)
                        otherAnswer = response._concept;

                    if (currentAnswer == otherAnswer && !string.IsNullOrEmpty(currentAnswer))
                        count++;
                }

                return count;
            }

            public void Print()
            {
                Console.WriteLine($"Животное: {_animal}, Черта характера: {_characterTrait}, Понятие: {_concept}");
            }
        }

        public struct Research
        {
            private string _name;

            private Response[] _responses;
            public string Name => _name;
            public Response[] Responses => _responses;

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] responses)
            {
                Response newResponse = new Response(responses[0], responses[1], responses[2]);
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = newResponse;
            }
            public string[] GetTopResponses(int question)
            {
                var allAnswers = _responses
                    .Select(r =>
                    question == 1 ? r.Animal :
                    question == 2 ? r.CharacterTrait :
                    question == 3 ? r.Concept : 
                    null ) // ни одно не подошло
                    .Where(a => !string.IsNullOrEmpty(a));

                return allAnswers
                    .GroupBy(answer => answer)
                    .OrderByDescending(group => group.Count())
                    .Take(5)
                    .Select(group => group.Key!) // ! null-forgiving operator фикс ошибки с нулом 
                    .ToArray();
            }

            public void Print()
            {
                Console.WriteLine(Name);
                Console.WriteLine(String.Join(", ", Responses));
            }
        }
    }
}
