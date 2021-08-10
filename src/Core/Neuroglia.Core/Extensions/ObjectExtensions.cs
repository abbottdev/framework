﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Neuroglia
{

    /// <summary>
    /// Defines extensions for <see cref="object"/>s
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// Transforms the object into an <see cref="IDictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="source">The object to transform</param>
        /// <returns>A <see cref="IDictionary{TKey, TValue}"/> containing the specified object's property name/value pairs</returns>
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        /// <summary>
        /// Transforms the object into an <see cref="IDictionary{TKey, TValue}"/>
        /// </summary>
        /// <typeparam name="T">The type of values wrapped by the <see cref="IDictionary{TKey, TValue}"/> to create</typeparam>
        /// <param name="source">The object to transform</param>
        /// <returns>A <see cref="IDictionary{TKey, TValue}"/> containing the specified object's property name/value pairs</returns>
        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object value = property.GetValue(source);
                if (IsOfType<T>(value))
                    dictionary.Add(property.Name, (T)value);
            }
            return dictionary;
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

    }

}
