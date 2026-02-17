namespace SecondSemestr
{
    class Program
    {

        // Задание от 17.02.2026 лабораторная на персонажей

        public static void Main(string[] args)
        {
            
        }

        public struct Character
        {
            public enum Outfit
            {
                OutfitOne = 1,
                OutfitTwo = 2,
                OutfitThree = 3,
                OutfitFour = 4
            }

            public enum Position
            {
                agressiveLeft = 1,
                agressiveRight = 2,
                defaultLeft = 3,
                defaultRight = 4,
                handLeft = 5,
                handRight = 6,
                crossLeft = 7,
                crossRight = 8
            }

            public enum Color
            {
                black = 0,
                white = 1
            }

            private string _name;
            private string _surname;
            private string _gender;
            private int _age;
            private Position _pose;
            private Color _skin;
            private Outfit _appearance;
            private string[] _clothes;

            public string Name => _name;
            public string Surname => _surname;
            public string Gender => _gender;
            public int Age => _age;
            public Position Pose => _pose;
            public Outfit Appearance => _appearance;
            public Color Skin => _skin;
            public string[] Clothes => _clothes.ToArray();

            public Character(string name, string surname, string gender, int age, Position pose, Outfit appearance, Color skin)
            {
                _name = name;
                _surname = surname;
                _gender = gender;
                _age = age;

                _pose = Position.defaultLeft;
                _appearance = Outfit.OutfitOne;
                _skin = Color.white;

                _clothes = new string[5] { "shirt", "pants", "shoes", "hat", "jacket" };
            }
        }

        public struct Village
        {
            private string _name;
            private Character[] _characters;

            public string Name => _name;
            public Character[] Characters => _characters.ToArray();

            public Village(string name) 
            {
                _name = name;
                _characters = new Character[8]
                {
                    new Character("Elena", "Voloshina", "Female", 28,
                Character.Position.handLeft, Character.Outfit.OutfitOne, Character.Color.white),

                    new Character("Dmitry", "Kozlov", "Male", 35,
                Character.Position.agressiveRight, Character.Outfit.OutfitTwo, Character.Color.black),

                    new Character("Anna", "Sokolova", "Female", 22,
                Character.Position.defaultLeft, Character.Outfit.OutfitThree, Character.Color.white),

                    new Character("Ivan", "Petrov", "Male", 45,
                Character.Position.crossRight, Character.Outfit.OutfitFour, Character.Color.black),

                    new Character("Maria", "Ivanova", "Female", 31,
                Character.Position.handRight, Character.Outfit.OutfitOne, Character.Color.white),

                    new Character("Sergey", "Smirnov", "Male", 52,
                Character.Position.agressiveLeft, Character.Outfit.OutfitTwo, Character.Color.black),

                    new Character("Olga", "Kuznetsova", "Female", 19,
                Character.Position.defaultRight, Character.Outfit.OutfitThree, Character.Color.white),

                    new Character("Alexey", "Morozov", "Male", 40,
                Character.Position.crossLeft, Character.Outfit.OutfitFour, Character.Color.black)
                };
            }

        }



    }
}