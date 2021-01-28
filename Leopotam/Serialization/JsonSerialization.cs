// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace EFramework.Serialization {
    /// <summary>
    /// Helper for custom naming of fields on json serialization / deserialization.
    /// Useful for decreasing json-data length.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class JsonNameAttribute : Attribute {
        /// <summary>
        /// Default initialization.
        /// </summary>
        public JsonNameAttribute () { }

        /// <summary>
        /// Initialization with specified name.
        /// </summary>
        /// <param name="name">Field name at json-data.</param>
        public JsonNameAttribute (string name) {
            Name = name;
        }

        /// <summary>
        /// Get json-data based name for field.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
    }

    /// <summary>
    /// Helper for fields that should be ignored during json serialization / deserialization.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : Attribute { }

    /// <summary>
    /// Json serialization.
    /// </summary>
    public class JsonSerialization {
        Reader _reader;

        readonly StringBuilder _sb = new StringBuilder (1024);

        readonly HashSet<Type> _numericTypes = new HashSet<Type> {
            typeof (byte),
            typeof (sbyte),
            typeof (short),
            typeof (ushort),
            typeof (int),
            typeof (uint),
            typeof (long),
            typeof (ulong),
            typeof (float),
            typeof (double)
        };

        /// <summary>
        /// Default initialization.
        /// </summary>
        public JsonSerialization () {
            _reader = new Reader ();
        }

        void SerializeMember (object obj) {
            if (obj == null) {
                return;
            }

            // string
            var asStr = obj as string;
            if (asStr != null) {
                _sb.Append ("\"");
                if (asStr.IndexOf ('"') != -1) {
                    asStr = asStr.Replace ("\\", "\\\\");
                    asStr = asStr.Replace ("\"", "\\\"");
                }
                _sb.Append (asStr);
                _sb.Append ("\"");
                return;
            }

            var objType = obj.GetType ();

            // nullable
            // null value will be skipped, dont use overrided default values for nullable types.
            //            var nullableType = Nullable.GetUnderlyingType (objType);
            //            if (nullableType != null) {
            //                _sb.Append ("null");
            //                return;
            //            }

            // number
            if (_numericTypes.Contains (objType)) {
                _sb.Append (((float) Convert.ChangeType (obj, typeof (float))).ToString (NumberFormatInfo.InvariantInfo));
                return;
            }
            // enum
            if (objType.IsEnum) {
                _sb.Append (Convert.ChangeType (obj, typeof (int)));
                return;
            }
            // boolean
            if (objType == typeof (bool)) {
                _sb.Append ((bool) obj ? "true" : "false");
                return;
            }

            // array
            var list = obj as IList;
            if (list != null) {
                _sb.Append ("[");
                var iMax = list.Count;
                if (iMax > 0) {
                    SerializeMember (list[0]);
                }
                for (var i = 1; i < iMax; i++) {
                    _sb.Append (",");
                    SerializeMember (list[i]);
                }
                _sb.Append ("]");
                return;
            }

            // dict
            var dict = obj as IDictionary;
            if (dict != null) {
                var dictEnum = dict.GetEnumerator ();
                var isComma = false;
                bool noNeedWrapKey;
                _sb.Append ("{");
                while (dictEnum.MoveNext ()) {
                    noNeedWrapKey = dictEnum.Key is string;
                    if (isComma) {
                        _sb.Append (",");
                    }
                    if (!noNeedWrapKey) {
                        _sb.Append ("\"");
                    }
                    SerializeMember (dictEnum.Key);
                    if (!noNeedWrapKey) {
                        _sb.Append ("\"");
                    }
                    _sb.Append (":");
                    SerializeMember (dictEnum.Value);
                    isComma = true;
                }
                _sb.Append ("}");
                return;
            }

            // object
            var desc = TypesCache.Instance.GetCache (objType);
            if (desc != null) {
                var isComma = false;
                object val;
                _sb.Append ("{");
                foreach (var field in desc.Fields) {
                    val = field.Value.GetValue (obj);
                    if (val != null) {
                        if (isComma) {
                            _sb.Append (",");
                        }
                        _sb.Append ("\"");
                        _sb.Append (field.Key);
                        _sb.Append ("\"");
                        _sb.Append (":");
                        SerializeMember (val);
                        isComma = true;
                    }
                }
                foreach (var prop in desc.Properties) {
                    val = prop.Value.GetValue (obj, null);
                    if (val != null) {
                        if (isComma) {
                            _sb.Append (",");
                        }
                        _sb.Append ("\"");
                        _sb.Append (prop.Key);
                        _sb.Append ("\"");
                        _sb.Append (":");
                        SerializeMember (val);
                        isComma = true;
                    }
                }
                _sb.Append ("}");
            }
        }

        /// <summary>
        /// Serialize specified object to json-data.
        /// </summary>
        /// <returns>Json data string.</returns>
        /// <param name="obj">Object to serialize.</param>
        public string Serialize (object obj) {
            if (obj == null) {
                throw new Exception ("instance is null");
            }
            _sb.Length = 0;

            SerializeMember (obj);

            return _sb.ToString ();
        }

        /// <summary>
        /// Deserialize json to instance of strong-typed class.
        /// </summary>
        /// <returns>Deserialized instance.</returns>
        /// <param name="json">Json data.</param>
        /// <typeparam name="T">Type of instance for deserialization.</typeparam>
        public T Deserialize<T> (string json) {
            if (json == null) {
                throw new Exception ("empty json data");
            }
            _reader.SetType (typeof (T));
            _reader.SetJson (json);
            return (T) _reader.ParseValue ();
        }

        class Reader {
            static bool IsWordBreak (int c) {
                return (c >= 0x01 && c <= 0x2a) || (c == 0x2c) || (c == 0x2f) || (c >= 0x3a && c <= 0x40) || (c >= 0x5b && c <= 0x5e) || (c == 0x60) || (c >= 0x7b);
            }

            enum JsonToken {
                None,

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

            readonly Stack<ArrayList> _arrayPool = new Stack<ArrayList> (16);

            readonly char[] _hexBuf = new char[4];

            readonly StringBuilder _stringBuf = new StringBuilder (128);

            string _json;

            int _jsonPos;

            Type _type;

            ArrayList GetArrayItem () {
                if (_arrayPool.Count > 0) {
                    return _arrayPool.Pop ();
                }
                return new ArrayList ();
            }

            void RecycleArrayItem (ArrayList item) {
                if (item != null) {
                    item.Clear ();
                    _arrayPool.Push (item);
                }
            }

            int JsonPeek () {
                return _jsonPos < _json.Length ? _json[_jsonPos] : -1;
            }

            char GetNextChar () {
                return _json[_jsonPos++];
            }

            object ParseByToken (JsonToken token) {
                switch (token) {
                    case JsonToken.String:
                        return ParseString ();
                    case JsonToken.Number:
                        return ParseNumber ();
                    case JsonToken.CurlyOpen:
                        return ParseObject ();
                    case JsonToken.SquaredOpen:
                        return ParseArray ();
                    case JsonToken.True:
                        return true;
                    case JsonToken.False:
                        return false;
                    case JsonToken.Null:
                        return null;
                    default:
                        return null;
                }
            }

            object ParseObject () {
                var objType = _type;
                object v = null;
                IDictionary dict = null;
                Type[] dictTypes = null;
                if (_type != null) {
                    v = Activator.CreateInstance (objType);
                    dict = v as IDictionary;
                    if (dict != null) {
                        dictTypes = objType.GetGenericArguments ();
                    }
                }

                // {
                GetNextChar ();
                while (true) {
                    switch (PeekNextToken ()) {
                        case JsonToken.None:
                            return null;
                        case JsonToken.Comma:
                            continue;
                        case JsonToken.CurlyClose:
                            return v;
                        default:

                            // key :
                            var name = ParseString ();
                            if (name == null || PeekNextToken () != JsonToken.Colon) {
                                throw new Exception ("Invalid object format");
                            }
                            GetNextChar ();

                            // value
                            if (objType != null) {
                                _type = dict != null ? dictTypes[1] : TypesCache.Instance.GetWantedType (objType, name);
                            }
                            var v1 = ParseValue ();
                            if (objType != null) {
                                if (v1 != null) {
                                    if (dict != null) {
                                        dict.Add (Convert.ChangeType (name, dictTypes[0], NumberFormatInfo.InvariantInfo), v1);
                                    } else {
                                        TypesCache.Instance.SetValue (objType, name, v, v1);
                                    }
                                }
                            }
                            _type = objType;
                            break;
                    }
                }
            }

            object ParseArray () {
                var isArray = false;
                var arrType = _type;
                Type itemType = null;
                ArrayList list = null;
                if (_type != null) {
                    isArray = arrType.IsArray;
                    itemType = isArray ? arrType.GetElementType () : arrType.GetProperty ("Item").PropertyType;
                    _type = itemType;
                    list = GetArrayItem ();
                }

                // [
                GetNextChar ();
                var parsing = true;
                while (parsing) {
                    switch (PeekNextToken ()) {
                        case JsonToken.None:
                            if (list != null) {
                                RecycleArrayItem (list);
                            }
                            return null;
                        case JsonToken.Comma:
                            continue;
                        case JsonToken.SquaredClose:
                            parsing = false;
                            break;
                        default:
                            var v1 = ParseByToken (PeekNextToken ());
                            if (itemType != null) {
                                list.Add (Convert.ChangeType (v1, itemType, NumberFormatInfo.InvariantInfo));
                                _type = itemType;
                            }
                            break;
                    }
                }
                object v = null;
                if (arrType != null) {
                    if (isArray) {
                        v = list.ToArray (itemType);
                    } else {
                        v = Activator.CreateInstance (arrType);
                        var vList = v as IList;
                        if (vList == null) {
                            throw new Exception (string.Format ("Type '{0}' not compatible with array data", _type.Name));
                        }
                        for (var i = 0; i < list.Count; i++) {
                            vList.Add (list[i]);
                        }
                    }
                }
                if (list != null) {
                    RecycleArrayItem (list);
                }
                return v;
            }

            string ParseString () {
                _stringBuf.Length = 0;
                char c;

                // "
                GetNextChar ();
                bool parsing = true;
                while (parsing) {
                    if (JsonPeek () == -1) {
                        break;
                    }
                    c = GetNextChar ();
                    switch (c) {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (JsonPeek () == -1) {
                                throw new Exception ("Invalid string format");
                            }
                            c = GetNextChar ();
                            switch (c) {
                                case '"':
                                case '\\':
                                case '/':
                                    _stringBuf.Append (c);
                                    break;
                                case 'b':
                                    _stringBuf.Append ('\b');
                                    break;
                                case 'f':
                                    _stringBuf.Append ('\f');
                                    break;
                                case 'n':
                                    _stringBuf.Append ('\n');
                                    break;
                                case 'r':
                                    _stringBuf.Append ('\r');
                                    break;
                                case 't':
                                    _stringBuf.Append ('\t');
                                    break;
                                case 'u':
                                    for (int i = 0; i < 4; i++) {
                                        _hexBuf[i] = GetNextChar ();
                                    }
                                    _stringBuf.Append ((char) Convert.ToInt32 (new string (_hexBuf), 16));
                                    break;
                            }
                            break;
                        default:
                            _stringBuf.Append (c);
                            break;
                    }
                }
                return _stringBuf.ToString ();
            }

            object ParseNumber () {
                var numString = GetNextWord ();
                if (_type != null) {
                    var n = float.Parse (numString, NumberFormatInfo.InvariantInfo);
                    if (_type == typeof (float)) {
                        return n;
                    }
                    if (_type == typeof (int)) {
                        return (int) n;
                    }
                    if (_type == typeof (long)) {
                        return (long) ((int) n);
                    }
                    if (_type == typeof (byte)) {
                        return (byte) ((int) n);
                    }
                    if (_type.IsEnum) {
                        return Enum.ToObject (_type, (int) n);
                    } else {
                        var nullableType = Nullable.GetUnderlyingType (_type);
                        return Convert.ChangeType (n, nullableType ?? _type, NumberFormatInfo.InvariantInfo);
                    }
                }
                return null;
            }

            void SkipWhiteSpaces () {
                while (Char.IsWhiteSpace ((char) JsonPeek ())) {
                    GetNextChar ();
                    if (JsonPeek () == -1) {
                        break;
                    }
                }
            }

            string GetNextWord () {
                _stringBuf.Length = 0;
                while (!IsWordBreak (JsonPeek ())) {
                    _stringBuf.Append (GetNextChar ());

                    if (JsonPeek () == -1) {
                        break;
                    }
                }
                return _stringBuf.ToString ();
            }

            JsonToken PeekNextToken () {
                SkipWhiteSpaces ();
                if (JsonPeek () == -1) {
                    return JsonToken.None;
                }
                switch ((char) JsonPeek ()) {
                    case '{':
                        return JsonToken.CurlyOpen;
                    case '}':
                        GetNextChar ();
                        return JsonToken.CurlyClose;
                    case '[':
                        return JsonToken.SquaredOpen;
                    case ']':
                        GetNextChar ();
                        return JsonToken.SquaredClose;
                    case ',':
                        GetNextChar ();
                        return JsonToken.Comma;
                    case '"':
                        return JsonToken.String;
                    case ':':
                        return JsonToken.Colon;
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
                        return JsonToken.Number;
                }
                switch (GetNextWord ()) {
                    case "false":
                        return JsonToken.False;
                    case "true":
                        return JsonToken.True;
                    case "null":
                        return JsonToken.Null;
                }
                throw new Exception ("Invalid json");
            }

            public object ParseValue () {
                return ParseByToken (PeekNextToken ());
            }

            public void SetJson (string jsonString) {
                _json = jsonString;
                _jsonPos = 0;
            }

            public void SetType (Type type) {
                _type = type;
            }
        }

        sealed class TypesCache {
            public sealed class TypeDesc {
                public readonly Dictionary<string, FieldInfo> Fields = new Dictionary<string, FieldInfo> (8);

                public readonly Dictionary<string, PropertyInfo> Properties = new Dictionary<string, PropertyInfo> (8);
            }

            public static readonly TypesCache Instance = new TypesCache ();

            readonly Dictionary<Type, TypeDesc> _types = new Dictionary<Type, TypeDesc> (32);

            static readonly object _syncLock = new object ();

            public void SetValue (Type type, string name, object instance, object val) {
                var desc = GetCache (type);
                FieldInfo fi;
                if (desc.Fields.TryGetValue (name, out fi)) {
                    fi.SetValue (
                        instance,
                        fi.FieldType.IsEnum && val is string ? Enum.Parse (fi.FieldType, (string) val) : val);
                } else {
                    PropertyInfo pi;
                    if (desc.Properties.TryGetValue (name, out pi)) {
                        pi.SetValue (
                            instance,
                            pi.PropertyType.IsEnum && val is string ? Enum.Parse (pi.PropertyType, (string) val) : val,
                            null);
                    }
                }
            }

            public Type GetWantedType (Type type, string name) {
                var desc = GetCache (type);
                if (desc.Fields.ContainsKey (name)) {
                    return desc.Fields[name].FieldType;
                }
                if (desc.Properties.ContainsKey (name)) {
                    return desc.Properties[name].PropertyType;
                }
                return null;
            }

            public void Clear () {
                lock (_syncLock) {
                    _types.Clear ();
                }
            }

            public TypeDesc GetCache (Type type) {
                TypeDesc desc;
                lock (_syncLock) {
                    if (!_types.ContainsKey (type)) {
                        desc = new TypeDesc ();
                        var ignoreType = typeof (JsonIgnoreAttribute);
                        var nameType = typeof (JsonNameAttribute);
                        string name;
                        foreach (var f in type.GetFields ()) {
                            if (f.IsPublic && !f.IsStatic && !f.IsInitOnly && !f.IsLiteral && !Attribute.IsDefined (f, ignoreType)) {
                                if (Attribute.IsDefined (f, nameType)) {
                                    name = ((JsonNameAttribute) Attribute.GetCustomAttribute (f, nameType)).Name;
                                    if (string.IsNullOrEmpty (name)) {
                                        name = f.Name;
                                    }
                                } else {
                                    name = f.Name;
                                }
                                try {
                                    desc.Fields.Add (name, f);
                                } catch {
                                    throw new ArgumentException ("Duplicated field name", name);
                                }
                            }
                        }
                        foreach (var p in type.GetProperties ()) {
                            if (p.CanRead && p.CanWrite && Attribute.IsDefined (p, nameType) && !Attribute.IsDefined (p, ignoreType)) {
                                if (string.CompareOrdinal (p.Name, "Item") != 0 || p.GetIndexParameters ().Length == 0) {
                                    if (Attribute.IsDefined (p, nameType)) {
                                        name = ((JsonNameAttribute) Attribute.GetCustomAttribute (p, nameType)).Name;
                                        if (string.IsNullOrEmpty (name)) {
                                            name = p.Name;
                                        }
                                    } else {
                                        name = p.Name;
                                    }
                                    try {
                                        if (desc.Fields.ContainsKey (name)) {
                                            throw new Exception ();
                                        }
                                        desc.Properties.Add (name, p);
                                    } catch {
                                        throw new ArgumentException ("Duplicated property name", name);
                                    }
                                }
                            }
                        }
                        _types[type] = desc;
                    } else {
                        desc = _types[type];
                    }
                }
                return desc;
            }
        }
    }
}