using System.Collections.Generic;

namespace KVK.Core.Decomposer
{
	public interface IDecomposer
	{
		IEnumerable<PathValue> Decompose(object obj);
	}
}