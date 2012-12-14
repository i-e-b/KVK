using System;
using System.Linq;
using KVK.Core;

class Program 
{ 
    // trie with payload of type <String> 
    static readonly Trie<String> value_trie = new Trie<String> 
    { 
        { "rabbit", "cute" }, 
        { "giraffe", "tall" }, 
        { "ape", "smart" }, 
        { "hippo", "large" }, 
    }; 
 
    // degenerate case of a trie without payload 
    static readonly Trie<bool> simple_trie = new Trie<bool> 
    { 
        { "rabbit", true }, 
        { "giraffe", true }, 
        { "ape", true }, 
        { "hippo", true }, 
    }; 
 
    static void Main(String[] args) 
    { 
        String s = "Once upon a time, a rabbit met an ape in the woods."; 
 
        // Retrieve payloads for words in the string. 
        // 
        // output: 
        //      cute 
        //      smart 
        foreach (String word in value_trie.AllSubstringValues(s)) 
                Console.WriteLine(word); 
 
        // Simply test a string for any of the words in the trie. 
        // Note that the Any() operator ensures that the input is no longer 
        // traversed once a single result is found. 
        // 
        // output: 
        //      True 
        Console.WriteLine(simple_trie.AllSubstringValues(s).Any(e=>e)); 
 
        s = "Four score and seven years ago."; 
        // output: 
        //      False 
        Console.WriteLine(simple_trie.AllSubstringValues(s).Any(e => e)); 

		Console.ReadKey();
    } 
} 