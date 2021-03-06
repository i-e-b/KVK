using System.Collections.Generic;
using KVK.Core.Decomposer;

namespace KVK.Core.PathValueKey
{
	public interface IPathValueKeyStore<T>
	{
		void Add(string path, string value, T key);
		IReadOnlyList<T> List(string path, string value);
		void Remove(string path, string value, T key);

		IEnumerable<PathValue> Recover(T matchingKey);
	}
}