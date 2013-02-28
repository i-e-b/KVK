using System;
using System.Linq;
using Ewah;

namespace Experiments.Unit.Tests.TertiarySearchTree
{
	public class helpers
	{
		public static char[] BinaryStringLeftToRight(long subject)
		{
			const string padding = "0000000000000000000000000000000000000000000000000000000000000000";
			return Convert.ToString(subject,2).Reverse().Concat(padding).Take(64).ToArray();
		} 

		
		public static char[] BinaryStringLeftToRight(EwahCompressedBitArray subject)
		{
			char[] padding = "0000000000000000000000000000000000000000000000000000000000000000".ToArray();
			foreach (var position in subject)
			{
				if (position>63)break;
				padding[position] = '1';
			}
			return padding;
		} 
	}
}