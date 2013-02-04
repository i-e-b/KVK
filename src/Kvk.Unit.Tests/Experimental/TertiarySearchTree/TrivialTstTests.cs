using System;
using System.Threading;
using NUnit.Framework;

namespace Kvk.Unit.Tests.TertiarySearchTree
{
	[TestFixture]
	public class TrivialTstTests
	{
		[Test]
		public void quick_test()
		{
			var root = new TstNode('h');
			root.Insert("hello");
			root.Insert("world");
			root.Insert("mind");
			root.Insert("gap");

			Assert.That(root.Search("hello"), Is.Not.Null);
			Assert.That(root.Search("beans"), Is.Null);
		}
	}

	public class TstNode
	{
		readonly char _ch;
		volatile bool _terminated;
		TstNode _left;
		TstNode _right;
		TstNode _center;

		public TstNode(char ch)
		{
			_ch = ch;
		}

		public bool IsTerminated()
		{
			return _terminated;
		}

		public TstNode Search(String word)
		{
			TstNode node = this;
			int i = 0;
			while (null != node)
			{
				char ch = word[i];
				if (ch < node._ch)
					node = node._left;
				else if (ch == node._ch)
				{
					i++;
					if (word.Length == i)
					{
						return (node._terminated) ? node : null;
					}
					node = node._center;
				}
				else
					node = node._right;
			}
			//not found
			return null;
		}

		public TstNode Insert(String word)
		{
			int i = 0;
			TstNode node = this;
			while (i < word.Length)
			{
				char ch = word[i];
				if (ch < node._ch)
				{
					node = insertLeftNode(ch, node);
				}
				else if (ch > node._ch)
				{
					node = insertRightNode(ch, node);
				}
				else
				{
					++i;
					if (i == word.Length)
					{
						node._terminated = true;
						break;
					}
					node = insertCenterNode(word, i, node);
				}
			}
			return node;
		}

		private TstNode insertCenterNode(String word, int i, TstNode node)
		{
			Interlocked.CompareExchange(ref node._center, new TstNode(word[i]), null);
			node = node._center;
			return node;
		}

		private TstNode insertRightNode(char ch, TstNode node)
		{
			Interlocked.CompareExchange(ref node._right, new TstNode(ch), null);
			node = node._right;
			return node;
		}

		private TstNode insertLeftNode(char ch, TstNode node)
		{
			Interlocked.CompareExchange(ref node._left, new TstNode(ch), null);
			node = node._left;
			return node;
		}
	}


}
