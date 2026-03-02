using System.Linq.Expressions;
using System.Runtime;
using System.Xml.Linq;

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
                    string otherAnswer = null;

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

        // Структура Research для управления опросом
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

            // Метод добавления нового ответа
            public void Add(string[] answers)
            {
                if (answers.Length != 3) return;

                Response newResponse = new Response(answers[0], answers[1], answers[2]);

                Array.Resize(ref _responses, _responses.Length + 1);

                _responses[_responses.Length - 1] = newResponse;
            }

            public string[] GetTopResponses(int question)
            {
                // Собираем все ответы на указанный вопрос
                var answers = _responses
                    // красивый короткий свич switch expression)
                    .Select(r => question switch
                    {
                        1 => r.Animal,
                        2 => r.CharacterTrait,
                        3 => r.Concept,
                        _ => null
                    })
                    .Where(a => !string.IsNullOrEmpty(a))
                    .GroupBy(a => a)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => g.Key)
                    .ToArray();

                return answers;
            }

            public void Print()
            {
                Console.WriteLine($"Название опроса: {_name}");
                Console.WriteLine($"Количество ответов: {_responses.Length}");
                Console.WriteLine("Топ-5 ответов по вопросам:");

                for (int i = 1; i <= 3; i++)
                {
                    string questionName = i switch
                    {
                        1 => "Животное",
                        2 => "Черта характера",
                        3 => "Понятие",
                        _ => ""
                    };

                    Console.WriteLine($"\n{questionName}:");

                    var topAnswers = GetTopResponses(i);

                    foreach (var answer in topAnswers)
                        Console.WriteLine($"  - {answer}");
                }
            }
        }
    }
}
