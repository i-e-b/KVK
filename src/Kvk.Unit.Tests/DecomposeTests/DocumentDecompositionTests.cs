using System;
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
			subject = new ObjectDecomposer();
		}

		[Test]
		public void Can_get_a_set_of_paths_and_values_from_anonymous_object ()
		{
			var result = subject.Decompose(anonDoc);

			Assert.That(result, Is.EquivalentTo(new []{
				new Tuple<string,string>("Url","http://example.com"),
				new Tuple<string,string>("Owner.Name","Jeff Smith"),
				new Tuple<string,string>("Owner.Address","Apt 4A, E 68th Street, New York, NY")
			}));
		}

		[Test][Description("Paths should always be full, array values should be multiples of the path")]
		public void Can_get_a_set_of_paths_and_values_from_a_complex_type_tree ()
		{
			var result = subject.Decompose(complex);
			
			Assert.That(result, Is.EquivalentTo(new []{
				new Tuple<string,string>("Date","2012-12-18T15:52:00Z"),
				new Tuple<string,string>("Name","Jeff"),
				new Tuple<string,string>("IsOK","True"),
				new Tuple<string,string>("Owner.Name","Jeff Smith"),
				new Tuple<string,string>("Owner.Address","Apt 4A"),
				new Tuple<string,string>("Owner.Address","E 68th Street"),
				new Tuple<string,string>("Owner.Address","New York"),
				new Tuple<string,string>("Owner.Address","NY"),
				new Tuple<string,string>("Owner.Notes.Note","Hello"),
				new Tuple<string,string>("Owner.Notes.Note","World")
			}));
		}

		#region sample docs (anonDoc, complex)
		readonly object anonDoc = new {
			Url = "http://example.com",
			Owner = new {
				Name = "Jeff Smith",
				Address = "Apt 4A, E 68th Street, New York, NY"
			}
		};

		readonly TypeJunk complex = new TypeJunk{
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
