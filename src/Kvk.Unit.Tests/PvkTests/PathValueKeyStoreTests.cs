using System;
using System.Linq;
using KVK.Core.PathValueKey;
using NUnit.Framework;

namespace Kvk.Unit.Tests.PvkTests
{
	[TestFixture]
	public class PathValueKeyStoreTests
	{
		IPathValueKeyStore<Guid> subject;

		[SetUp]
		public void pvk_store ()
		{
			subject = new PathValueKeyStore<Guid>();
		}

		[Test]
		public void Can_store_and_retrieve_a_key_by_path_and_value ()
		{
			var key = Guid.NewGuid();

			subject.Add("my.path", "a value", key);
			var result = subject.List("my.path", "a value");

			Assert.That(result.Single(), Is.EqualTo(key));
		}

		[Test]
		public void Can_store_and_retrieve_multiple_keys_by_the_same_path_and_value ()
		{
			var key1 = Guid.NewGuid();
			var key2 = Guid.NewGuid();

			subject.Add("my.path", "a value", key1);
			subject.Add("my.path", "a value", key2);
			var result = subject.List("my.path", "a value");
			
			Assert.That(result, Is.EquivalentTo(new[] {key1, key2}));
		}

		[Test]
		public void Storing_different_key_paths_are_stored_and_accessed_separately ()
		{
			var key = Guid.NewGuid();

			subject.Add("my.path.1", "a value", key);
			subject.Add("my.path.2", "a value", key);

			var result = subject.List("my.path.1", "a value");

			Assert.That(result.Single(), Is.EqualTo(key));
		}

		[Test]
		public void Can_remove_keys_from_the_store ()
		{
			var key1 = Guid.NewGuid();
			var key2 = Guid.NewGuid();

			subject.Add("my.path", "a value", key1);
			subject.Add("my.path", "a value", key2);
			var result = subject.List("my.path", "a value");
			
			Assert.That(result, Is.EquivalentTo(new[] {key1, key2}));

			subject.Remove("my.path", "a value", key1);
			result = subject.List("my.path", "a value");
			
			Assert.That(result, Is.EquivalentTo(new[] {key2}));
		}
	}
}
