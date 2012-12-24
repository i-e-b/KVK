using System;
using System.Linq;
using KVK.Core;
using NUnit.Framework;

namespace Kvk.Unit.Tests
{
	[TestFixture]
	public class VeryBasicDocumentStoreTests
	{
		IDocumentStore subject;

		[SetUp]
		public void a_document_store ()
		{
			subject = new DocumentStore();
		}

		[Test]
		public void Can_store_and_retrieve_a_sample_document ()
		{
			subject.Store(sample_document);
			var result = (TypeJunk)subject.FindAll("Owner.Name", "Jeff Smith").Single();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Date, Is.EqualTo(sample_document.Date));
		}

		[Test]
		public void Can_recover_a_documents_data_from_index ()
		{
			subject.Store(sample_document);
/*
			var start = DateTime.Now;
			for (int i = 0; i < 1e4; i++)
			{
				var x = subject.RecoverFromIndex(0).ToList();
			}
			var time = DateTime.Now - start;
			Console.WriteLine(time);*/

			var result = subject.RecoverFromIndex(0).ToList();

			foreach (var pathValue in result)
			{
				Console.WriteLine(pathValue);
			}

			Assert.That(result.Count, Is.EqualTo(10));
		}

		#region var sample_document;
		readonly TypeJunk sample_document = new TypeJunk{
			Date = new DateTime(2012, 12, 18, 15, 52, 00),
			IsOK = true,
			Name = "Jeff",
			Owner = new SubType {
				Address = new []{"Apt 4A", "E 68th Street", "New York", "NY"},
				Name = "Jeff Smith",
				Notes = new NotesType {
					Note =  new []{"Hello", "World"}
				}
			}
		};
		#endregion
	}
}
