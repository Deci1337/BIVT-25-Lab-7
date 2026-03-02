namespace Lab7.Purple
{
    public class Task2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;
            private int _result;

            public string Name { get { return _name; } }
            public string Surname { get { return _surname; } }
            public int Distance { get { return _distance; } }
            public int[] Marks
            {
                get
                {
                    var newarr = new int[_marks.Length];
                    Array.Copy(_marks, newarr, _marks.Length);
                    return newarr;
                }
            }

            public int Result => _result;




            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;

                _marks = new int[5];

            }

            public void Jump(int distance, int[] marks)
            {
                _distance = distance;
                _marks = marks;

                int defaultPoints = 60;
                int defaultDist = 120;

                int points;
                int sum = 0;

                if (defaultDist - _distance >= (defaultPoints/2))
                    points = 0;
                else
                {
                    points = defaultPoints + 2 * (_distance - defaultDist);
                }

                Array.Sort(marks);

                for (int i = 1; i < marks.Length - 1; i++)
                    sum+= marks[i];

                sum += points;

                _result = sum;
            }

            public static void Sort(Participant[] array)
            {
                Participant[] sorted = array.OrderByDescending(p => p.Result).ToArray();

                // Явно переприсваиваем каждый элемент (важно для структур!)
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = sorted[i];
                }
            }

            public void Print()
            {
                Console.WriteLine($"{Surname} {Name}: {Result}");
            }

        }



    }
}
