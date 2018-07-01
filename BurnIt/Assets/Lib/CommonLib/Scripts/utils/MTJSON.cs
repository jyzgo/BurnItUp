/*
 * Copyright (c) 2012 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 * 
 * 减少反序列化时的内存占用。
 * 增加对行注释（以 “/” 或者 “#” 开头）的支持。
 * 增加对“.”开头的浮点数的支持
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace MTUnity.Utils
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Text;

	// Example usage:
	//
	//  using UnityEngine;
	//  using System.Collections;
	//  using System.Collections.Generic;
	//  using MTUnity.Utils;
	//
	//  public class MTJSONTest : MonoBehaviour {
	//      void Start () {
	//          var jsonString = "{ \"array\": [1.44,2,3], " +
	//                          "\"object\": {\"key1\":\"value1\", \"key2\":256}, " +
	//                          "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
	//                          "\"unicode\": \"\\u3041 Men\u00fa sesi\u00f3n\", " +
	//                          "\"int\": 65536, " +
	//                          "\"float\": 3.1415926, " +
	//                          "\"bool\": true, " +
	//                          "\"null\": null }";
	//
	//          var dict = MTJSON.Deserialize(jsonString) as Dictionary<string,object>;
	//
	//          Debug.Log("deserialized: " + dict.GetType());
	//          Debug.Log("dict['array'][0]: " + ((List<object>) dict["array"])[0]);
	//          Debug.Log("dict['string']: " + (string) dict["string"]);
	//          Debug.Log("dict['float']: " + (double) dict["float"]); // floats come out as doubles
	//          Debug.Log("dict['int']: " + (long) dict["int"]); // ints come out as longs
	//          Debug.Log("dict['unicode']: " + (string) dict["unicode"]);
	//
	//          var str = MTJSON.Serialize(dict);
	//
	//          Debug.Log("serialized: " + str);
	//      }
	//  }

	/// <summary>
	/// This class encodes and decodes JSON strings.
	/// Spec. details, see http://www.json.org/
	///
	/// JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
	/// All numbers are parsed to doubles.
	/// </summary>
	public static class MTJSON
	{
		// interpret all numbers as if they are english US formatted numbers
		private static NumberFormatInfo numberFormat = (new CultureInfo ("en-US")).NumberFormat;

		/// <summary>
		/// Parses the string json into a value
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
		public static MTJSONObject Deserialize (string json)
		{
			// save the string for debug information
			if (json == null) {
				return null;
			}

			return Parser.Parse (json);
		}

		/// <summary>
		/// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
		/// </summary>
		/// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
		/// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
		public static string Serialize (object obj)
		{
			return Serializer.Serialize (obj);
		}

		private sealed class Parser : IDisposable
		{
			private const string WhiteSpace = " \t\n\r";
			private const string WordBreak = " \t\n\r{}[],:\"";
			private const string LineBreak = "\n\r";
			private StringReader json;
			private StringBuilder strBuff;
			private StringBuilder strBuff2;

			private Parser (string jsonString)
			{
				this.json = new StringReader (jsonString);
				strBuff = new StringBuilder ();
				strBuff2 = new StringBuilder ();
			}

			private enum TOKEN
			{
				NONE,
				COMMENTS,
				CURLY_OPEN,
				CURLY_CLOSE,
				SQUARED_OPEN,
				SQUARED_CLOSE,
				COLON,
				COMMA,
				STRING,
				NUMBER,
				TRUE,
				FALSE,
				NULL
			}

			private char PeekChar {
				get {
					return Convert.ToChar (this.json.Peek ());
				}
			}

			private char NextChar {
				get {
					return Convert.ToChar (this.json.Read ());
				}
			}

			private string NextWord {
				get {
					strBuff.Length = 0;

					while (WordBreak.IndexOf(this.PeekChar) == -1) {
						strBuff.Append (this.NextChar);

						if (this.json.Peek () == -1) {
							break;
						}
					}

					return strBuff.ToString ();
				}
			}

			private TOKEN NextToken {
				get {
					this.EatWhitespace ();

					if (this.json.Peek () == -1) {
						return TOKEN.NONE;
					}

					char c = this.PeekChar;
					switch (c) {
					case '/':
					case '#':
						return TOKEN.COMMENTS;
					case '{':
						return TOKEN.CURLY_OPEN;
					case '}':
						this.json.Read ();
						return TOKEN.CURLY_CLOSE;
					case '[':
						return TOKEN.SQUARED_OPEN;
					case ']':
						this.json.Read ();
						return TOKEN.SQUARED_CLOSE;
					case ',':
						this.json.Read ();
						return TOKEN.COMMA;
					case '"':
						return TOKEN.STRING;
					case ':':
						return TOKEN.COLON;
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '-':
					case '.':
						return TOKEN.NUMBER;
					}

					string word = this.NextWord;

					switch (word) {
					case "false":
						return TOKEN.FALSE;
					case "true":
						return TOKEN.TRUE;
					case "null":
						return TOKEN.NULL;
					}

					return TOKEN.NONE;
				}
			}

			public static MTJSONObject Parse (string jsonString)
			{
				using (var instance = new Parser(jsonString)) {
					return instance.ParseValue ();
				}
			}

			public void Dispose ()
			{
				this.json.Dispose ();
				this.json = null;
				this.strBuff = null;
				this.strBuff2 = null;
			}

			private MTJSONObject ParseObject ()
			{
				Dictionary<string, MTJSONObject> table = new Dictionary<string, MTJSONObject> ();

				// ditch opening brace
				this.json.Read ();

				// {
				while (true) {
					switch (this.NextToken) {
					case TOKEN.NONE:
						return null;
					case TOKEN.COMMENTS:
						EatComments ();
						continue;
					case TOKEN.COMMA:
						continue;
					case TOKEN.CURLY_CLOSE:
						return new MTJSONObject (table);
					default:
                        // name
						string name = (string)this.ParseString (false);
						if (name == null) {
							return null;
						}

                        // :
						if (this.NextToken != TOKEN.COLON) {
							return null;
						}

                        // ditch the colon
						this.json.Read ();

                        // value
						table [name] = this.ParseValue ();
						break;
					}
				}
			}

			private MTJSONObject ParseArray ()
			{
				List<MTJSONObject> array = new List<MTJSONObject> ();

				// ditch opening bracket
				this.json.Read ();

				// [
				var parsing = true;
				while (parsing) {
					TOKEN nextToken = this.NextToken;

					switch (nextToken) {
					case TOKEN.NONE:
						return null;
					case TOKEN.COMMENTS:
						EatComments ();
						continue;
					case TOKEN.COMMA:
						continue;
					case TOKEN.SQUARED_CLOSE:
						parsing = false;
						break;
					default:
						MTJSONObject value = this.ParseByToken (nextToken);

						array.Add (value);
						break;
					}
				}

				return new MTJSONObject (array);
			}

			private MTJSONObject ParseValue ()
			{
				TOKEN nextToken = this.NextToken;
				return this.ParseByToken (nextToken);
			}

			private MTJSONObject ParseByToken (TOKEN token)
			{
				switch (token) {
				case TOKEN.STRING:
					return (MTJSONObject)this.ParseString (true);
				case TOKEN.NUMBER:
					return this.ParseNumber ();
				case TOKEN.CURLY_OPEN:
					return this.ParseObject ();
				case TOKEN.SQUARED_OPEN:
					return this.ParseArray ();
				case TOKEN.TRUE:
					return new MTJSONObject (true);
				case TOKEN.FALSE:
					return new MTJSONObject (false);
				case TOKEN.NULL:
					return null;
				default:
					return null;
				}
			}

			private object ParseString (bool isValue = true)
			{
				strBuff.Length = 0;
				char c;

				// ditch opening quote
				this.json.Read ();

				bool parsing = true;
				while (parsing) {
					if (this.json.Peek () == -1) {
						parsing = false;
						break;
					}

					c = this.NextChar;
					switch (c) {
					case '"':
						parsing = false;
						break;
					case '\\':
						if (this.json.Peek () == -1) {
							parsing = false;
							break;
						}

						c = this.NextChar;
						switch (c) {
						case '"':
						case '\\':
						case '/':
							strBuff.Append (c);
							break;
						case 'b':
							strBuff.Append ('\b');
							break;
						case 'f':
							strBuff.Append ('\f');
							break;
						case 'n':
							strBuff.Append ('\n');
							break;
						case 'r':
							strBuff.Append ('\r');
							break;
						case 't':
							strBuff.Append ('\t');
							break;
						case 'u':
							strBuff2.Length = 0;

							for (int i = 0; i < 4; i++) {
								strBuff2.Append (this.NextChar);
							}

							strBuff.Append ((char)Convert.ToInt32 (strBuff2.ToString (), 16));
							break;
						}

						break;
					default:
						strBuff.Append (c);
						break;
					}
				}

				if (isValue) {
					return new MTJSONObject (strBuff.ToString ());
				}
				return strBuff.ToString ();
			}

			private MTJSONObject ParseNumber ()
			{
				string number = this.NextWord;

				if (number.IndexOf ('.') == -1) {
					return new MTJSONObject (long.Parse (number, numberFormat));
				}

				return new MTJSONObject (double.Parse (number, numberFormat));
			}

			private void EatWhitespace ()
			{
				while (WhiteSpace.IndexOf(this.PeekChar) != -1) {
					this.json.Read ();

					if (this.json.Peek () == -1) {
						break;
					}
				}
			}

			private void EatComments ()
			{
				while (LineBreak.IndexOf(this.PeekChar) == -1) {
					this.json.Read ();
					
					if (this.json.Peek () == -1) {
						break;
					}
				}
			}
		}

		private sealed class Serializer
		{
			private StringBuilder builder;

			private Serializer ()
			{
				this.builder = new StringBuilder ();
			}

			public static string Serialize (object obj)
			{
				var instance = new Serializer ();

				instance.SerializeValue (obj);

				return instance.builder.ToString ();
			}

			private void SerializeValue (object value)
			{
				MTJSONObject asMTJSONObject;
				IList asList;
				IDictionary asDict;
				string asStr;

				asMTJSONObject = value as MTJSONObject;
				if (asMTJSONObject != null) {
					value = asMTJSONObject.o;
				}
				if (value == null) {
					this.builder.Append ("null");
				} else if ((asStr = value as string) != null) {
					this.SerializeString (asStr);
				} else if (value is bool) {
					this.builder.Append (value.ToString ().ToLower ());
				} else if ((asList = value as IList) != null) {
					this.SerializeArray (asList);
				} else if ((asDict = value as IDictionary) != null) {
					this.SerializeObject (asDict);
				} else if (value is char) {
					this.SerializeString (value.ToString ());
				} else {
					this.SerializeOther (value);
				}
			}

			private void SerializeObject (IDictionary obj)
			{
				bool first = true;

				this.builder.Append ('{');

				foreach (object e in obj.Keys) {
					if (!first) {
						this.builder.Append (',');
					}

					this.SerializeString (e.ToString ());
					this.builder.Append (':');

					this.SerializeValue (obj [e]);

					first = false;
				}

				this.builder.Append ('}');
			}

			private void SerializeArray (IList array)
			{
				this.builder.Append ('[');

				bool first = true;

				foreach (object obj in array) {
					if (!first) {
						this.builder.Append (',');
					}

					this.SerializeValue (obj);

					first = false;
				}

				this.builder.Append (']');
			}

			private void SerializeString (string str)
			{
				this.builder.Append ('\"');

				char[] charArray = str.ToCharArray ();
				foreach (var c in charArray) {
					switch (c) {
					case '"':
						this.builder.Append ("\\\"");
						break;
					case '\\':
						this.builder.Append ("\\\\");
						break;
					case '\b':
						this.builder.Append ("\\b");
						break;
					case '\f':
						this.builder.Append ("\\f");
						break;
					case '\n':
						this.builder.Append ("\\n");
						break;
					case '\r':
						this.builder.Append ("\\r");
						break;
					case '\t':
						this.builder.Append ("\\t");
						break;
					default:
						int codepoint = Convert.ToInt32 (c);
						if ((codepoint >= 32) && (codepoint <= 126)) {
							this.builder.Append (c);
						} else {
							this.builder.Append ("\\u" + Convert.ToString (codepoint, 16).PadLeft (4, '0'));
						}

						break;
					}
				}

				this.builder.Append ('\"');
			}

			private void SerializeOther (object value)
			{
				if (value is float
					|| value is int
					|| value is uint
					|| value is long
					|| value is double
					|| value is sbyte
					|| value is byte
					|| value is short
					|| value is ushort
					|| value is ulong
					|| value is decimal) {
					this.builder.Append (value.ToString ());
				} else {
					this.SerializeString (value.ToString ());
				}
			}
		}
	}

	/// <summary>
	/// MTJSON object.
	/// </summary>
	public class MTJSONObject
	{
		object _o;

		public object o {
			get { return _o; }
		}
		
		public bool isNull {
			get { return _o == null; }
		}
		
		public bool isDict {
			get { return _o is IDictionary; }
		}
		
		public bool isList {
			get { return _o is IList; }
		}
		
		public Dictionary<string, MTJSONObject> dict {
			get { return _o as Dictionary<string, MTJSONObject>; }
		}
		
		public List<MTJSONObject> list {
			get { return _o as List<MTJSONObject>; }
		}

		public int count {
			get {
				IDictionary asDict = _o as IDictionary;
				if (asDict != null) {
					return asDict.Count;
				}
				IList asList = _o as IList;
				if (asList != null) {
					return asList.Count;
				}
				return -1;
			}
		}
		
		public string s {
			get { return _o.ToString (); }
		}
		
		public bool b {
			get { return Convert.ToBoolean (_o); }
		}
		
		public long l {
			get { return Convert.ToInt64 (_o); }
		}
		
		public int i {
			get { return Convert.ToInt32 (_o); }
		}
		
		public int si {
			get { return Convert.ToInt16 (_o); }
		}
		
		public double d {
			get { return Convert.ToDouble (_o); }
		}
		
		public float f {
			get { return Convert.ToSingle (_o); }
		}

		public MTJSONObject this [int index] {
			get { return Get (index); }
			set { Set (index, value); }
		}
		
		public MTJSONObject this [string key] {
			get { return Get (key); }
			set { Set (key, value); }
		}

		public MTJSONObject (object o)
		{
			_o = o;
		}

		public static MTJSONObject CreateDict ()
		{
			return new MTJSONObject (new Dictionary<string, MTJSONObject> ());
		}

		public static MTJSONObject CreateList ()
		{
			return new MTJSONObject (new List<MTJSONObject> ());
		}

		public MTJSONObject Get (string key)
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			MTJSONObject value;
			asDict.TryGetValue (key, out value);
			return value;
		}

		public MTJSONObject Get (int index)
		{
			List<MTJSONObject> asList = list;
			if (index >= 0 && index < asList.Count) {
				return asList [index];
			}
			return null;
		}

		public string GetString (string key, string def = "")
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			MTJSONObject value;
			asDict.TryGetValue (key, out value);
			if (value != null) {
				return value.s;
			}
			return def;
		}

		public float GetFloat (string key, float def = 0)
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			MTJSONObject value;
			asDict.TryGetValue (key, out value);
			if (value != null) {
				return value.f;
			}
			return def;
        }

        public double GetDouble (string key, double def = 0)
        {
            Dictionary<string, MTJSONObject> asDict = dict;
            MTJSONObject value;
            asDict.TryGetValue (key, out value);
            if (value != null) {
                return value.d;
            }
            return def;
        }

		public int GetInt (string key, int def = 0)
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			MTJSONObject value;
			asDict.TryGetValue (key, out value);
			if (value != null) {
				return value.i;
			}
			return def;
        }

        public long GetLong (string key, long def = 0)
        {
            Dictionary<string, MTJSONObject> asDict = dict;
            MTJSONObject value;
            asDict.TryGetValue (key, out value);
            if (value != null) {
                return value.l;
            }
            return def;
        }

		public bool GetBool (string key, bool def = false)
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			MTJSONObject value;
			asDict.TryGetValue (key, out value);
			if (value != null) {
				return value.b;
			}
			return def;
		}

		public void Set (string key, object value)
		{
			Add (key, value);
		}
		
		public void Set (int index, object item)
		{
			List<MTJSONObject> asList = list;
			if (index >= 0) {
				while (index >= asList.Count) {
					asList.Add (null);
				}
				MTJSONObject asMTJSONObject = null;
				if (item != null) {
					asMTJSONObject = item as MTJSONObject;
					if (asMTJSONObject == null) {
						asMTJSONObject = new MTJSONObject (item);
					}
				}
				asList [index] = asMTJSONObject;
			}
		}

		public void Add (string key, object value)
		{
			MTJSONObject asMTJSONObject = null;
			if (value != null) {
				asMTJSONObject = value as MTJSONObject;
				if (asMTJSONObject == null) {
					asMTJSONObject = new MTJSONObject (value);
				}
			}
			Dictionary<string, MTJSONObject> asDict = dict;
			if (asDict.ContainsKey (key)) {
				asDict [key] = asMTJSONObject;
			} else {
				asDict.Add (key, asMTJSONObject);
			}
		}

		public void Add (object item)
		{
			Set (list.Count, item);
		}

		public void Remove (string key)
		{
			Dictionary<string, MTJSONObject> asDict = dict;
			if (asDict.ContainsKey (key)) {
				asDict.Remove (key);
			}
		}

		public void RemoveAt (int index)
		{
			List<MTJSONObject> asList = list;
			if (index >= 0 && index < asList.Count) {
				asList.RemoveAt (index);
			}
		}

		public MTJSONObject Clone ()
		{
			if (isDict || isList) {
				return MTJSON.Deserialize (MTJSON.Serialize (_o));
			}
			return new MTJSONObject (_o);
		}

		public override string ToString ()
		{
			if (isDict || isList) {
				return MTJSON.Serialize (_o);
			}
			return _o.ToString ();
		}

		public override bool Equals (Object obj)
		{
			if (obj == null) {
				return this._o == null;
			}

			MTJSONObject jsonObj = obj as MTJSONObject;
			if ((Object)jsonObj == null) {
				return false;
			}

			if (this._o == null && jsonObj._o == null) {
				return true;
			}

			return this._o.Equals (jsonObj._o);
		}

		public bool Equals (MTJSONObject jsonObj)
		{
			if ((object)jsonObj == null) {
				return this._o == null;
			}

			if (this._o == null && jsonObj._o == null) {
				return true;
			}

			return this._o.Equals (jsonObj._o);
		}

		public override int GetHashCode ()
		{
			return _o.GetHashCode ();
		}

		public static bool operator == (MTJSONObject a, MTJSONObject b)
		{
			if (Object.ReferenceEquals (a, b)) {
				return true;
			}

			if ((((object)a == null) && b.isNull) || (((object)b == null) && a.isNull)) {
				return true;
			}

			if (((object)a == null) || ((object)b == null)) {
				return false;
			}

			return a.o == b.o;
		}

		public static bool operator != (MTJSONObject a, MTJSONObject b)
		{
			return !(a == b);
		}

	}
}
