using System.Collections.Generic;

namespace KVK.Core.Trie
{
	/// <summary>
	/// Marker interface to make working with node types easier
	/// </summary>
	public interface ITrieNode { }

	public interface ITrieNode<T> : ITrieNode
	{
		T Value { get; set; } 

		bool HasValue { get; }
		bool IsLeaf { get; }

		ITrieNode<T> this[char c] { get; }
		ITrieNode<T>[] ChildNodes { get; }
		ITrieNode<T> Parent { get; }

		void SetLeaf();
		int ChildCount { get; }
		bool ShouldOptimize { get; }
		ITrieNode<T> FindRoot();

		KeyValuePair<char, ITrieNode<T>>[] CharNodePairs();

		ITrieNode<T> AddChild(char c, ref int node_count);

		IEnumerable<T> SubsumedValues();
		IEnumerable<ITrieNode<T>> SubsumedNodes();
		IEnumerable<ITrieNode<T>> SubsumedNodesExceptThis();
		void OptimizeChildNodes();
	}
}