using System.Collections.Generic;
using System.Linq;
using KVK.Core.Decomposer;
using KVK.Core.PathValueKey;

namespace KVK.Core
{
	public class DocumentStore : IDocumentStore
	{
		/*
		 * This simple version stores the originals as
		 * entire object documents.
		 * 
		 * The plan is to be able to reconstruct the 
		 * original document from the indexes themselves,
		 * allowing documents to be sharded and reconstruction
		 * to be load balanced.
		 * 
		 * This will require much more work on the serialisation!
		 * 
		 */

		readonly IPathValueKeyStore<int> index;
		readonly IList<object> originals;
		readonly IDecomposer decomposer;

		public DocumentStore()
		{
			decomposer = new ObjectDecomposer();
			index = new PathValueKeyStore<int>();
			originals = new List<object>();
		}

		public void Store(object document)
		{
			var key = AddAndGetKey(document);
			IndexDocumentWithKey(document, key);
		}

		public IEnumerable<object> FindAll(string path, string value)
		{
			return index
				.List(path, value)
				.Select(result => originals[result]);
		}

		void IndexDocumentWithKey(object document, int idx)
		{
			var pathValues = decomposer.Decompose(document);

			foreach (var pathValue in pathValues)
			{
				index.Add(pathValue.Path, pathValue.Value, idx);
			}
		}

		int AddAndGetKey(object document)
		{
			int idx;
			lock (originals)
			{
				originals.Add(document);
				idx = originals.Count - 1;
			}
			return idx;
		}

	}
}