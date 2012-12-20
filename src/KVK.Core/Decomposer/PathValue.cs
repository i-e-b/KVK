using System;

namespace KVK.Core.Decomposer
{
	public class PathValue: IEquatable<PathValue>
	{
		public string Path { get; set; }
		public string Value { get; set; }

		public PathValue(string path, string value)
		{
			Path = path;
			Value = value;
		}

		public bool Equals(PathValue other)
		{
			return Path == other.Path && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Path.GetHashCode();
		}

		public override string ToString()
		{
			return "\""+Path+"\" : \""+Value+"\"";
		}
	}
}
