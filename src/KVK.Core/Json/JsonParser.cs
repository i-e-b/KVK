﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KVK.Core.Json
{
    public class JsonParser
    {
        enum Token
        {
            None = -1,           // Used to denote no Lookahead available
            CurlyOpen,
            CurlyClose,
            SquaredOpen,
            SquaredClose,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null
        }

        readonly char[] json;
        readonly StringBuilder s = new StringBuilder();
    	readonly bool ignorecase;
        Token lookAheadToken = Token.None;
        int index;

        public JsonParser(string json, bool ignorecase)
        {
            this.json = json.ToCharArray();
            this.ignorecase = ignorecase;
        }

        public object Decode()
        {
	        var decode = ParseValue();

	        return decode;
        }

	    private Dictionary<string, object> ParseObject()
        {
			var table = new Dictionary<string, object>();

	        ConsumeToken(); // {

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.CurlyClose:
                        ConsumeToken();
                        return table;

					default:
						ReadNamedValue(table);
						break;
				}
            }
        }

	    void ReadNamedValue(IDictionary<string, object> table)
	    {
			// name
		    var name = ParseString();
		    if (ignorecase)
			    name = name.ToLower();

		    // :
		    if (NextToken() != Token.Colon)
		    {
			    throw new Exception("Expected colon at index " + index);
		    }

		    // value
		    var value = ParseValue();

		    table[name] = value;
	    }

	    private IList<object> ParseArray()
        {
            var array = new List<object>();
            ConsumeToken(); // [

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.SquaredClose:
                        ConsumeToken();
                        return array;

                    default:
						array.Add(ParseValue());
                        break;
                }
            }
        }

        private object ParseValue()
        {
            switch (LookAhead())
            {
                case Token.Number:
                    return ParseNumber();

                case Token.String:
                    return ParseString();

                case Token.CurlyOpen:
                    return ParseObject();

                case Token.SquaredOpen:
                    return ParseArray();

                case Token.True:
                    ConsumeToken();
                    return "true";

                case Token.False:
                    ConsumeToken();
                    return "false";

                case Token.Null:
                    ConsumeToken();
                    return null;
            }

            throw new Exception("Unrecognized token at index" + index);
        }

        private string ParseString()
        {
            ConsumeToken(); // "

            s.Length = 0;

            var runIndex = -1;

            while (index < json.Length)
            {
                var c = json[index++];

                if (c == '"')
                {
                    if (runIndex != -1)
                    {
                        if (s.Length == 0)
                            return new string(json, runIndex, index - runIndex - 1);

                        s.Append(json, runIndex, index - runIndex - 1);
                    }
                    return s.ToString();
                }

                if (c != '\\')
                {
                    if (runIndex == -1)
                        runIndex = index - 1;

                    continue;
                }

                if (index == json.Length) break;

                if (runIndex != -1)
                {
                    s.Append(json, runIndex, index - runIndex - 1);
                    runIndex = -1;
                }

                switch (json[index++])
                {
                    case '"':
                        s.Append('"');
                        break;

                    case '\\':
                        s.Append('\\');
                        break;

                    case '/':
                        s.Append('/');
                        break;

                    case 'b':
                        s.Append('\b');
                        break;

                    case 'f':
                        s.Append('\f');
                        break;

                    case 'n':
                        s.Append('\n');
                        break;

                    case 'r':
                        s.Append('\r');
                        break;

                    case 't':
                        s.Append('\t');
                        break;

                    case 'u':
                        {
                            int remainingLength = json.Length - index;
                            if (remainingLength < 4) break;

                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint = ParseUnicode(json[index], json[index + 1], json[index + 2], json[index + 3]);
                            s.Append((char)codePoint);

                            // skip 4 chars
                            index += 4;
                        }
                        break;
                }
            }

            throw new Exception("Unexpectedly reached end of string");
        }

        private static uint ParseSingleChar(char c1, uint multiplier)
        {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9')
                p1 = (uint)(c1 - '0') * multiplier;
            else if (c1 >= 'A' && c1 <= 'F')
                p1 = (uint)((c1 - 'A') + 10) * multiplier;
            else if (c1 >= 'a' && c1 <= 'f')
                p1 = (uint)((c1 - 'a') + 10) * multiplier;
            return p1;
        }

        private static uint ParseUnicode(char c1, char c2, char c3, char c4)
        {
            var p1 = ParseSingleChar(c1, 0x1000);
            var p2 = ParseSingleChar(c2, 0x100);
            var p3 = ParseSingleChar(c3, 0x10);
            var p4 = ParseSingleChar(c4, 1);

            return p1 + p2 + p3 + p4;
        }

        private string ParseNumber()
        {
            ConsumeToken();

            // Need to start back one place because the first digit is also a token and would have been consumed
            var startIndex = index - 1;

            do
            {
                var c = json[index];

                if ((c >= '0' && c <= '9') || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E')
                {
                    if (++index == json.Length) throw new Exception("Unexpected end of string whilst parsing number");
                    continue;
                }

                break;
            } while (true);

            return new string(json, startIndex, index - startIndex);
        }

        private Token LookAhead()
        {
            if (lookAheadToken != Token.None) return lookAheadToken;

            return lookAheadToken = NextTokenCore();
        }

        private void ConsumeToken()
        {
            lookAheadToken = Token.None;
        }

        private Token NextToken()
        {
            var result = lookAheadToken != Token.None ? lookAheadToken : NextTokenCore();

            lookAheadToken = Token.None;

            return result;
        }

        private Token NextTokenCore()
        {
            char c;

            // Skip past whitespace
            do
            {
                c = json[index];

                if (c > ' ') break;
                if (c != ' ' && c != '\t' && c != '\n' && c != '\r') break;

            } while (++index < json.Length);

            if (index == json.Length)
            {
                throw new Exception("Reached end of string unexpectedly");
            }

            c = json[index];

            index++;

            //if (c >= '0' && c <= '9')
            //    return Token.Number;

            switch (c)
            {
                case '{':
                    return Token.CurlyOpen;

                case '}':
                    return Token.CurlyClose;

                case '[':
                    return Token.SquaredOpen;

                case ']':
                    return Token.SquaredClose;

                case ',':
                    return Token.Comma;

                case '"':
                    return Token.String;

				case '0': case '1': case '2': case '3': case '4':
				case '5': case '6': case '7': case '8': case '9':
                case '-': case '+': case '.':
                    return Token.Number;

                case ':':
                    return Token.Colon;

                case 'f':
                    if (json.Length - index >= 4 &&
                        json[index + 0] == 'a' &&
                        json[index + 1] == 'l' &&
                        json[index + 2] == 's' &&
                        json[index + 3] == 'e')
                    {
                        index += 4;
                        return Token.False;
                    }
                    break;

                case 't':
                    if (json.Length - index >= 3 &&
                        json[index + 0] == 'r' &&
                        json[index + 1] == 'u' &&
                        json[index + 2] == 'e')
                    {
                        index += 3;
                        return Token.True;
                    }
                    break;

                case 'n':
                    if (json.Length - index >= 3 &&
                        json[index + 0] == 'u' &&
                        json[index + 1] == 'l' &&
                        json[index + 2] == 'l')
                    {
                        index += 3;
                        return Token.Null;
                    }
                    break;

            }

            throw new Exception("Could not find token at index " + --index);
        }
    }
}
