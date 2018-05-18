using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ShortGuid
{
    /// <summary>
    ///     Represents a globally unique identifier (GUID) with a
    ///     shorter string value.
    /// </summary>
    [JsonConverter(typeof(ShortGuidJsonConverter))]
    public struct ShortGuid
    {
        /// <summary>
        ///     A read-only instance of the ShortGuid class whose value
        ///     is guaranteed to be all zeroes.
        /// </summary>
        public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

        private Guid guidValue;

        private string encodedValue;

        /// <summary>
        ///     Creates a ShortGuid from a base64 encoded string
        /// </summary>
        /// <param name="value">
        ///     The encoded guid as a
        ///     base64 string
        /// </param>
        public ShortGuid(string value)
        {
            encodedValue = value;
            guidValue = Decode(value);
        }

        /// <summary>
        ///     Creates a ShortGuid from a Guid
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        public ShortGuid(Guid guid)
        {
            encodedValue = Encode(guid);
            guidValue = guid;
        }

        /// <summary>
        ///     Gets/sets the underlying Guid
        /// </summary>
        public Guid Guid
        {
            get => guidValue;

            set
            {
                if (value != guidValue)
                {
                    guidValue = value;
                    encodedValue = Encode(value);
                }
            }
        }

        /// <summary>
        ///     Gets/sets the underlying base64 encoded string
        /// </summary>
        public string Value
        {
            get => encodedValue;

            set
            {
                if (value == encodedValue) return;

                encodedValue = value;
                guidValue = Decode(value);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the ShortGuid class
        /// </summary>
        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        /// <summary>
        ///     Creates a new instance of a Guid using the string value,
        ///     then returns the base64 encoded version of the Guid.
        /// </summary>
        /// <param name="value">An actual Guid string (i.e. not a ShortGuid)</param>
        public static string Encode(string value)
        {
            return Encode(new Guid(value));
        }

        /// <summary>
        ///     Encodes the given Guid as a base64 string that is 22
        ///     characters long.
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        public static string Encode(Guid guid)
        {
            var encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        /// <summary>
        ///     Decodes the given base64 string
        /// </summary>
        /// <param name="value">The base64 encoded string of a Guid</param>
        /// <returns>A new Guid</returns>
        public static Guid Decode(string value)
        {
            value = value.Replace("_", "/").Replace("-", "+");
            var buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }

        /// <summary>
        ///     Determines if both ShortGuids have the same underlying
        ///     Guid value.
        /// </summary>
        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            return x.guidValue == y.guidValue;
        }

        /// <summary>
        ///     Determines if both ShortGuids do not have the
        ///     same underlying Guid value.
        /// </summary>
        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     Implicitly converts the ShortGuid to it's string equivalent
        /// </summary>
        public static implicit operator string(ShortGuid shortGuid)
        {
            return shortGuid.encodedValue;
        }

        /// <summary>
        ///     Implicitly converts the ShortGuid to it's Guid equivalent
        /// </summary>
        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid.guidValue;
        }

        /// <summary>
        ///     Implicitly converts the string to a ShortGuid
        /// </summary>
        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }

        /// <summary>
        ///     Implicitly converts the Guid to a ShortGuid
        /// </summary>
        public static implicit operator ShortGuid(Guid guid)
        {
            return new ShortGuid(guid);
        }

        /// <summary>
        ///     Returns the base64 encoded guid as a string
        /// </summary>
        public override string ToString()
        {
            return encodedValue;
        }

        /// <summary>
        ///     Returns a value indicating whether this instance and a
        ///     specified Object represent the same type and value.
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleInvalidCastException")]
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case ShortGuid _:
                    return guidValue.Equals(((ShortGuid) obj).guidValue);
                case Guid _:
                    return guidValue.Equals((Guid) obj);
                case string _:
                    return guidValue.Equals(((ShortGuid) obj).guidValue);
            }

            return false;
        }

        /// <summary>
        ///     Returns the HashCode for underlying Guid.
        /// </summary>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return guidValue.GetHashCode();
        }

        /// <summary>
        ///     Serialize the ShortGuid class as a single string
        ///     value instead of a pair.
        /// </summary>
        public class ShortGuidJsonConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var sg = (ShortGuid) value;
                serializer.Serialize(writer, sg.Value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                return new ShortGuid(reader.Value.ToString());
            }

            public override bool CanConvert(Type objectType)
            {
                return false;
            }
        }
    }
}