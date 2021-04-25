// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OpenForge.Server.Messages
{
    public static class MessageShared
    {
        public static readonly Type[] FinalTypes = new Type[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char), typeof(float), typeof(double), typeof(bool), typeof(decimal), typeof(string) };
        public static readonly Type[] PrimitiveArrayTypes = new Type[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char), typeof(float), typeof(double), typeof(bool), typeof(decimal) };

        public static Type GetArrayElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }

        public static Expression GetArrayIndex(Type type, Expression source, ParameterExpression i)
        {
            if (type.IsArray)
            {
                return Expression.ArrayIndex(source, i);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var getItemMethod = source.Type.GetMethod("get_Item", new Type[] { typeof(int) });
                return Expression.Call(source, getItemMethod, i);
            }

            throw new Exception("Not a valid array type.");
        }

        public static Expression GetLengthExpression(Type type, Expression source)
        {
            if (type.IsArray)
            {
                return Expression.Property(source, "Length");
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return Expression.Property(source, "Count");
            }

            throw new Exception("Not a valid array type.");
        }

        public static bool IsTypeArray(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}
