﻿/*
 * Copyright © 2021 Neuroglia SPRL. All rights reserved.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Neuroglia
{

    /// <summary>
    /// Defines helper methods to handle <see cref="Enum"/>s
    /// </summary>
    public static class EnumHelper
    {

        /// <summary>
        /// Parses the specified input into the desired <see cref="Enum"/>
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <param name="enumType">The type of the enum to parse</param>
        /// <returns>The parsed value</returns>
        public static object Parse(string value, Type enumType)
        {
            if (int.TryParse(value, out int intValue))
                return intValue;
            List<int> values = new();
            foreach (string flag in value.Split("|", StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()))
            {
                bool match = false;
                foreach (string name in Enum.GetNames(enumType))
                {
                    if (flag == name)
                    {
                        values.Add((int)Enum.Parse(enumType, flag));
                        match = true;
                        break;
                    }
                    EnumMemberAttribute enumMemberAttribute = enumType.GetField(name).GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttribute != null)
                    {
                        if (flag.ToLower() == enumMemberAttribute.Value.ToLower())
                        {
                            values.Add((int)Enum.Parse(enumType, name));
                            match = true;
                            break;
                        }
                    }
                }
                if (!match)
                    return enumType.GetDefaultValue();
            }
            int result = values.First();
            for (int i = 1; i < values.Count; i++)
            {
                result |= values[i];
            }
            return result;
        }

        /// <summary>
        /// Parses the specified input into the desired enum
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to parse</typeparam>
        /// <param name="value">The value to parse</param>
        /// <returns>The parsed value</returns>
        public static TEnum Parse<TEnum>(string value)
        {
            return (TEnum)Parse(value, typeof(TEnum));
        }

        /// <summary>
        /// Gets the string representation for the specified enum value
        /// </summary>
        /// <param name="value">The value to stringify</param>
        /// <param name="enumType">The type of the enum to stringify</param>
        /// <returns>The string representation for the specified enum value</returns>
        public static string Stringify(Enum value, Type enumType)
        {
            List<string> names = new();
            if (enumType.GetCustomAttribute<FlagsAttribute>() == null)
            {
                string name = Enum.GetName(enumType, value);
                EnumMemberAttribute enumMemberAttribute = enumType.GetField(name).GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute != null)
                    name = enumMemberAttribute.Value;
                return name;
            }
            foreach (object flag in GetFlags(value, enumType))
            {
                string name = Enum.GetName(enumType, flag);
                EnumMemberAttribute enumMemberAttribute = enumType.GetField(name).GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute != null)
                    name = enumMemberAttribute.Value;
                names.Add(name);
            }
            return string.Join(" | ", names);
        }

        /// <summary>
        /// Gets the string representation for the specified enum value
        /// </summary>
        /// <param name="value">The value to stringify</param>
        /// <typeparam name="TEnum">The type of the enum to stringify</typeparam>
        /// <returns>The string representation for the specified enum value</returns>
        public static string Stringify<TEnum>(TEnum value)
            where TEnum : Enum
        {
            return Stringify(value, typeof(TEnum));
        }

        /// <summary>
        /// Gets all the enum values contained by the specified flags value
        /// </summary>
        /// <param name="flags">The flags to get the values of</param>
        /// <param name="enumType">The type of the enumeration to get the flags of</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all the enum values contained by the specifed flags value</returns>
        public static IEnumerable GetFlags(Enum flags, Type enumType)
        {
            foreach (Enum value in Enum.GetValues(enumType))
            {
                if (flags.HasFlag(value))
                    yield return value;
            }
        }

        /// <summary>
        /// Gets all the enum values contained by the specified flags value
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to get the flags of</typeparam>
        /// <param name="flags">The flags to get the values of</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all the enum values contained by the specifed flags value</returns>
        public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum flags)
            where TEnum : Enum
        {
            return GetFlags(flags, typeof(TEnum)).OfType<TEnum>();
        }

        /// <summary>
        /// Gets the <see cref="FieldInfo"/> that corresponds to the specified enum value
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to get a <see cref="FieldInfo"/> of</typeparam>
        /// <param name="value">The enum value to get the <see cref="FieldInfo"/> of</param>
        /// <returns>The <see cref="FieldInfo"/> that corresponds to the specified enum value</returns>
        public static FieldInfo GetField<TEnum>(TEnum value)
            where TEnum : Enum
        {
            return typeof(TEnum).GetField(Enum.GetName(typeof(TEnum), value));
        }

        /// <summary>
        /// Gets the display name of the specified enum value
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to get the display name for</typeparam>
        /// <param name="value">The value to get the display name for</param>
        /// <returns>The display name of the specified enum value</returns>
        public static string GetDisplayName<TEnum>(TEnum value)
            where TEnum : Enum
        {
            string name = Enum.GetName(typeof(TEnum), value);
            FieldInfo field = GetField(value);
            if (field.TryGetCustomAttribute(out DisplayAttribute dispplayAttribute)
                && !string.IsNullOrWhiteSpace(dispplayAttribute.GetName()))
                name = dispplayAttribute.Name;
            return name;
        }

    }

}
