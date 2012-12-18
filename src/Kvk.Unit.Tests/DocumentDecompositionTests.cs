using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Kvk.Unit.Tests
{
	[TestFixture]
	public class DocumentDecompositionTests
	{
		readonly object anonDoc = new {
			Url = "http://example.com",
			Owner = new {
				Name = "Jeff Smith",
				Address = "Apt 4A, E 68th Street, New York, NY"
			}
		};

		[Test]
		public void Can_get_a_set_of_paths_and_values ()
		{
			var result = Decompose(anonDoc);

			Assert.That(result, Is.EquivalentTo(new []{
				new Tuple<string,string>("Url","http://example.com"),
				new Tuple<string,string>("Owner.Name","Jeff Smith"),
				new Tuple<string,string>("Owner.Address","Apt 4A, E 68th Street, New York, NY")
			}));
		}

		IEnumerable<Tuple<string, string>> Decompose(object o, string root = "")
		{
			var props = o.GetType().GetProperties();
			foreach (var property in props)
			{
				if (!(property.CanRead)) continue;
				if (property.PropertyType == typeof(string))
				{
					yield return new Tuple<string,string>(root + property.Name, property.GetValue(o).ToString());
				} else
				{
					var others = Decompose(property.GetValue(o), property.Name+".");
					foreach (var other in others) { yield return other; }
				}
			}
		}
	}
}
