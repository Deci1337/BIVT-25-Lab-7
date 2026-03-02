namespace Lab7.Purple
{
    public class Task4
    {
        public struct Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;

            public string Name { get { return _name; } }
            public string Surname { get { return _surname; } }
            public double Time { get { return _time; } }

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
            }

            public void Run(double time)
            {
                _time = time;
            }

            public void Print()
            {
                Console.WriteLine($"{Surname} {Name}: {Time}");
            }
        }

        public struct Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

            public string Name { get { return _name; } }
            public Sportsman[] Sportsmen { get { return _sportsmen; } }

            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }

            public Group(Group group)
            {
                _name = group.Name;
                _sportsmen = group.Sportsmen;
            }

            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null)
                {
                    _sportsmen = new Sportsman[1];
                    _sportsmen[0] = sportsman;
                    return;
                }
                Sportsman[] newArray = new Sportsman[_sportsmen.Length + 1];
                for (int i = 0; i < _sportsmen.Length; i++)
                    newArray[i] = _sportsmen[i];
                newArray[_sportsmen.Length] = sportsman;
                _sportsmen = newArray;
            }

            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null || sportsmen.Length == 0) return;
                if (_sportsmen == null)
                {
                    _sportsmen = new Sportsman[sportsmen.Length];
                    for (int i = 0; i < sportsmen.Length; i++)
                        _sportsmen[i] = sportsmen[i];
                    return;
                }
                Sportsman[] newArray = new Sportsman[_sportsmen.Length + sportsmen.Length];
                for (int i = 0; i < _sportsmen.Length; i++)
                    newArray[i] = _sportsmen[i];
                for (int i = 0; i < sportsmen.Length; i++)
                    newArray[_sportsmen.Length + i] = sportsmen[i];
                _sportsmen = newArray;
            }

            public void Add(Group group)
            {
                if (group.Sportsmen != null && group.Sportsmen.Length > 0)
                    Add(group.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null || _sportsmen.Length == 0) return;
                for (int i = 0; i < _sportsmen.Length - 1; i++)
                {
                    for (int j = i + 1; j < _sportsmen.Length; j++)
                    {
                        if (_sportsmen[i].Time > _sportsmen[j].Time)
                        {
                            Sportsman t = _sportsmen[i];
                            _sportsmen[i] = _sportsmen[j];
                            _sportsmen[j] = t;
                        }
                    }
                }
            }

            public static Group Merge(Group group1, Group group2)
            {
                Sportsman[] arr1 = group1.Sportsmen ?? new Sportsman[0];
                Sportsman[] arr2 = group2.Sportsmen ?? new Sportsman[0];
                Sportsman[] merged = new Sportsman[arr1.Length + arr2.Length];
                int i = 0, j = 0, k = 0;
                while (i < arr1.Length && j < arr2.Length)
                {
                    if (arr1[i].Time <= arr2[j].Time)
                        merged[k++] = arr1[i++];
                    else
                        merged[k++] = arr2[j++];
                }
                while (i < arr1.Length)
                    merged[k++] = arr1[i++];
                while (j < arr2.Length)
                    merged[k++] = arr2[j++];
                Group newGroup = new Group("Финалисты");
                newGroup._sportsmen = merged;
                return newGroup;
            }

            public void Print()
            {
                Console.WriteLine(Name);
                if (_sportsmen != null)
                {
                    for (int i = 0; i < _sportsmen.Length; i++)
                        _sportsmen[i].Print();
                }
            }
        }
    }
}
