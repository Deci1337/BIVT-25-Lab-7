using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab7Test.Purple
{
   [TestClass]
   public sealed class Task5
   {
       record InputResponse(string Animal, string CharacterTrait, string Concept);
       record OutputRow(string[] answers);

       private InputResponse[] _inputResponses; 
       private string[][] _outputRows;


       private Lab7.Purple.Task5.Response[] _responses;
       private Lab7.Purple.Task5.Research[] _researchGroups;
       private string[][] _topResponses;

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

           _inputResponses = inputJson.GetProperty("Task5").Deserialize<InputResponse[]>()!;

           _outputRows = outputJson.GetProperty("Task5").Deserialize<string[][]>()!;
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab7.Purple.Task5.Response);
           Assert.IsTrue(type.IsValueType, "Response должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Animal")?.CanRead ?? false, "Нет свойства Animal");
           Assert.IsTrue(type.GetProperty("CharacterTrait")?.CanRead ?? false, "Нет свойства CharacterTrait");
           Assert.IsTrue(type.GetProperty("Concept")?.CanRead ?? false, "Нет свойства Concept");
           Assert.IsFalse(type.GetProperty("Animal")?.CanWrite ?? false, "Animal только для чтения");
           Assert.IsFalse(type.GetProperty("CharacterTrait")?.CanWrite ?? false, "CharacterTrait только для чтения");
           Assert.IsFalse(type.GetProperty("Concept")?.CanWrite ?? false, "Concept только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string), typeof(string) }, null), "Нет публичного конструктора Response(string animal, string characterTrait, string concept)");
			Assert.IsNotNull(type.GetMethod("CountVotes", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab7.Purple.Task5.Response[]), typeof(int) }, null), "Нет публичного метода CountVotes(Response[] responses, int q)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 3);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 9);

			type = typeof(Lab7.Purple.Task5.Research);
           Assert.IsTrue(type.IsValueType, "Research должен быть структурой");
			Assert.AreEqual(type.GetFields().Count(f => f.IsPublic), 0);
			Assert.IsTrue(type.GetProperty("Responses")?.CanRead ?? false, "Нет свойства Responses");
           Assert.IsFalse(type.GetProperty("Responses")?.CanWrite ?? false, "Responses только для чтения");
			Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null), "Нет публичного конструктора Research(string name)");
			Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string[]) }, null), "Нет публичного метода Add(string[] answers)");
			Assert.IsNotNull(type.GetMethod("GetTopResponses", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int) }, null), "Нет публичного метода GetTopResponses(int question)");
			Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
			Assert.AreEqual(type.GetProperties().Count(f => f.PropertyType.IsPublic), 2);
			Assert.AreEqual(type.GetConstructors().Count(f => f.IsPublic), 1);
			Assert.AreEqual(type.GetMethods().Count(f => f.IsPublic), 9);
		}

       [TestMethod]
       public void Test_01_InitResponses()
       {
           InitResponse();
           CheckResponses();
       }

       [TestMethod]
       public void Test_02_InitResearch()
       {
           InitResponse();
           InitResearch();
           CheckResearch();
       }

       [TestMethod]
       public void Test_03_AddOne()
       {
           InitResponse();
           InitResearch();
           AddOne();
           CheckResearch(true);
       }

       [TestMethod]
       public void Test_04_GetTopResponses()
       {
           InitResponse();
           InitResearch();
           AddOne();
           GetTopResponses();
           CheckTopResponses();
       }

       private void InitResponse()
       {
           _responses = _inputResponses
               .Select(r => new Lab7.Purple.Task5.Response(r.Animal, r.CharacterTrait, r.Concept))
               .ToArray();
       }

       private void InitResearch()
       {
           _researchGroups = new Lab7.Purple.Task5.Research[_outputRows.Length];
           for (int i = 0; i < _researchGroups.Length; i++)
           {
               _researchGroups[i] = new Lab7.Purple.Task5.Research("Search " + i.ToString());
           }
       }

       private void AddOne()
       {
           for (int i = 0; i < _researchGroups.Length; i++)
           {
               foreach (var r in _responses)
                   _researchGroups[i].Add(new string[] { r.Animal, r.CharacterTrait, r.Concept });
           }
       }

       private void GetTopResponses()
       {
           _topResponses = new string[_researchGroups.Length][];
           for (int i = 0; i < _researchGroups.Length; i++)
           {
               _topResponses[i] = _researchGroups[i].GetTopResponses(i+1);
           }
       }

       private void CheckResponses()
       {
           Assert.AreEqual(_inputResponses.Length, _responses.Length);
           for (int i = 0; i < _responses.Length; i++)
           {
               Assert.AreEqual(_inputResponses[i].Animal, _responses[i].Animal);
               Assert.AreEqual(_inputResponses[i].CharacterTrait, _responses[i].CharacterTrait);
               Assert.AreEqual(_inputResponses[i].Concept, _responses[i].Concept);
           }
       }

       private void CheckResearch(bool filled = false)
       {
           for (int i = 0; i < _researchGroups.Length; i++)
           {
               if (filled)
               {
                   Assert.AreEqual(_responses.Length, _researchGroups[i].Responses.Length);
                   for (int j = 0; j < _responses.Length; j++)
                   {
                       Assert.AreEqual(_responses[j].Animal, _researchGroups[i].Responses[j].Animal);
                       Assert.AreEqual(_responses[j].CharacterTrait, _researchGroups[i].Responses[j].CharacterTrait);
                       Assert.AreEqual(_responses[j].Concept, _researchGroups[i].Responses[j].Concept);
                   }
               }
               else
               {
                   Assert.IsNotNull(_researchGroups[i].Responses);
               }
           }
       }

       private void CheckTopResponses()
       {
           for (int i = 0; i < _outputRows.Length; i++)
           {
               Assert.AreEqual(_outputRows[i].Length, _topResponses[i].Length);
               for (int j = 0; j < _outputRows[i].Length; j++)
               {
                   Assert.AreEqual(_outputRows[i][j], _topResponses[i][j]);
               }
           }
       }
   }
}

