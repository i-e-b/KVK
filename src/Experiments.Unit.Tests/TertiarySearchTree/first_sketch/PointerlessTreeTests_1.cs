using System;
using Experiments.Unit.Tests.TertiarySearchTree;
using NUnit.Framework;

namespace Kvk.Unit.Tests.Experimental
{
	[TestFixture]
	public class PointerlessTreeTests_1
	{
		[Test]
		public void sizes()
		{
			var pointer = IntPtr.Size*8;
			var s_ulong = sizeof(ulong)*8;
			var s_char = sizeof(char);

			Console.WriteLine("Pointer: "+IntPtr.Size*8);
			Console.WriteLine("Char: "+sizeof(char)*8);

			Console.WriteLine("Size of TernaryTreePage: "+(s_ulong + (64*s_char)));
			Console.WriteLine("Size of quarter-full pointer tree: "+((16*pointer)+(16*s_char)));

			
			Assert.Pass();
		}

		[Test]
		public void marking_population()
		{
			//"as", "at", "cup", "cute", "he", "i" and "us"
			var subject = new TernaryTreePage('c');
			subject.Add("cute");
			subject.Add("cat");
			subject.Add("cup");
			subject.Add("he");
			subject.Add("us");
			subject.Add("i");
			subject.Add("as");

			Console.WriteLine("0 1 ....2....             3");
			Console.WriteLine("rLCR--------.--------------------------._______________________|");
			Console.WriteLine(helpers.BinaryStringLeftToRight((long) subject.Population));
			Console.WriteLine(subject.Values);
		}

		[Test]
		[TestCase(0,1)]
		[TestCase(1,4)]
		[TestCase(2,7)]
		[TestCase(3,10)]
		[TestCase(4,13)]
		[TestCase(5,16)]
		[TestCase(6,19)]
		[TestCase(7,22)]
		[TestCase(8,25)]
		[TestCase(9,28)]
		[TestCase(10,31)]
		[TestCase(11,34)]
		[TestCase(12,37)]
		public void ranking(int index, int expected)
		{
			Assert.That(TernaryTreePage.LeftOf(index), Is.EqualTo(expected));
			Assert.That(TernaryTreePage.RightOf(index), Is.EqualTo(expected+2));
		}
	}
}