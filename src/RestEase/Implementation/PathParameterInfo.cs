﻿using System;
using System.Collections.Generic;

namespace RestEase.Implementation
{
    /// <summary>
    /// Interface for path parameter info. Defines common functionality for path parameter info objects.
    /// </summary>
    public interface IPathParameterInfo
    {
        /// <summary>
        /// Gets a value indicating whether this path parameter should be URL-encoded
        /// </summary>
        bool UrlEncode { get; }

        /// <summary>
        /// Gets the method to use to serialize the path parameter.
        /// </summary>
        PathSerializationMethod SerializationMethod { get; }

        /// <summary>
        /// Serialize the value into a name -> value pair using the given serializer
        /// </summary>
        /// <param name="serializer">Serializer to use</param>
        /// <param name="requestInfo">RequestInfo representing the request</param>
        /// <returns>Serialized value</returns>
        KeyValuePair<string, string> SerializeValue(RequestPathParamSerializer serializer, IRequestInfo requestInfo);

        /// <summary>
        /// Serialize the value into a name -> value pair using its ToString method
        /// </summary>
        /// <param name="formatProvider"><see cref="IFormatProvider"/> to use if the value implements <see cref="IFormattable"/></param>
        /// <returns>Serialized value</returns>
        KeyValuePair<string, string> SerializeToString(IFormatProvider formatProvider);
    }

    /// <inheritdoc />
    /// <summary>
    /// Structure containing information about a desired path parameter
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public struct PathParameterInfo<T> : IPathParameterInfo
    {
        private readonly string name;
        private readonly T value;
        private readonly string format;

        /// <summary>
        /// Gets a value indicating whether this path parameter should be URL-encoded
        /// </summary>
        public bool UrlEncode { get; }

        /// <inheritdoc />
        public PathSerializationMethod SerializationMethod { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PathParameterInfo{T}"/> Structure
        /// </summary>
        /// <param name="name">Name of the name/value pair</param>
        /// <param name="value">Value of the name/value pair</param>
        /// <param name="format">Format parameter to pass to ToString if value implements <see cref="IFormattable"/></param>
        /// <param name="urlEncode">Indicates whether this parameter should be url-encoded</param>
        /// <param name="serializationMethod">Method to use to serialize the path value.</param>
        public PathParameterInfo(string name, T value, string format, bool urlEncode, PathSerializationMethod serializationMethod)
        {
            this.name = name;
            this.value = value;
            this.format = format;
            this.UrlEncode = urlEncode;
            this.SerializationMethod = serializationMethod;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if serializer or requestInfo are null</exception>
        public KeyValuePair<string, string> SerializeValue(RequestPathParamSerializer serializer, IRequestInfo requestInfo)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (requestInfo == null)
                throw new ArgumentNullException(nameof(requestInfo));

            var serializedValue = serializer.SerializePathParam(this.value, new RequestPathParamSerializerInfo(requestInfo, this.format));
            return new KeyValuePair<string, string>(this.name, serializedValue);
        }

        /// <inheritdoc />
        public KeyValuePair<string, string> SerializeToString(IFormatProvider formatProvider)
        {
            return new KeyValuePair<string, string>(this.name, ToStringHelper.ToString(this.value, this.format, formatProvider));
        }
    }
}
