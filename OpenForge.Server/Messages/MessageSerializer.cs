// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OpenForge.Server.Extensions;

namespace OpenForge.Server.Messages
{
    public static class MessageSerializer
    {
        private static readonly Dictionary<Type, Expression<Action<MessageWriter, object>>> s_messageSerializerExpressions = new Dictionary<Type, Expression<Action<MessageWriter, object>>>();
        private static readonly Dictionary<Type, Action<MessageWriter, object>> s_messageSerializers = new Dictionary<Type, Action<MessageWriter, object>>();

        public static Action<MessageWriter, object> GetSerializer(Type type)
        {
            lock (s_messageSerializers)
            {
                if (s_messageSerializers.TryGetValue(type, out var serializer))
                {
                    return serializer;
                }

                var builtSerializer = BuildSerializer(type);
                s_messageSerializers[type] = builtSerializer;
                return builtSerializer;
            }
        }

        public static void Serialize(MessageWriter writer, object value)
        {
            var serializer = GetSerializer(value.GetType());
            serializer(writer, value);
        }

        private static Expression<Action<MessageWriter, object>> BuildExpressionTree(Type type)
        {
            var writer = Expression.Parameter(typeof(MessageWriter), "writer");
            var value = Expression.Parameter(typeof(object), "value");

            var variables = new List<ParameterExpression>();
            var expressions = new List<Expression>();

            var castedValue = Expression.Parameter(type, "castedValue");
            variables.Add(castedValue);
            expressions.Add(Expression.IfThen(Expression.Equal(value, Expression.Constant(null, typeof(object))), Expression.Throw(Expression.Constant(new MessageException($"Field of type '{type.Name}' can not be null.")))));
            expressions.Add(Expression.Assign(castedValue, Expression.TypeAs(value, type)));

            var properties = type.GetSerializableProperties();

            foreach (var property in properties)
            {
                var source = Expression.Property(castedValue, property);
                expressions.Add(Write(property.PropertyType, writer, source));
            }

            return Expression.Lambda<Action<MessageWriter, object>>(Expression.Block(variables, expressions), writer, value);
        }

        private static Action<MessageWriter, object> BuildSerializer(Type type) => GetExpressionTree(type).Compile();

        private static Expression CallTypeSerializer(Type type, Expression writer, Expression value)
        {
            var expressionTree = GetExpressionTree(type);
            return Expression.Invoke(expressionTree, writer, Expression.TypeAs(value, typeof(object)));
        }

        private static Expression GetArrayIndex(Type type, Expression source, ParameterExpression i)
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

        private static Expression<Action<MessageWriter, object>> GetExpressionTree(Type type)
        {
            lock (s_messageSerializerExpressions)
            {
                if (s_messageSerializerExpressions.TryGetValue(type, out var expressionTree))
                {
                    return expressionTree;
                }

                var builtExpressionTree = BuildExpressionTree(type);
                s_messageSerializerExpressions[type] = builtExpressionTree;
                return builtExpressionTree;
            }
        }

        private static Expression GetLengthExpression(Type type, Expression source)
        {
            if (type.IsArray)
            {
                return Expression.Property(source, nameof(Array.Length));
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return Expression.Property(source, "Count");
            }

            throw new Exception("Not a valid array type.");
        }

        private static MethodInfo GetMessageWriterMethod(Type type)
        {
            if (type == typeof(sbyte))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(sbyte) });
            }
            else if (type == typeof(short))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(short) });
            }
            else if (type == typeof(int))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(int) });
            }
            else if (type == typeof(long))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(long) });
            }
            else if (type == typeof(byte))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(byte) });
            }
            else if (type == typeof(ushort))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(ushort) });
            }
            else if (type == typeof(uint))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(uint) });
            }
            else if (type == typeof(ulong))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(ulong) });
            }
            else if (type == typeof(char))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(char) });
            }
            else if (type == typeof(float))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(float) });
            }
            else if (type == typeof(double))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(double) });
            }
            else if (type == typeof(bool))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(bool) });
            }
            else if (type == typeof(decimal))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(decimal) });
            }
            else if (type == typeof(string))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(string) });
            }
            else if (type == typeof(sbyte[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(sbyte[]) });
            }
            else if (type == typeof(short[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(short[]) });
            }
            else if (type == typeof(int[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(int[]) });
            }
            else if (type == typeof(long[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(long[]) });
            }
            else if (type == typeof(byte[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(byte[]) });
            }
            else if (type == typeof(ushort[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(ushort[]) });
            }
            else if (type == typeof(uint[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(uint[]) });
            }
            else if (type == typeof(ulong[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(ulong[]) });
            }
            else if (type == typeof(char[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(char[]) });
            }
            else if (type == typeof(float[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(float[]) });
            }
            else if (type == typeof(double[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(double[]) });
            }
            else if (type == typeof(bool[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(bool[]) });
            }
            else if (type == typeof(decimal[]))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(decimal[]) });
            }
            else if (type == typeof(List<sbyte>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<sbyte>) });
            }
            else if (type == typeof(List<short>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<short>) });
            }
            else if (type == typeof(List<int>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<int>) });
            }
            else if (type == typeof(List<long>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<long>) });
            }
            else if (type == typeof(List<byte>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<byte>) });
            }
            else if (type == typeof(List<ushort>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<ushort>) });
            }
            else if (type == typeof(List<uint>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<uint>) });
            }
            else if (type == typeof(List<ulong>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<ulong>) });
            }
            else if (type == typeof(List<char>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<char>) });
            }
            else if (type == typeof(List<float>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<float>) });
            }
            else if (type == typeof(List<double>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<double>) });
            }
            else if (type == typeof(List<bool>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<bool>) });
            }
            else if (type == typeof(List<decimal>))
            {
                return typeof(MessageWriter).GetMethod(nameof(MessageWriter.Write), new Type[] { typeof(List<decimal>) });
            }
            else
            {
                throw new Exception("Not a base type.");
            }
        }

        private static Expression Write(Type type, Expression writer, Expression source)
        {
            if (MessageShared.IsTypeArray(type))
            {
                var elementType = MessageShared.GetArrayElementType(type);

                var nullCheck = Expression.Equal(source, Expression.Constant(null, type));
                return Expression.IfThenElse(nullCheck,
                    Write(typeof(int), writer, Expression.Constant(0)),
                    MessageShared.PrimitiveArrayTypes.Contains(elementType) ? WritePrimitiveArray(type, writer, source) : WriteArray(type, elementType, writer, source));
            }
            else
            {
                return WriteNonArray(type, writer, source);
            }
        }

        private static Expression WriteArray(Type type, Type elementType, Expression writer, Expression source)
        {
            var expressions = new List<Expression>();
            var lengthExpression = GetLengthExpression(type, source);
            expressions.Add(WriteFrom(typeof(int), writer, lengthExpression));

            var breakLabel = Expression.Label("LoopBreak");
            var i = Expression.Parameter(typeof(int), "i");
            expressions.Add(Expression.Assign(i, Expression.Constant(0)));

            var loop = Expression.Loop(
                Expression.IfThenElse(
                    Expression.LessThan(i, lengthExpression),
                        Expression.Block(
                            Write(elementType, writer, GetArrayIndex(type, source, i)),
                            Expression.Assign(i, Expression.Increment(i))
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel);

            expressions.Add(loop);

            return Expression.Block(new ParameterExpression[] { i }, expressions);
        }

        private static Expression WriteFrom(Type typeToWrite, Expression writer, Expression source)
        {
            var writeMethod = GetMessageWriterMethod(typeToWrite);
            if (!MessageShared.FinalTypes.Contains(typeToWrite))
            {
                throw new Exception("Not a valid type.");
            }

            return Expression.Call(writer, writeMethod, source);
        }

        private static Expression WriteNonArray(Type type, Expression writer, Expression source)
        {
            if (MessageShared.FinalTypes.Contains(type))
            {
                return WriteFrom(type, writer, source);
            }
            else if (type.IsEnum)
            {
                var underlyingType = Enum.GetUnderlyingType(type);
                return Write(underlyingType, writer, Expression.Convert(source, underlyingType));
            }
            else
            {
                return CallTypeSerializer(type, writer, source);
            }
        }

        private static Expression WritePrimitiveArray(Type arrayType, Expression writer, Expression source)
        {
            var lengthExpression = GetLengthExpression(arrayType, source);

            var writeMethod = GetMessageWriterMethod(arrayType);
            if (writeMethod == null)
            {
                throw new Exception("Not a valid type.");
            }

            return Expression.Block(
                WriteFrom(typeof(int), writer, lengthExpression),
                Expression.Call(writer, writeMethod, source)
            );
        }
    }
}
