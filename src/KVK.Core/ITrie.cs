using System.Collections.Generic;

namespace KVK.Core
{
	public interface ITrie<T> 
	{
		ITrieNode<T> Add(string key, T value);
		T Find(string key);
		ITrieNode<T> FindNode(string key);
		IEnumerable<T> FindAll(string key);
	}
}