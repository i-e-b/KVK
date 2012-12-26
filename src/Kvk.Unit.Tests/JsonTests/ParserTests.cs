using System.Collections.Generic;
using System.Linq;
using KVK.Core.Decomposer;
using KVK.Core.Json;
using NUnit.Framework;

namespace Kvk.Unit.Tests.JsonTests
{
	[TestFixture]
	public class ParserTests
	{
		JsonParser subject;
		const string Doc = @"{
	""Key"" : {
		""Path"" : [""Value 1"", ""Value 2""]
	}
}";

		[SetUp]
		public void setup()
		{
			subject = new JsonParser(Doc, false);
		}

		[Test]
		public void reading_a_json_document_results_in_a_correct_object()
		{
			var result = subject.Decode() as Dictionary<string, object>;
			Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());
			Assert.That(result["Key"], Is.InstanceOf<Dictionary<string, object>>());

			var a = result["Key"] as Dictionary<string, object>;
			Assert.That(a["Path"], Is.InstanceOf<List<object>>());

			Assert.That(a["Path"], Is.EquivalentTo(new [] {"Value 1", "Value 2"}));
		}

		[Test]
		public void Reading_a_json_document_results_in_an_object_that_can_be_decomposed()
		{
			var result = subject.Decode();

			var paths = new JsonObjectDecomposer().Decompose(result).ToList();

			Assert.That(paths, Contains.Item(new PathValue("Key.Path", "Value 1")));
			Assert.That(paths, Contains.Item(new PathValue("Key.Path", "Value 2")));
			Assert.That(paths.Count, Is.EqualTo(2));
		}
	}
}
