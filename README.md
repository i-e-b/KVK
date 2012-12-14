KVK
===

Experiments in relational document datastores. Key-Value-Key: Reinventing the wheel

Schema-free data store as a set of Key-Value stores
===================================================

Documents are composed of a hierarchy of keys and values. Each document has a single unique key.
Each key in the document can be expressed as a key path -- so in the document:

```
id1 = {
	Group : {
		Item : {
			Upc : "29875897587365"
		}
	}
}
```

The `Upc` key has a key path of `Group.Item.Upc`.

Principles
----------

* Every key path is a separate store
* Each __key__ in each store is the _value_ in the document
* Each __value__ in each store is a list of documents that have the key-path and value

So for document:
```
x = {
  Uri : "http://example.com",
  Owner : {
    Name : "me"
  }
}
```

* "Uri" --> ["http://example.com" : [x, ...]]
* "Owner.Name" --> ["me" : [x, ...]]

With pattern "store name" --> ["value" : [list of containing documents]]

The keys in each store could be Tries where each node has a (potentially empty) list of matching documents.

Notes
-----
* For scaling, can list hosts rather than documents, then go to host for definitive answer.
* Key-value existence checking v. easy.