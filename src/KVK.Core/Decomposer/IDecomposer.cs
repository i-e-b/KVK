using System;
using System.Collections.Generic;

namespace KVK.Core.Decomposer
{
	public interface IDecomposer
	{
		IEnumerable<Tuple<string, string>> Decompose(object obj);
	}
}