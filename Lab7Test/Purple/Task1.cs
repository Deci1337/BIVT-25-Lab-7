using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab7Test.Purple
{
   [TestClass]
   public sealed class Task1
   {
       record InputRow(string Name, string Surname, double[] Coefs, int[][] Marks);
       record OutputRow(string Name, string Surname, double TotalScore);

       private InputRow[] _input;
       private OutputRow[] _output;
       private Lab7.Purple.Task1.Participant[] _participant;

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

           _input = inputJson.GetProperty("Task1").Deserialize<InputRow[]>()!;
           _output = outputJson.GetProperty("Task1").Deserialize<OutputRow[]>()!;

           _participant = new Lab7.Purple.Task1.Participant[_input.Length];
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab7.Purple.Task1.Participant);
           Assert.IsTrue(type.IsValueType, "Participant должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Coefs")?.CanRead ?? false, "Нет свойства Coefs");
           Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
           Assert.IsTrue(type.GetProperty("TotalScore")?.CanRead ?? false, "Нет свойства TotalScore");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Coefs")?.CanWrite ?? false, "Свойство Coefs должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("TotalScore")?.CanWrite ?? false, "Свойство TotalScore должно быть только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
			Assert.IsNotNull(type.GetMethod("SetCriterias", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double[]) }, null), "Нет публичного метода SetCriterias(double[] coefs)");
			Assert.IsNotNull(type.GetMethod("Jump", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int[]) }, null), "Нет публичного метода Jump(int[] marks)");
			Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task1.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 5);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 13);
		}

       [TestMethod]
       public void Test_01_Create()
       {
           Init();
           CheckStruct(coefsExpected: false, jumpsExpected: false);
       }

       [TestMethod]
       public void Test_02_Init()
       {
           Init();
           CheckStruct(coefsExpected: false, jumpsExpected: false);
       }

       [TestMethod]
       public void Test_03_Coefs()
       {
           Init();
           SetCriterias();
           CheckStruct(coefsExpected: true, jumpsExpected: false);
       }

       [TestMethod]
       public void Test_04_Jumps()
       {
           Init();
           SetCriterias();
           Jump();
           CheckStruct(coefsExpected: true, jumpsExpected: true);
       }

       [TestMethod]
       public void Test_05_Sort()
       {
           Init();
           SetCriterias();
           Jump();

           Lab7.Purple.Task1.Participant.Sort(_participant);

           Assert.AreEqual(_output.Length, _participant.Length);
           for (int i = 0; i < _participant.Length; i++)
           {
               Assert.AreEqual(_output[i].Name, _participant[i].Name);
               Assert.AreEqual(_output[i].Surname, _participant[i].Surname);
               Assert.AreEqual(_output[i].TotalScore, _participant[i].TotalScore, 0.01);
           }
       }

       [TestMethod]
       public void Test_06_ArrayLinq()
       {
           Init();
           SetCriterias();
           Jump();
           ArrayLinq();
           CheckStruct(coefsExpected: true, jumpsExpected: true);
       }

       private void Init()
       {
           for (int i = 0; i < _input.Length; i++)
               _participant[i] = new Lab7.Purple.Task1.Participant(_input[i].Name, _input[i].Surname);
       }

       private void SetCriterias()
       {
           for (int i = 0; i < _input.Length; i++)
               _participant[i].SetCriterias(_input[i].Coefs);
       }

       private void Jump()
       {
           for (int i = 0; i < _input.Length; i++)
           {
               foreach (var jump in _input[i].Marks)
                   _participant[i].Jump(jump);
           }
       }

       private void ArrayLinq()
       {
           for (int i = 0; i < _participant.Length; i++)
           {
               var marks = _participant[i].Marks;
               if (marks == null) continue;

               int rows = marks.GetLength(0);
               int cols = marks.GetLength(1);
               for (int r = 0; r < rows; r++)
                   for (int c = 0; c < cols; c++)
                       marks[r, c] = -1;

               var coefs = _participant[i].Coefs;
               for (int c = 0; c < coefs.Length; c++)
                   coefs[c] = -1;
           }
       }

       private void CheckStruct(bool coefsExpected, bool jumpsExpected)
       {
           Assert.AreEqual(_input.Length, _participant.Length);

           for (int i = 0; i < _input.Length; i++)
           {
               Assert.AreEqual(_input[i].Name, _participant[i].Name);
               Assert.AreEqual(_input[i].Surname, _participant[i].Surname);

               if (coefsExpected)
                   for (int c = 0; c < _input[i].Coefs.Length; c++)
                       Assert.AreEqual(_input[i].Coefs[c], _participant[i].Coefs[c], 0.0001);

               if (jumpsExpected)
               {
                   var marks = _participant[i].Marks;
                   Assert.IsNotNull(marks);

                   int rows = _input[i].Marks.Length;
                   int cols = _input[i].Marks[0].Length;

                   Assert.AreEqual(rows, marks.GetLength(0));
                   Assert.AreEqual(cols, marks.GetLength(1));

                   for (int r = 0; r < rows; r++)
                       for (int c = 0; c < cols; c++)
                           Assert.AreEqual(_input[i].Marks[r][c], marks[r, c]);
               }
           }
       }
   }
}

