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
    public static class MessageDeserializer
    {
        private static readonly Dictionary<Type, Expression<Func<MessageReader, object>>> s_messageDeserializerExpressions = new Dictionary<Type, Expression<Func<MessageReader, object>>>();
        private static readonly Dictionary<Type, Func<MessageReader, object>> s_messageDeserializers = new Dictionary<Type, Func<MessageReader, object>>();

        public static T Deserialize<T>(MessageReader reader) => (T)Deserialize(reader, typeof(T));

        public static object Deserialize(MessageReader reader, Type type)
        {
            var deserializer = GetDeserializer(type);
            return deserializer(reader);
        }

        public static Func<MessageReader, object> GetDeserializer(Type type)
        {
            lock (s_messageDeserializers)
            {
                if (s_messageDeserializers.TryGetValue(type, out var deserializer))
                {
                    return deserializer;
                }

                var builtDeserializer = BuildDeserializer(type);
                s_messageDeserializers[type] = builtDeserializer;
                return builtDeserializer;
            }
        }

        private static Expression ArrayAccess(Type type, Type elementType, Expression target, Expression i)
        {
            if (type.IsArray)
            {
                return Expression.ArrayAccess(target, i);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var listType = typeof(List<>).MakeGenericType(elementType);
                var indexer = listType.GetProperty("Item", new Type[] { typeof(int) });
                return Expression.Property(target, indexer, new Expression[] { i });
            }

            throw new Exception("Not a valid array type.");
        }

        private static Func<MessageReader, object> BuildDeserializer(Type type) => GetExpressionTree(type).Compile();

        private static Expression<Func<MessageReader, object>> BuildExpressionTree(Type type)
        {
            var reader = Expression.Parameter(typeof(MessageReader), "reader");

            var variables = new List<ParameterExpression>();
            var expressions = new List<Expression>();
            var returnTarget = Expression.Label(typeof(object));

            var value = Expression.Parameter(type, "value");
            variables.Add(value);
            expressions.Add(Expression.Assign(value, Expression.New(type)));

            var properties = type.GetSerializableProperties();
            foreach (var property in properties)
            {
                var target = Expression.Property(value, property);
                expressions.Add(Read(property.PropertyType, reader, target));
            }

            expressions.Add(Expression.Return(returnTarget, Expression.TypeAs(value, typeof(object))));
            expressions.Add(Expression.Label(returnTarget, Expression.Constant(null)));

            return Expression.Lambda<Func<MessageReader, object>>(Expression.Block(variables, expressions), reader);
        }

        private static Expression CallTypeDeserializer(Type type, Expression reader, Expression target)
        {
            var expressionTree = GetExpressionTree(type);
            return Expression.Assign(target, Expression.TypeAs(Expression.Invoke(expressionTree, reader), type));
        }

        private static Expression CallTypeSerializer(Type type, Expression writer, Expression value)
        {
            var expressionTree = GetExpressionTree(type);
            return Expression.Invoke(expressionTree, writer, Expression.TypeAs(value, typeof(object)));
        }

        private static Expression<Func<MessageReader, object>> GetExpressionTree(Type type)
        {
            lock (s_messageDeserializerExpressions)
            {
                if (s_messageDeserializerExpressions.TryGetValue(type, out var expressionTree))
                {
                    return expressionTree;
                }

                var builtExpressionTree = BuildExpressionTree(type);
                s_messageDeserializerExpressions[type] = builtExpressionTree;
                return builtExpressionTree;
            }
        }

        private static MethodInfo GetMessageReaderMethod(Type t)
        {
            if (t == typeof(sbyte))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadSByte));
            }
            else if (t == typeof(short))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt16));
            }
            else if (t == typeof(int))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt32));
            }
            else if (t == typeof(long))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt64));
            }
            else if (t == typeof(byte))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadByte));
            }
            else if (t == typeof(ushort))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt16));
            }
            else if (t == typeof(uint))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt32));
            }
            else if (t == typeof(ulong))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt64));
            }
            else if (t == typeof(char))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadChar));
            }
            else if (t == typeof(float))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadSingle));
            }
            else if (t == typeof(double))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadDouble));
            }
            else if (t == typeof(bool))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadBoolean));
            }
            else if (t == typeof(string))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadString));
            }
            else if (t == typeof(sbyte[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadSByteArray));
            }
            else if (t == typeof(short[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt16Array));
            }
            else if (t == typeof(int[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt32Array));
            }
            else if (t == typeof(long[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt64Array));
            }
            else if (t == typeof(byte[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadByteArray));
            }
            else if (t == typeof(ushort[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt16Array));
            }
            else if (t == typeof(uint[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt32Array));
            }
            else if (t == typeof(ulong[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt64Array));
            }
            else if (t == typeof(char[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadCharArray));
            }
            else if (t == typeof(float[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadFloatArray));
            }
            else if (t == typeof(double[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadDoubleArray));
            }
            else if (t == typeof(bool[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadBooleanArray));
            }
            else if (t == typeof(decimal[]))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadDecimalArray));
            }
            else if (t == typeof(List<sbyte>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadSByteList));
            }
            else if (t == typeof(List<short>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt16List));
            }
            else if (t == typeof(List<int>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt32List));
            }
            else if (t == typeof(List<long>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadInt64List));
            }
            else if (t == typeof(List<byte>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadByteList));
            }
            else if (t == typeof(List<ushort>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt16List));
            }
            else if (t == typeof(List<uint>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt32List));
            }
            else if (t == typeof(List<ulong>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadUInt64List));
            }
            else if (t == typeof(List<char>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadCharList));
            }
            else if (t == typeof(List<float>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadFloatList));
            }
            else if (t == typeof(List<double>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadDoubleList));
            }
            else if (t == typeof(List<bool>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadBooleanList));
            }
            else if (t == typeof(List<decimal>))
            {
                return typeof(MessageReader).GetMethod(nameof(MessageReader.ReadDecimalList));
            }
            else
            {
                throw new Exception("Not a base type.");
            }
        }

        private static Expression MakeArray(Type type, Type elementType, Expression length)
        {
            if (type.IsArray)
            {
                return Expression.NewArrayBounds(elementType, new Expression[] { length });
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var listType = typeof(List<>).MakeGenericType(elementType);
                var constructor = listType.GetConstructor(new Type[] { elementType.MakeArrayType() });
                return Expression.New(constructor, Expression.NewArrayBounds(elementType, new Expression[] { length }));
            }

            throw new Exception("Not a valid array type.");
        }

        private static Expression Read(Type type, Expression reader, Expression target)
        {
            if (MessageShared.IsTypeArray(type))
            {
                var elementType = MessageShared.GetArrayElementType(type);
                return MessageShared.PrimitiveArrayTypes.Contains(elementType) ? ReadPrimitiveArray(type, reader, target) : ReadArray(type, elementType, reader, target);
            }
            else
            {
                return ReadNonArray(type, reader, target);
            }
        }

        private static Expression ReadArray(Type type, Type elementType, Expression reader, Expression target)
        {
            var expressions = new List<Expression>();
            var length = Expression.Parameter(typeof(int), "length");
            expressions.Add(ReadInto(typeof(int), reader, length));
            expressions.Add(Expression.Assign(target, MakeArray(type, elementType, length)));

            var breakLabel = Expression.Label("LoopBreak");
            var i = Expression.Parameter(typeof(int), "i");
            expressions.Add(Expression.Assign(i, Expression.Constant(0)));

            var loop = Expression.Loop(
                Expression.IfThenElse(
                    Expression.LessThan(i, length),
                        Expression.Block(
                            Read(elementType, reader, ArrayAccess(type, elementType, target, i)),
                            Expression.Assign(i, Expression.Increment(i))
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel);

            expressions.Add(loop);

            return Expression.Block(new ParameterExpression[] { i, length }, expressions);
        }

        private static Expression ReadInto(Type type, Expression reader, Expression target)
        {
            var readMethod = GetMessageReaderMethod(type);
            if (!MessageShared.FinalTypes.Contains(type))
            {
                throw new Exception("Not a valid type.");
            }

            return Expression.Assign(target, Expression.Call(reader, readMethod));
        }

        private static Expression ReadNonArray(Type type, Expression reader, Expression target)
        {
            if (MessageShared.FinalTypes.Contains(type))
            {
                return ReadInto(type, reader, target);
            }
            else if (type.IsEnum)
            {
                var underlyingType = Enum.GetUnderlyingType(type);
                var enumValue = Expression.Parameter(underlyingType, "enumValue");

                return Expression.Block(new[] { enumValue }, new Expression[]
                    {
                        Read(underlyingType, reader, enumValue),
                        Expression.Assign(target, Expression.Convert(enumValue, type))
                    });
            }
            else
            {
                return CallTypeDeserializer(type, reader, target);
            }
        }

        private static Expression ReadPrimitiveArray(Type arrayType, Expression reader, Expression target)
        {
            var length = Expression.Parameter(typeof(int), "length");

            var readMethod = GetMessageReaderMethod(arrayType);
            if (readMethod == null)
            {
                throw new Exception("Not a valid type.");
            }

            return Expression.Block(
                new ParameterExpression[]
                {
                    length
                },
                new Expression[]
                {
                    ReadInto(typeof(int), reader, length),
                    Expression.IfThen(Expression.GreaterThan(length, Expression.Constant(0)), Expression.Assign(target, Expression.Call(reader, readMethod, length)))
                }
            );
        }
    }
}
