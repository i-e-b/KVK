using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.Text;

namespace KVK.Core.Decomposer
{
	public class ObjectDecomposer:IDecomposer
	{
		IEnumerable<Tuple<string, string>> Decompose(object o, string root = "")
		{
			var props = o.GetType().GetProperties();
			foreach (var property in props)
			{
				if (!(property.CanRead)) continue;

				var value = property.GetValue(o);
				var isSerialisable = property.PropertyType.Attributes.HasFlag(TypeAttributes.Serializable);

				if (isSerialisable)
				{
					if (value is IEnumerable && !(value is string) && !(value.GetType().IsValueType) )
					{
						foreach (var subvalue in ((IEnumerable)value))
						{
							yield return new Tuple<string, string>(root + property.Name, TypeSerializer.SerializeToString(subvalue));
						}
					}
					else
					{
						yield return new Tuple<string, string>(root + property.Name, TypeSerializer.SerializeToString(value));
					}
				}
				else
				{
					var others = Decompose(property.GetValue(o), root + property.Name + ".");
					foreach (var other in others) { yield return other; }
				}
			}
		}

		public IEnumerable<Tuple<string, string>> Decompose(object obj)
		{
			return Decompose(obj, "");
		}
	}
}