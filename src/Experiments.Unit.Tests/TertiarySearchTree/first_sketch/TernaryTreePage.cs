namespace Kvk.Unit.Tests.Experimental
{
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
}
