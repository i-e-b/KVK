using KVK.Core.Decomposer;
using NUnit.Framework;

namespace Kvk.Unit.Tests.PvkTests
{
	[TestFixture]
	public class PathValueTests
	{
		[Test]
		public void Path_values_store_path ()
		{
			var x = new PathValue("this path", "");
			Assert.That(x.Path, Is.EqualTo("this path"));
		}

		[Test]
		public void Path_values_store_value ()
		{
			var x = new PathValue("", "a value");
			Assert.That(x.Value, Is.EqualTo("a value"));
		}

		[Test]
		public void Path_values_are_considered_equal_if_both_path_and_value_are_equal ()
		{
			var x = new PathValue("path", "value");
			var y = new PathValue("path", "value");

			Assert.That(x, Is.EqualTo(y));
		}

		[Test]
		public void Path_values_to_string_gives_nice_output ()
		{
			var x = new PathValue("path", "value");

			Assert.That(x.ToString(), Is.EqualTo("\"path\" : \"value\""));
		}

		[Test]
		public void Path_values_are_considered_inequal_if_paths_are_different ()
		{
			var x = new PathValue("path1", "value");
			var y = new PathValue("path2", "value");

			Assert.That(x, Is.Not.EqualTo(y));
		}
		[Test]
		public void Path_values_are_considered_inequal_if_values_are_different ()
		{
			var x = new PathValue("path", "value1");
			var y = new PathValue("path", "value2");

			Assert.That(x, Is.Not.EqualTo(y));
		}

		[Test]
		public void Hashcodes_are_the_same_where_paths_are_the_same ()
		{
			var x = new PathValue("path", "value1").GetHashCode();
			var y = new PathValue("path", "value2").GetHashCode();

			Assert.That(x, Is.EqualTo(y));
		}
	}
}
