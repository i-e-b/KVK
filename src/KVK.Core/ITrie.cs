using System;
using System.Collections.Generic;

namespace KVK.Core
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

		/// <summary> Recover a key-path by its node, by searching. </summary>
		string GetKey(ITrieNode<T> node);

		/// <summary> Recover a key-path by its node, by searching. </summary>
		string GetKeyByValue(T node);

		/// <summary> find all values that match any substring </summary>
		IEnumerable<T> AllSubstringValues(string s);

		/// <summary> Convert one trie to another, converting the stored values </summary>
		ITrie<TNew> ToTrie<TNew>(Func<T, TNew> value_converter);
	}
}