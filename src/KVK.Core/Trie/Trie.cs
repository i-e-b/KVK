using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable CompareNonConstrainedGenericWithNull
// Originally by http://www.glennslayden.com/code/c-sharp/trie

namespace KVK.Core.Trie
{
	public class Trie<TValue> : IEnumerable<ITrieNode<TValue>>, ITrie<TValue>
	{
		abstract class TrieNodeBase:ITrieNode<TValue>
		{
			public TValue Value { get; set; }

			public bool HasValue { get { return !Equals(Value, default(TValue)); } }
			public abstract bool IsLeaf { get; }
			public abstract ITrieNode<TValue> this[char c] { get; }
			public abstract ITrieNode<TValue>[] Nodes { get; }
			public abstract void SetLeaf();
			public abstract int ChildCount { get; }
			public abstract bool ShouldOptimize { get; }
			public abstract KeyValuePair<char, ITrieNode<TValue>>[] CharNodePairs();
			public abstract ITrieNode<TValue> AddChild(char c, ref int node_count);

			/// <summary>
			/// Includes current node value
			/// </summary>
			public IEnumerable<TValue> SubsumedValues()
			{
				if (Value != null)
					yield return Value;
				if (Nodes != null)
					foreach (var child in Nodes)
						if (child != null)
							foreach (var t in child.SubsumedValues())
								yield return t;
			}

			/// <summary>
			/// Includes current node
			/// </summary>
			public IEnumerable<ITrieNode<TValue>> SubsumedNodes()
			{
				yield return this;
				if (Nodes != null)
					foreach (TrieNodeBase child in Nodes)
						if (child != null)
							foreach (TrieNodeBase n in child.SubsumedNodes())
								yield return n;
			}

			/// <summary>
			/// Doesn't include current node
			/// </summary>
			public IEnumerable<ITrieNode<TValue>> SubsumedNodesExceptThis()
			{
				if (Nodes != null)
					foreach (TrieNodeBase child in Nodes)
						if (child != null)
							foreach (TrieNodeBase n in child.SubsumedNodes())
								yield return n;
			}

			/// <summary>
			/// Note: doesn't de-optimize optimized nodes if re-run later
			/// </summary>
			public void OptimizeChildNodes()
			{
				if (Nodes != null)
					foreach (var keyValuePair in CharNodePairs())
					{
						var node = keyValuePair.Value;
						if (node.ShouldOptimize)
						{
							var newNode = new SparseTrieNode(node.CharNodePairs()) {Value = node.Value};
							ReplaceChild(keyValuePair.Key, newNode);
						}
						node.OptimizeChildNodes();
					}
			}

			public abstract void ReplaceChild(Char c, TrieNodeBase n);

		};

		class SparseTrieNode : TrieNodeBase
		{
			Dictionary<Char, ITrieNode<TValue>> d;

			public SparseTrieNode(IEnumerable<KeyValuePair<Char, ITrieNode<TValue>>> ie)
			{
				d = new Dictionary<char, ITrieNode<TValue>>();
				foreach (var kvp in ie)
					d.Add(kvp.Key, kvp.Value);
			}

			public override ITrieNode<TValue> this[Char c]
			{
				get
				{
					ITrieNode<TValue> node;
					return d.TryGetValue(c, out node) ? node : null;
				}
			}

			public override ITrieNode<TValue>[] Nodes { get { return d.Values.ToArray(); } }

			/// <summary>
			/// do not use in current form. This means, run OptimizeSparseNodes *after* any pruning
			/// </summary>
			public override void SetLeaf() { d = null; }

			public override int ChildCount { get { return d.Count; } }

			public override KeyValuePair<Char, ITrieNode<TValue>>[] CharNodePairs()
			{
				return d.ToArray();
			}

			public override ITrieNode<TValue> AddChild(char c, ref int node_count)
			{
				ITrieNode<TValue> node;
				if (!d.TryGetValue(c, out node))
				{
					node = new TrieNode();
					node_count++;
					d.Add(c, node);
				}
				return node;
			}

			public override void ReplaceChild(Char c, TrieNodeBase n)
			{
				d[c] = n;
			}

			public override bool ShouldOptimize { get { return false; } }
			public override bool IsLeaf { get { return d == null; } }

		};

		class TrieNode : TrieNodeBase
		{
			private ITrieNode<TValue>[] nodes;
			private Char m_base;

			public override int ChildCount { get { return (nodes != null) ? nodes.Count(e => e != null) : 0; } }

			public override ITrieNode<TValue>[] Nodes { get { return nodes; } }

			public override void SetLeaf() { nodes = null; }

			public override KeyValuePair<Char, ITrieNode<TValue>>[] CharNodePairs()
			{
				var rg = new KeyValuePair<char, ITrieNode<TValue>>[ChildCount];
				Char ch = m_base;
				int i = 0;
				foreach (TrieNodeBase child in nodes)
				{
					if (child != null)
						rg[i++] = new KeyValuePair<char, ITrieNode<TValue>>(ch, child);
					ch++;
				}
				return rg;
			}

			public override ITrieNode<TValue> this[char c]
			{
				get
				{
					if (nodes != null && m_base <= c && c < m_base + nodes.Length)
						return nodes[c - m_base];
					return null;
				}
			}

			public override ITrieNode<TValue> AddChild(char c, ref int node_count)
			{
				if (nodes == null)
				{
					m_base = c;
					nodes = new ITrieNode<TValue>[1];
				}
				else if (c >= m_base + nodes.Length)
				{
					Array.Resize(ref nodes, c - m_base + 1);
				}
				else if (c < m_base)
				{
					var c_new = (Char)(m_base - c);
					var tmp = new ITrieNode<TValue>[nodes.Length + c_new];
					nodes.CopyTo(tmp, c_new);
					m_base = c;
					nodes = tmp;
				}

				var node = nodes[c - m_base];
				if (node == null)
				{
					node = new TrieNode();
					node_count++;
					nodes[c - m_base] = node;
				}
				return node;
			}

			public override void ReplaceChild(Char c, TrieNodeBase n)
			{
				if (nodes == null || c >= m_base + nodes.Length || c < m_base)
					throw new Exception();
				nodes[c - m_base] = n;
			}

			public override bool ShouldOptimize
			{
				get
				{
					if (nodes == null)
						return false;
					return (ChildCount * 9 < nodes.Length);		// empirically determined optimal value (space & time)
				}
			}

			public override bool IsLeaf { get { return nodes == null; } }
		};

		ITrieNode<TValue> thisNode = new TrieNode();
		int nodeCount;

		// in combination with Add(...), enables C# 3.0 initialization syntax, even though it never seems to call it
		public IEnumerator GetEnumerator()
		{
			return thisNode.SubsumedNodes().GetEnumerator();
		}

		IEnumerator<ITrieNode<TValue>> IEnumerable<ITrieNode<TValue>>.GetEnumerator()
		{
			return thisNode.SubsumedNodes().GetEnumerator();
		}

		public IEnumerable<TValue> Values { get { return thisNode.SubsumedValues(); } }

		public void Compact()
		{
			if (thisNode.ShouldOptimize)
			{
				thisNode = new SparseTrieNode(thisNode.CharNodePairs());
			}
			thisNode.OptimizeChildNodes();
		}

		public ITrieNode<TValue> Add(string s, TValue v)
		{
			var node = thisNode;
			foreach (Char c in s)
				node = node.AddChild(c,ref nodeCount);

			node.Value = v;
			return node;
		}

		public bool Contains(String s)
		{
			var node = thisNode;
			foreach (var c in s)
			{
				node = node[c];
				if (node == null)
					return false;
			}
			return node.HasValue;
		}

		/// <summary>
		/// Debug only; this is hideously inefficient
		/// </summary>
		public string GetKey(ITrieNode<TValue> seek)
		{
			var sofar = new StringBuilder();

			Func<ITrieNode<TValue>, bool> fn = null;
			fn = cur =>
			{
				foreach (var kvp in cur.CharNodePairs())
				{
					if (kvp.Value == seek)
					{
						sofar.Insert(0, kvp.Key);
						return true;
					}
					if (kvp.Value.Nodes == null || !fn(kvp.Value)) continue;

					sofar.Insert(0, kvp.Key);
					return true;
				}
				return false;
			};

			return fn(thisNode) ? sofar.ToString() : null;
		}

		/// <summary>
		/// Debug only; this is hideously inefficient
		/// </summary>
		public string GetKeyByValue(TValue seek)
		{
			var sofar = new StringBuilder();

			Func<ITrieNode<TValue>, bool> fn = null;
			fn = cur =>
			{
				foreach (var kvp in cur.CharNodePairs())
				{
					if (kvp.Value.Value != null && kvp.Value.Value.Equals(seek))
					{
						sofar.Insert(0, kvp.Key);
						return true;
					}
					if (kvp.Value.Nodes == null || !fn(kvp.Value)) continue;

					sofar.Insert(0, kvp.Key);
					return true;
				}
				return false;
			};

			return fn(thisNode) ? sofar.ToString() : null;
		}

		public ITrieNode<TValue> FindNode(String s_in)
		{
			var node = thisNode;
			return s_in.Any(c => (node = node[c]) == null) ? null : node;
		}

		public ITrieNode<TValue> FindNodeOrLast(String key, out bool wasExactMatch)
		{
			var node = thisNode;
			foreach (var c in key)
			{
				if (node.IsLeaf)
				{
					wasExactMatch = false;
					return node;
				}
				if ((node = node[c]) != null) continue;
				wasExactMatch = false;
				return null;
			}
			wasExactMatch = true;
			return node;
		}

		public TValue Find(string targetKey)
		{
			return UnsafeFind(targetKey);
		}

		// TODO : write in safe code.
		unsafe TValue UnsafeFind(String s_in)
		{
			var node = thisNode;
			fixed (Char* pin_s = s_in)
			{
				Char* p = pin_s;
				Char* p_end = p + s_in.Length;
				while (p < p_end)
				{
					if ((node = node[*p]) == null)
						return default(TValue);
					p++;
				}
				return node.Value;
			}
		}

		public IEnumerable<TValue> FindAll(String s_in)
		{
			var node = thisNode;
			foreach (var c in s_in)
			{
				if ((node = node[c]) == null)
					break;
				if (node.Value != null)
					yield return node.Value;
			}
		}

		public IEnumerable<TValue> AllSubstringValues(String s)
		{
			int i_cur = 0;
			while (i_cur < s.Length)
			{
				var node = thisNode;
				int i = i_cur;
				while (i < s.Length)
				{
					node = node[s[i]];
					if (node == null)
						break;
					if (node.Value != null)
						yield return node.Value;
					i++;
				}
				i_cur++;
			}
		}

		void DepthFirstTraverse(Action<String,ITrieNode<TValue>> callback)
		{
			var rgch = new Char[100];
			int depth = 0;

			Action<ITrieNode<TValue>> fn = null;
			fn = cur =>
			{
				if (depth >= rgch.Length)
				{
					var tmp = new Char[rgch.Length * 2];
					Buffer.BlockCopy(rgch, 0, tmp, 0, rgch.Length * sizeof(Char));
					rgch = tmp;
				}
				foreach (var kvp in cur.CharNodePairs())
				{
					rgch[depth] = kvp.Key;
					var n = kvp.Value;
					if (n.Nodes != null)
					{
						depth++;
						fn(n);
						depth--;
					}
					else if (n.Value == null)		// leaf nodes should always have a value
						throw new Exception();

					if (n.Value != null)
						callback(new String(rgch, 0, depth+1), n);
				}
			};

			fn(thisNode);
		}

		public ITrie<TNew> ToTrie<TNew>(Func<TValue, TNew> valueConverter)
		{
			var t = new Trie<TNew>();
			DepthFirstTraverse((s,n)=> t.Add(s,valueConverter(n.Value)));
			return t;
		}
	};
}