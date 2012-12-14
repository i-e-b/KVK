using System;
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
    }
}
