using System;
using System.Collections.Generic;

namespace KVK.Core.Trie
{
	public interface ITrie<T>
	{
		/// <summary> Add a value to a key path. Returns the resulting node </summary>
		ITrieNode<T> Add(string key, T value);

		/// <summary> Find a single value by key path </summary>
		T Find(string key);

		/// <summary> Find a single node at the end of a key path </summary>
		ITrieNode<T> FindNode(string key);

		/// <summary> Find all values in key's path </summary>
		IEnumerable<T> FindAll(string key);

		/// <summary> All values stored in Trie regardless of key </summary>
		IEnumerable<T> Values { get; }

		/// <summary> Returns true if the key-path exists and has a value </summary>
		bool Contains(string path);

		/// <summary> Recover a key-path by its node, by parent chain (reasonably efficient). </summary>
		string GetKey(ITrieNode<T> node);

		/// <summary> Recover a key-path by its node, by searching (low efficiency). </summary>
		string GetKeyByValue(T node);

		/// <summary> find all values that match any substring </summary>
		IEnumerable<T> AllSubstringValues(string s);

		/// <summary> Convert one trie to another, converting the stored values </summary>
		ITrie<TNew> ToTrie<TNew>(Func<T, TNew> valueConverter);

		/// <summary> Optimise memory usage for this trie </summary>
		void Compact();

		/// <summary>Find the node for a given key, or the leaf node for the longest matching substring</summary>
		/// <remarks>
		/// If continuation from the terminal node is possible with a different input string, then that node is not
		/// returned as a 'last' node for the given input. In other words, 'last' nodes must be leaf nodes, where
		/// continuation possibility is truly unknown. The presense of a nodes array that we couldn't match to 
		/// means the search fails; it is not the design of the 'OrLast' feature to provide 'closest' or 'best'
		/// matching but rather to enable truncated tails still in the context of exact prefix matching.
		/// </remarks>
		ITrieNode<T> FindNodeOrLast(string key, out bool wasExactMatch);

		/// <summary> Root level node for this tree. </summary>
		ITrieNode<T> Root();
	}
}