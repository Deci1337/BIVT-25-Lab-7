using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab7Test.Purple
{
   [TestClass]
   public sealed class Task2
   {
       record InputRow(string Name, string Surname, int Distance, int[] Marks);
       record OutputRow(string Name, string Surname, int Result);

       private InputRow[] _input;
       private OutputRow[] _output;
       private Lab7.Purple.Task2.Participant[] _participant;

       [TestInitialize]
       public void LoadData()
       {
           var folder = Directory.GetParent(Directory.GetCurrentDirectory())
                                 .Parent.Parent.Parent.FullName;
           folder = Path.Combine(folder, "Lab7Test", "Purple");

           var inputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "input.json")))!;
           var outputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "output.json")))!;

           _input = inputJson.GetProperty("Task2").Deserialize<InputRow[]>()!;
           _output = outputJson.GetProperty("Task2").Deserialize<OutputRow[]>()!;

           _participant = new Lab7.Purple.Task2.Participant[_input.Length];
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab7.Purple.Task2.Participant);
           Assert.IsTrue(type.IsValueType, "Participant должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Distance")?.CanRead ?? false, "Нет свойства Distance");
           Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
           Assert.IsTrue(type.GetProperty("Result")?.CanRead ?? false, "Нет свойства Result");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Distance")?.CanWrite ?? false, "Свойство Distance должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Result")?.CanWrite ?? false, "Свойство Result должно быть только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
			Assert.IsNotNull(type.GetMethod("Jump", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(int[]) }, null), "Нет публичного метода Jump(int distance, int[] marks)");
			Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task2.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 5);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 12);
		}

       [TestMethod]
       public void Test_01_Create()
       {
           Init();
           CheckStruct(jumpExpected: false);
       }

       [TestMethod]
       public void Test_02_Init()
       {
           Init();
           CheckStruct(jumpExpected: false);
       }

       [TestMethod]
       public void Test_03_Jump()
       {
           Init();
           Jump();
           CheckStruct(jumpExpected: true);
       }

       [TestMethod]
       public void Test_04_Sort()
       {
           Init();
           Jump();

           Lab7.Purple.Task2.Participant.Sort(_participant);

           Assert.AreEqual(_output.Length, _participant.Length);
           for (int i = 0; i < _participant.Length; i++)
           {
               Assert.AreEqual(_output[i].Name, _participant[i].Name);
               Assert.AreEqual(_output[i].Surname, _participant[i].Surname);
               Assert.AreEqual(_output[i].Result, _participant[i].Result);
           }
       }

       [TestMethod]
       public void Test_05_ArrayLinq()
       {
           Init();
           Jump();
           ArrayLinq();
           CheckStruct(jumpExpected: true);
       }

       private void Init()
       {
           for (int i = 0; i < _input.Length; i++)
               _participant[i] = new Lab7.Purple.Task2.Participant(_input[i].Name, _input[i].Surname);
       }

       private void Jump()
       {
           for (int i = 0; i < _input.Length; i++)
               _participant[i].Jump(_input[i].Distance, _input[i].Marks);
       }

       private void ArrayLinq()
       {
           for (int i = 0; i < _participant.Length; i++)
           {
               var marks = _participant[i].Marks;
               if (marks == null) continue;

               for (int j = 0; j < marks.Length; j++)
                   marks[j] = -1;
           }
       }

       private void CheckStruct(bool jumpExpected)
       {
           Assert.AreEqual(_input.Length, _participant.Length);

           for (int i = 0; i < _input.Length; i++)
           {
               Assert.AreEqual(_input[i].Name, _participant[i].Name);
               Assert.AreEqual(_input[i].Surname, _participant[i].Surname);

               if (jumpExpected)
               {
                   var marks = _participant[i].Marks;
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(_input[i].Marks.Length, marks.Length);
                   for (int j = 0; j < marks.Length; j++)
                       Assert.AreEqual(_input[i].Marks[j], marks[j]);

                   Assert.AreEqual(
                       _output.First(x => x.Name == _participant[i].Name && x.Surname == _participant[i].Surname).Result,
                       _participant[i].Result);
               }
               else
               {
                   var marks = _participant[i].Marks;
                   if (marks != null)
                       for (int j = 0; j < marks.Length; j++)
                           Assert.AreEqual(0, marks[j]);

                   Assert.AreEqual(0, _participant[i].Result);
               }
           }
       }
   }
}

