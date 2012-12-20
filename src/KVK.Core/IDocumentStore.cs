using System.Collections.Generic;

namespace KVK.Core
{
	public interface IDocumentStore
	{
		void Store(object document);
		IEnumerable<object> FindAll(string path, string value);
	}
}