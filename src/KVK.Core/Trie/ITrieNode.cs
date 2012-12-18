using System.Collections.Generic;

namespace KVK.Core.Trie
{
	public interface ITrieNode<T>
	{
		T Value { get; set; } 

		bool HasValue { get; }
		bool IsLeaf { get; }

		ITrieNode<T> this[char c] { get; }
		ITrieNode<T>[] Nodes { get; }

		void SetLeaf();
		int ChildCount { get; }
		bool ShouldOptimize { get; }

		KeyValuePair<char, ITrieNode<T>>[] CharNodePairs();

		ITrieNode<T> AddChild(char c, ref int node_count);

		IEnumerable<T> SubsumedValues();
		IEnumerable<ITrieNode<T>> SubsumedNodes();
		IEnumerable<ITrieNode<T>> SubsumedNodesExceptThis();
		void OptimizeChildNodes();
	}
}