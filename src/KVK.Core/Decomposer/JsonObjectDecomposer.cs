using System;
using System.Collections;
using System.Collections.Generic;

namespace KVK.Core.Decomposer
{
	public class JsonObjectDecomposer:IDecomposer
	{
		static IEnumerable<PathValue> Decompose(object o, string root)
		{
			var nroot = (root == "") ? ("") : (root + ".");
			if (o is IDictionary)
			{
				var dic = o as IDictionary;
				foreach (var key in dic.Keys)
				{
					var val = dic[key];
					var deeper = Decompose(val, nroot + key);
					foreach (var pv in deeper) yield return pv;
				}
			}
			else if (o is IList)
			{
				var list = o as IList;
				foreach (var item in list)
				{
					var deeper = Decompose(item, root);
					foreach (var pv in deeper) yield return pv;
				}
			}
			else if (o is string)
			{
				yield return new PathValue(root, o.ToString());
			}
			else
			{
				throw new Exception("Not type supported in Json decomposer: "+o.GetType().Name);
			}
		}

		public IEnumerable<PathValue> Decompose(object obj)
		{
			return Decompose(obj, "");
		}
	}
}