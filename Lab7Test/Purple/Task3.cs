using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab7Test.Purple
{
   [TestClass]
   public sealed class Task3
   {
       record InputRow(string Name, string Surname, double[] Marks);
       record OutputRow(string Name, string Surname, int Score, int TopPlace, double TotalMark);

       private InputRow[] _input;
       private OutputRow[] _output;
       private Lab7.Purple.Task3.Participant[] _participant;

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

           _input = inputJson.GetProperty("Task3").Deserialize<InputRow[]>()!;
           _output = outputJson.GetProperty("Task3").Deserialize<OutputRow[]>()!;

           _participant = new Lab7.Purple.Task3.Participant[_input.Length];
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab7.Purple.Task3.Participant);
           Assert.IsTrue(type.IsValueType, "Participant должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
           Assert.IsTrue(type.GetProperty("Score")?.CanRead ?? false, "Нет свойства Score");
           Assert.IsTrue(type.GetProperty("TopPlace")?.CanRead ?? false, "Нет свойства TopPlace");
           Assert.IsTrue(type.GetProperty("TotalMark")?.CanRead ?? false, "Нет свойства TotalMark");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Score")?.CanWrite ?? false, "Свойство Score должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("TopPlace")?.CanWrite ?? false, "Свойство TopPlace должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("TotalMark")?.CanWrite ?? false, "Свойство TotalMark должно быть только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
			Assert.IsNotNull(type.GetMethod("Evaluate", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double) }, null), "Нет публичного метода Evaluate(double result)");
			Assert.IsNotNull(type.GetMethod("SetPlaces", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task3.Participant[]) }, null), "Нет публичного статического метода SetPlaces(Participant[] array)");
			Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task3.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()"); 
           Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 7);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 15);
		}

       [TestMethod]
       public void Test_01_Create()
       {
           Init();
           CheckStruct(evaluated: false);
       }

       [TestMethod]
       public void Test_02_Init()
       {
           Init();
           CheckStruct(evaluated: false);
       }

       [TestMethod]
       public void Test_03_Evaluate()
       {
           Init();
           Evaluate();
           CheckStruct(evaluated: true);
       }

       [TestMethod]
       public void Test_04_SetPlaces()
       {
           Init();
           Evaluate();
           SetPlaces();
           CheckStruct(evaluated: true);
       }

       [TestMethod]
       public void Test_05_Sort()
       {
           Init();
           Evaluate();
           SetPlaces();

           Lab7.Purple.Task3.Participant.Sort(_participant);

           Assert.AreEqual(_output.Length, _participant.Length);
           for (int i = 0; i < _participant.Length; i++)
           {
               Assert.AreEqual(_output[i].Score, _participant[i].Score);
               Assert.AreEqual(_output[i].TopPlace, _participant[i].TopPlace);
               Assert.AreEqual(_output[i].TotalMark, _participant[i].TotalMark, 0.0001);
           }
       }

       [TestMethod]
       public void Test_06_ArrayLinq()
       {
           Init();
           Evaluate();
           SetPlaces();
           ArrayLinq();
           CheckStruct(evaluated: true);
       }

       private void Init()
       {
           for (int i = 0; i < _input.Length; i++)
               _participant[i] = new Lab7.Purple.Task3.Participant(_input[i].Name, _input[i].Surname);
       }

       private void Evaluate()
       {
           for (int i = 0; i < _input.Length; i++)
           {
               foreach (var mark in _input[i].Marks)
                   _participant[i].Evaluate(mark);
           }
       }

       private void SetPlaces()
       {
           Lab7.Purple.Task3.Participant.SetPlaces(_participant);
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

       private void CheckStruct(bool evaluated)
       {
           Assert.AreEqual(_input.Length, _participant.Length);

           for (int i = 0; i < _participant.Length; i++)
           {
               Assert.AreEqual(_input[i].Name, _participant[i].Name);
               Assert.AreEqual(_input[i].Surname, _participant[i].Surname);

               var marks = _participant[i].Marks;

               if (evaluated)
               {
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(_input[i].Marks.Length, marks.Length);
                   for (int j = 0; j < marks.Length; j++)
                       Assert.AreEqual(_input[i].Marks[j], marks[j], 0.0001);
               }
               else
               {
                   if (marks != null)
                       for (int j = 0; j < marks.Length; j++)
                           Assert.AreEqual(0, marks[j], 0.0001);
               }
           }
       }
   }
}

