using System;
using System.Linq;
using NUnit.Framework;

namespace Kvk.Unit.Tests.Experimental
{
	[TestFixture]
	public class PointerlessTree
	{
		[Test]
		public void sizes()
		{
			var pointer = IntPtr.Size*8;
			var s_ulong = sizeof(ulong)*8;
			var s_char = sizeof(char);

			Console.WriteLine("Pointer: "+IntPtr.Size*8);
			Console.WriteLine("Char: "+sizeof(char)*8);

			Console.WriteLine("Size of DTT: "+(s_ulong + (64*s_char)));
			Console.WriteLine("Size of half-full pointer tree: "+((32*pointer)+(32*s_char)));

			
			Assert.Pass();
		}

		[Test]
		public void marking_population()
		{
			// a tree with up to 64 nodes can be represented by:
			//   ulong population = 0x0000000000000000; 
			//   var values = new char[64];
			// the 'population' follows potential nodes left->right, top->bottom.
			// a value of '0' is no node (like a null pointer). A value of '1' is
			// a node with a value. Any no-node has no potential children, and
			// isn't counted in the population anymore.
			// all values are in population order and are stored densely.
			// (dense storage may change).

			var subject = new TernaryTreePage('c');
			subject.Add("cute");
			subject.Add("cat");
			subject.Add("cup");

			Console.WriteLine("rLCR-----------------------------------------------------------|");
			Console.WriteLine(Convert.ToString((long)subject.Population,2).Reverse().ToArray());
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

	class TernaryTreePage // storing chars.
	{
		public ulong Population = 0x0000000000000000; 
		public char[] Values = new char[64];
		
		public TernaryTreePage(char rootNode)
		{
			// for simplicity, root is always populated
			Values[0] = rootNode;
			Population |= 1;
		}

		bool IsSet(int i) { return (Population & (1ul << i)) != 0; }
		void Set(int i) { Population |= 1ul << i; }
		//void Unset(int i) { Population ^= (Population & (1ul << i)); }

		public static int LeftOf(int i)
		{
			return (i * 3) + 1;
		}
		public static int Below(int i)
		{
			return (i * 3) + 2;
		}
		public static int RightOf(int i)
		{
			return (i * 3) + 3;
		}

		public void Add(string s)
		{
			char current = Values[0];
			int idx = 0;
			int spos = 0;
			while (spos < s.Length)
			{
				var ch = s[spos];
				if (ch < current)
				{// insert left
					// don't advance spos
					// if no left, add pop & value
					// set current position to left node pos
					var next = LeftOf(idx);
					if (!IsSet(next)) Values[next] = ch;
					Set(next);
					idx=next;
					current=Values[idx];
				}
				else if (ch > current)
				{// insert right
					// don't advance i
					// if no right, add pop & value
					// set current position to left node pos
					var next = RightOf(idx);
					if (!IsSet(next)) Values[next] = ch;
					Set(next);
					idx=next;
					current=Values[idx];
				}
				else
				{// insert centre
					spos++;
					// if we're at the end of the string, exit
					// otherwise add a centre node
					if (spos == s.Length) return; // would also add a terminus mark here

					ch = s[spos];
					var next = Below(idx);
					if (!IsSet(next)) Values[next] = ch;
					Set(next);
					idx = next;
					current=Values[idx];
				}
			}
		}
	}
}
