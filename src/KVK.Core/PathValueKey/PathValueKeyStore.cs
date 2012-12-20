using System.Collections.Generic;
using System.Linq;
using KVK.Core.Decomposer;
using KVK.Core.Trie;

namespace KVK.Core.PathValueKey
{
	public class PathValueKeyStore<T> : IPathValueKeyStore<T>
	{
		class Trie : Trie<ISet<T>>{}

		readonly IDictionary<string, Trie> paths;
		readonly IDictionary<T, ISet<ITrieNode>> backreferences;

		public PathValueKeyStore()
		{
			backreferences = new Dictionary<T, ISet<ITrieNode>>();
			paths = new Dictionary<string, Trie>();
		}

		public ITrie<ISet<T>> Lookup(string path)
		{
			if ( ! paths.ContainsKey(path))
			{
				paths.Add(path, new Trie());
			}
			return paths[path];
		}

		public void Add(string path, string value, T key)
		{
			var lookup = Lookup(path);
			var list = lookup.Find(value) ?? new HashSet<T>();
			list.Add(key);
			var node = lookup.Add(value, list);

			AddBackreference(key, node);
		}


		public IReadOnlyList<T> List(string path, string value)
		{
			return Lookup(path).Find(value).ToArray();
		}

		public void Remove(string path, string value, T key)
		{
			var lookup = Lookup(path);
			var node = lookup.FindNode(value);
			if (node == null) return;

			node.Value.Remove(key);
			RemoveBackreferences(key, node);
		}

		// TODO: not yet under test
		void AddBackreference(T key, ITrieNode<ISet<T>> node)
		{
			if (! backreferences.ContainsKey(key))
			{
				backreferences.Add(key, new HashSet<ITrieNode>());
			}
			backreferences[key].Add(node);
		}

		// TODO: not yet under test
		void RemoveBackreferences(T key, ITrieNode<ISet<T>> node)
		{

		}

		// TODO: not yet under test
		public IEnumerable<PathValue> Recover(T matchingKey)
		{
			// TODO: this can be made more efficient still;
			// rather than looping through all the paths for every node, 
			// we can make matching sets.
			foreach (ITrieNode<ISet<T>> node in backreferences[matchingKey])
			{
				var root = node.FindRoot();
				foreach (var path in paths.Keys)
				{
					var trie = paths[path];
					if (root != trie.Root()) continue;

					var value = trie.GetKey(node);
					yield return new PathValue(path, value);
				}
			}

			/*
			foreach (var path in paths.Keys)
			{
				var trie = paths[path];
				foreach (var value in trie.Values)
				{
					if (!value.Contains(matchingKey)) continue;
					var kvkPath = trie.GetKeyByValue(value); // very, very inefficient!

					yield return new PathValue(path, kvkPath);
				}
			}
			*/
		}
	}
}