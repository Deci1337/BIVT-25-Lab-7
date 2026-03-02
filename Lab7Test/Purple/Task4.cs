using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab7Test.Purple
{
   [TestClass]
   public sealed class Task4
   {
       record InputRow(string Name, string Surname, double Time);
       record OutputRow(string Name, string Surname, double Time);

       private InputRow[][] _inputGroups;a
       private OutputRow[] _output;
       private Lab7.Purple.Task4.Sportsman[][] _sportsmanGroups;
       private Lab7.Purple.Task4.Group[] _group;

       [TestInitialize]
       public void LoadData()
       {
           var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
           folder = Path.Combine(folder, "Lab7Test", "Purple");

           var inputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "input.json"))
           )!;
           var outputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "output.json"))
           )!;

           var task4Input = inputJson.GetProperty("Task4");
           var task4Output = outputJson.GetProperty("Task4");

           int groupCount = task4Input.EnumerateObject().Count();
           _inputGroups = new InputRow[groupCount][];

           int g = 0;
           foreach (var groupProp in task4Input.EnumerateObject())
           {
               _inputGroups[g] = groupProp.Value.Deserialize<InputRow[]>()!;
               g++;
           }

           _output = task4Output.GetProperty("Финалисты").Deserialize<OutputRow[]>()!;

           _sportsmanGroups = new Lab7.Purple.Task4.Sportsman[_inputGroups.Length][];
           _group = new Lab7.Purple.Task4.Group[_inputGroups.Length];
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab7.Purple.Task4.Sportsman);

           Assert.IsTrue(type.IsValueType, "Sportsman должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Time")?.CanRead ?? false, "Нет свойства Time");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Time")?.CanWrite ?? false, "Свойство Time должно быть только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Sportsman(string name, string surname)");
			Assert.IsNotNull(type.GetMethod("Run", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double) }, null), "Нет публичного метода Run(double time)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 3);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 9);

			type = typeof(Lab7.Purple.Task4.Group);
           Assert.IsTrue(type.IsValueType, "Group должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Sportsmen")?.CanRead ?? false, "Нет свойства Sportsmen");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Sportsmen")?.CanWrite ?? false, "Свойство Sportsmen должно быть только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null), "Нет публичного конструктора Group(string name)");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task4.Group) }, null), "Нет публичного конструктора Group(Group group)");
			Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task4.Sportsman) }, null), "Нет публичного метода Add(Sportsman elem)");
			Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task4.Sportsman[]) }, null), "Нет публичного метода Add(Sportsman[] array)");
			Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task4.Group) }, null), "Нет публичного метода Add(Group group)");
			Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Sort()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 2);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 2);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 12);
		}

       [TestMethod]
       public void Test_01_InitSportsmen()
       {
           InitSportsmen();
           CheckSportsmen();
       }

       [TestMethod]
       public void Test_02_Run()
       {
           InitSportsmen();
           RunSportsmen();
           CheckSportsmen(true);
       }

       [TestMethod]
       public void Test_03_InitGroups()
       {
           InitGroups();
           CheckGroups();
       }

       [TestMethod]
       public void Test_04_AddOne()
       {
           InitGroups();
           AddOne();
           CheckGroups();
       }

       [TestMethod]
       public void Test_05_AddSeveral()
       {
           InitGroups();
           AddSeveral();
           CheckGroups();
       }

       [TestMethod]
       public void Test_06_SortGroups()
       {
           InitGroups();
           AddOne();
           AddSeveral();
           SortGroups();
           CheckGroups(sorted:true);
       }

       [TestMethod]
       public void Test_07_MergeGroups()
       {
           InitGroups();
           AddOne();
           AddSeveral();
           SortGroups();

           var merged = Lab7.Purple.Task4.Group.Merge(_group[0], _group[1]);

           var allSportsmen = merged.Sportsmen;
           Assert.AreEqual(_output.Length, allSportsmen.Length);

           for (int i = 0; i < _output.Length; i++)
           {
               Assert.AreEqual(_output[i].Name, allSportsmen[i].Name);
               Assert.AreEqual(_output[i].Surname, allSportsmen[i].Surname);
               Assert.AreEqual(_output[i].Time, allSportsmen[i].Time, 0.0001);
           }
       }

       [TestMethod]
       public void Test_08_AddGroup()
       {
           InitGroups();
           var groupToAdd = new Lab7.Purple.Task4.Group("Extra");
           foreach (var s in _sportsmanGroups[1])
               groupToAdd.Add(s);

           foreach (var g in _group)
               g.Add(groupToAdd);

           CheckGroups();
       }

       [TestMethod]
       public void Test_09_ArrayLinq()
       {
           InitGroups();
           AddOne();
           AddSeveral();
           ArrayLinq();
           CheckGroups(shifted:true);
       }

       private void InitSportsmen()
       {
           for (int g = 0; g < _inputGroups.Length; g++)
           {
               _sportsmanGroups[g] = new Lab7.Purple.Task4.Sportsman[_inputGroups[g].Length];
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i] = new Lab7.Purple.Task4.Sportsman(
                       _inputGroups[g][i].Name,
                       _inputGroups[g][i].Surname
                   );
               }
           }
       }

       private void RunSportsmen()
       {
           for (int g = 0; g < _inputGroups.Length; g++)
           {
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i].Run(_inputGroups[g][i].Time);
               }
           }
       }

       private void InitGroups()
       {
           InitSportsmen();
           RunSportsmen();
           for (int g = 0; g < _sportsmanGroups.Length; g++)
           {
               _group[g] = new Lab7.Purple.Task4.Group($"Group {g + 1}");
               foreach (var s in _sportsmanGroups[g])
                   _group[g].Add(s);
           }
       }

       private void AddOne()
       {
           foreach (var grp in _group)
           {
               foreach (var s in _sportsmanGroups[0])
                   grp.Add(s);
           }
       }

       private void AddSeveral()
       {
           foreach (var grp in _group)
               grp.Add(_sportsmanGroups[1]);
       }

       private void SortGroups()
       {
           foreach (var grp in _group)
               grp.Sort();
       }

       private void ArrayLinq()
       {
           foreach (var grp in _group)
           {
               var sportsmen = grp.Sportsmen;
               for (int i = 0; i < sportsmen.Length / 2; i++)
                   sportsmen[i] = sportsmen[i + 1];
           }
       }

       private void CheckSportsmen(bool hasRun = false)
       {
           for (int g = 0; g < _sportsmanGroups.Length; g++)
           {
               for (int i = 0; i < _sportsmanGroups[g].Length; i++)
               {
                   Assert.AreEqual(_inputGroups[g][i].Name, _sportsmanGroups[g][i].Name);
                   Assert.AreEqual(_inputGroups[g][i].Surname, _sportsmanGroups[g][i].Surname);
                   if (hasRun)
                   {
                       Assert.AreEqual(_inputGroups[g][i].Time, _sportsmanGroups[g][i].Time, 0.0001);
                   }
                   else
                   {
                       Assert.AreEqual(0, _sportsmanGroups[g][i].Time, 0.0001);
                   }
               }
           }
       }

       private void CheckGroups(bool sorted = false, bool shifted = false)
       {
           for (int g = 0; g < _group.Length; g++)
           {
               var sportsmen = _group[g].Sportsmen;
               var input = _sportsmanGroups[g];
               if (input == null)
               {
                   Assert.IsNull(sportsmen);
               }
               else
               {
                   Assert.AreEqual(input.Length, sportsmen.Length);
                   var compareInput = input;
                   if (sorted)
                       compareInput = input.OrderBy(s => s.Time).ToArray();
                   if (shifted)
                       compareInput = input.Skip(1).Take(input.Length/2).Concat(input.Skip(input.Length / 2).Take(input.Length - input.Length / 2)).ToArray();
                   for (int i = 0; i < input.Length; i++)
                   {
                       Assert.AreEqual(compareInput[i].Name, sportsmen[i].Name);
                       Assert.AreEqual(compareInput[i].Surname, sportsmen[i].Surname);
                       Assert.AreEqual(compareInput[i].Time, sportsmen[i].Time, 0.0001);
                   }
               }
           }
       }
   }
}

