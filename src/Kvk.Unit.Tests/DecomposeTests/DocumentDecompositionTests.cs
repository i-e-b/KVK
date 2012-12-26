using System;
using System.Collections.Generic;
using KVK.Core.Decomposer;
using NUnit.Framework;

namespace Kvk.Unit.Tests.DecomposeTests
{
	[TestFixture]
	public class DocumentDecompositionTests
	{
		IDecomposer subject;

		[SetUp]
		public void setup()
		{
			subject = new JsonObjectDecomposer();
		}

		[Test]
		public void Can_get_a_set_of_paths_and_values_from_dictionaries_and_lists ()
		{
			var result = subject.Decompose(anonDoc);

			Assert.That(result, Is.EquivalentTo(new []{
				new PathValue("Url","http://example.com"),
				new PathValue("Owner.Name","Jeff Smith"),
				new PathValue("Owner.Address","Apt 4A"),
				new PathValue("Owner.Address","E 68th Street"),
				new PathValue("Owner.Address","New York, NY")
			}));
		}

		#region sample docs (anonDoc, complex)
		readonly object anonDoc = new Dictionary<string,object>
			{
			{"Url", "http://example.com"},
			{"Owner", new Dictionary<string,object>{
				{"Name", "Jeff Smith"},
				{"Address", new List<object>{"Apt 4A", "E 68th Street", "New York, NY"}}
			}}
		};
		#endregion
	}
}
