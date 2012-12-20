using System.Collections.Generic;
using System.Linq;
using KVK.Core.Trie;

namespace KVK.Core.PathValueKey
{
	public class PathValueKeyStore<T> : IPathValueKeyStore<T>
	{
		readonly IDictionary<string, ITrie<ISet<T>>> paths;

		public PathValueKeyStore()
		{
			paths = new Dictionary<string, ITrie<ISet<T>>>();
		}

		public ITrie<ISet<T>> Lookup(string path)
		{
			if ( ! paths.ContainsKey(path))
			{
				paths.Add(path, new Trie<ISet<T>>());
			}
			return paths[path];
		}

		public void Add(string path, string value, T key)
		{
			var lookup = Lookup(path);
			var list = lookup.Find(value) ?? new HashSet<T>();
			list.Add(key);
			lookup.Add(value, list);
		}

		public IReadOnlyList<T> List(string path, string value)
		{
			return Lookup(path).Find(value).ToArray();
		}

		public void Remove(string path, string value, T key)
		{
			var lookup = Lookup(path);
			var list = lookup.Find(value);
			if (list == null) return;

			list.Remove(key);
		}
	}
}