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
				new PathValue("Url","http://example.com"),
				new PathValue("Owner.Name","Jeff Smith"),
				new PathValue("Owner.Address","Apt 4A, E 68th Street, New York, NY")
			}));
		}

		[Test][Description("Paths should always be full, array values should be multiples of the path")]
		public void Can_get_a_set_of_paths_and_values_from_a_complex_type_tree ()
		{
			var result = subject.Decompose(complex);
			
			Assert.That(result, Is.EquivalentTo(new []{
				new PathValue("Date","2012-12-18T15:52:00Z"),
				new PathValue("Name","Jeff"),
				new PathValue("IsOK","True"),
				new PathValue("Owner.Name","Jeff Smith"),
				new PathValue("Owner.Address","Apt 4A"),
				new PathValue("Owner.Address","E 68th Street"),
				new PathValue("Owner.Address","New York"),
				new PathValue("Owner.Address","NY"),
				new PathValue("Owner.Notes.Note","Hello"),
				new PathValue("Owner.Notes.Note","World")
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
