using System;
using System.Collections.Generic;
using System.Linq;
using Experiments.Unit.Tests.TertiarySearchTree;
using NUnit.Framework;
using BitArray = Ewah.EwahCompressedBitArray;

namespace Experiments.Unit.Tests
{
	[TestFixture]
    public class Experiments
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
			var subject = new TernaryTreePage_ewah('c');
			subject.Add("cute");
			subject.Add("cat");
			subject.Add("cup");
			subject.Add("he");
			subject.Add("us");
			subject.Add("i");
			subject.Add("as");
			subject.Add("me?");
			subject.Add("so what");
			subject.Add("Right, here's a properly long string of the sort you really would see in a database");
			subject.Add("Right, here's a properly long string of the sort you really would see in a data store");

			Console.WriteLine(helpers.BinaryStringLeftToRight(subject.Population));

			Console.WriteLine(subject.addedCharacters+" characters added");
			subject.Values.Print();
		}
		[Test]
		public void ewah_array_ternary_tree ()
		{

		}
    }

	
	class TernaryTreePage_ewah // storing chars.
	{
		public BitArray Population = new BitArray();
		public Map<char> Values = new Map<char>();
		public int addedCharacters = 0;
		
		public TernaryTreePage_ewah(char rootNode)
		{
			// for simplicity, root is always populated
			Values[0] = rootNode;
			Set(0);
		}

		bool IsSet(long i) {
			return Population.Intersects(SetAt(i));
		}
		void Set(long i) { 
			if (!Population.Set(i))
			{
				Population = Population.Or(SetAt(i));
			}
		}

		BitArray SetAt(long i)
		{
			var x = new BitArray();
			x.Set(i);
			return x;
		}

		public static long LeftOf(long i)
		{
			return (i * 3) + 1;
		}
		public static long Below(long i)
		{
			return (i * 3) + 2;
		}
		public static long RightOf(long i)
		{
			return (i * 3) + 3;
		}

		public void Add(string s)
		{
			char current = Values[0];
			long idx = 0;
			int spos = 0;
			while (spos < s.Length)
			{
				var ch = s[spos];
				if (ch < current)
				{
					var next = LeftOf(idx);
					if (!IsSet(next)) Values[next] = ch;
					Set(next);
					idx=next;
					current=Values[idx];
				}
				else if (ch > current)
				{
					var next = RightOf(idx);
					if (!IsSet(next)) Values[next] = ch;
					Set(next);
					idx=next;
					current=Values[idx];
				}
				else
				{
					addedCharacters++;
					spos++;
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

	class Map<TV>
	{
		readonly Dictionary<long, TV> _d;

		public Map()
		{
			_d = new Dictionary<long, TV>();
		}
		public TV this[long idx]
		{
			get
			{
				if (_d.ContainsKey(idx)) return _d[idx];
				return default(TV);
			}
			set
			{
				if (_d.ContainsKey(idx)) _d[idx] = value;
				else _d.Add(idx, value);
			}
		}
		public void Print()
		{
			long m = _d.Keys.Max();
			Console.WriteLine("Contains "+_d.Keys.Count+" items with max index "+m);
			foreach (var value in _d.Values)
			{
				Console.Write(value);
			}
		}
	}
}
