using KVK.Core;
using NUnit.Framework;

namespace Kvk.Unit.Tests
{
	[TestFixture]
    public class TrieTests
    {
		ITrie<object> subject;

		[SetUp]
		public void A_trie_with_some_values ()
		{
			subject = new Trie<object>();

			subject.Add("polyglot", "one");
			subject.Add("polyhedron", "two");
			subject.Add("polyglottal", "three");
		}

		[Test]
		public void Can_find_a_stored_value_based_on_a_key_string ()
		{
			Assert.That(subject.Find("polyglot"), Is.EqualTo("one"));
			Assert.That(subject.Find("polyhedron"), Is.EqualTo("two"));
		}

		[Test]
		public void Can_find_a_node_based_on_a_common_prefix()
		{
			Assert.That(subject.FindNode("poly"), Is.Not.Null);
		}

		[Test]
		public void Finding_a_node_based_on_a_non_existent_prefix_returns_null()
		{
			Assert.That(subject.FindNode("mono"), Is.Null);
		}

		[Test]
		public void Can_find_all_node_values_in_a_string_path ()
		{
			Assert.That(subject.FindAll("polyglottal"), Is.EquivalentTo(new []{"one", "three"}));
		}

		[Test]
		public void Can_read_out_all_stored_values_regardless_of_key ()
		{
			Assert.That(subject.Values, Is.EquivalentTo(new[]{"one","two","three"}));
		}

		[Test]
		[TestCase("poly")]
		[TestCase("polyglottal")]
		public void Can_recover_a_key_path_by_its_node (string path)
		{
			var node = subject.FindNode(path);
			var result = subject.GetKey(node);

			Assert.That(result, Is.EqualTo(path));
		}


		[Test]
		[TestCase("one","polyglot")]
		[TestCase("two","polyhedron")]
		public void Can_recover_a_key_path_by_its_value (object value, string expectedPath)
		{
			var result = subject.GetKeyByValue(value);

			Assert.That(result, Is.EqualTo(expectedPath));
		}

		[Test]
		[TestCase("poly",false)]
		[TestCase("polyhedron", true)]
		[TestCase("polyglot", true)]
		[TestCase("polyglott", false)]
		[TestCase("different", false)]
		public void Can_find_if_a_key_path_exists_and_has_a_value (string path, bool expected)
		{
			Assert.That(subject.Contains(path), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("Poly put the kettle on", new string[0])]
		[TestCase("What about polyglot persistence?", new[] { "one" })]
		[TestCase("polyglottal", new[] { "one", "three" })]
		[TestCase("persaypolyglottalpolyhedronplay", new[] { "one", "three", "two" })]
		public void Given_a_string_can_find_all_values_that_match_any_substring (string input, string[] expected)
		{
			Assert.That(subject.AllSubstringValues(input), Is.EquivalentTo(expected));
		}

		[Test]
		public void Can_convert_one_trie_to_another ()
		{
			var other = subject.ToTrie(old => (old.ToString() == "one") ? ("X") : ("Y"));

			Assert.That(other.AllSubstringValues("polyglot, polyhedron, polyglottal"), Is.EquivalentTo(new[] { "X", "Y", "X", "Y" }));
		}

		[Test]
		public void Trie_still_works_after_being_compacted()
		{
			subject.Compact();
			Assert.That(subject.AllSubstringValues("persaypolyglottalpolyhedronplay"), Is.EquivalentTo(new[] { "one", "three", "two" }));
		}

		[Test]
		[TestCase("polyglot", true, "one", "polyglot")]
		[TestCase("polyglottalism", false, "three", "polyglottal")]
		public void Can_find_closest_matching_substring_nodes (string target, bool exact, object expectedValue, string expectedPath)
		{
			bool wasExact;
			var result = subject.FindNodeOrLast(target, out wasExact);
			var path = subject.GetKey(result);

			Assert.That(result, Is.Not.Null, "result");
			Assert.That(result.Value, Is.EqualTo(expectedValue), "value");
			Assert.That(wasExact, Is.EqualTo(exact), "exact");
			Assert.That(path, Is.EqualTo(expectedPath), "path");
		}

		[Test]
		public void Finding_closest_substring_doesnt_match_non_leaf_paths ()
		{
			bool b;
			var result = subject.FindNodeOrLast("polygon", out b);
			
			Assert.That(result, Is.Null);
		}

		[Test]
		public void Finding_closest_substring_matches_partial_paths ()
		{
			bool b;
			var result = subject.FindNodeOrLast("poly", out b);
			
			Assert.That(result, Is.Not.Null);
			Assert.That(b, Is.True);
		}
    }
}
