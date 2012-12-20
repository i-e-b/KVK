using System.Collections.Generic;
using KVK.Core.Decomposer;

namespace KVK.Core
{
	public interface IDocumentStore
	{
		void Store(object document);
		IEnumerable<object> FindAll(string path, string value);

		/// <summary> This is a hack to demonstrate the recovery idea </summary>
		IEnumerable<PathValue> RecoverFromIndex_EXAMPLE_HACK(int i);
	}
}