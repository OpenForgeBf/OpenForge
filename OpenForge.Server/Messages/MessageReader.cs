// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenForge.Server.Enumerations;
using OpenForge.Server.PacketStructures;

namespace OpenForge.Server.Messages
{
    public class MessageReader : BinaryReader
    {
        public MessageReader(Stream input) : base(input)
        {
        }

        public MessageReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public MessageReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public T Deserialize<T>() => MessageDeserializer.Deserialize<T>(this);

        public object Deserialize(Type type) => MessageDeserializer.Deserialize(this, type);

        public bool[] ReadBooleanArray(int count) => ReadPrimitiveArray<bool>(count);

        public List<bool> ReadBooleanList(int count) => ReadPrimitiveList<bool>(count);

        public byte[] ReadByteArray(int count) => ReadPrimitiveArray<byte>(count);

        public List<byte> ReadByteList(int count) => ReadPrimitiveList<byte>(count);

        public char[] ReadCharArray(int count) => ReadPrimitiveArray<char>(count);

        public List<char> ReadCharList(int count) => ReadPrimitiveList<char>(count);

        public decimal[] ReadDecimalArray(int count) => ReadPrimitiveArray<decimal>(count);

        public List<decimal> ReadDecimalList(int count) => ReadPrimitiveList<decimal>(count);

        public double[] ReadDoubleArray(int count) => ReadPrimitiveArray<double>(count);

        public List<double> ReadDoubleList(int count) => ReadPrimitiveList<double>(count);

        public float[] ReadFloatArray(int count) => ReadPrimitiveArray<float>(count);

        public List<float> ReadFloatList(int count) => ReadPrimitiveList<float>(count);

        public CNetDataHeader ReadHeader()
        {
            var header = new CNetDataHeader();
            var length = ReadInt32();
            var clientIds = new long[length];
            for (var i = 0; i < length; length++)
            {
                clientIds[i] = ReadInt64();
            }

            header.ClientIds = clientIds;

            header.Interface = (InterfaceType)ReadInt32();
            header.MessageId = ReadInt32();

            header.Broadcast = ReadBoolean();
            header.RemoteMethod = ReadBoolean();

            header.DestinationServerId = ReadInt32();
            header.SourceServerId = ReadInt32();
            header.SequenceNumber = ReadInt32();
            header.RequestId = ReadInt32();

            header.CharacterId = ReadUInt64();
            header.Channel = ReadUInt64();

            return header;
        }

        public short[] ReadInt16Array(int count) => ReadPrimitiveArray<short>(count);

        public List<short> ReadInt16List(int count) => ReadPrimitiveList<short>(count);

        public int[] ReadInt32Array(int count) => ReadPrimitiveArray<int>(count);

        public List<int> ReadInt32List(int count) => ReadPrimitiveList<int>(count);

        public long[] ReadInt64Array(int count) => ReadPrimitiveArray<long>(count);

        public List<long> ReadInt64List(int count) => ReadPrimitiveList<long>(count);

        public sbyte[] ReadSByteArray(int count) => ReadPrimitiveArray<sbyte>(count);

        public List<sbyte> ReadSByteList(int count) => ReadPrimitiveList<sbyte>(count);

        public override string ReadString()
        {
            var length = ReadInt32();

            if (length > 0)
            {
                var bytes = ReadBytes(length);
                return Encoding.UTF8.GetString(bytes);
            }

            return null;
        }

        public ushort[] ReadUInt16Array(int count) => ReadPrimitiveArray<ushort>(count);

        public List<ushort> ReadUInt16List(int count) => ReadPrimitiveList<ushort>(count);

        public uint[] ReadUInt32Array(int count) => ReadPrimitiveArray<uint>(count);

        public List<uint> ReadUInt32List(int count) => ReadPrimitiveList<uint>(count);

        public ulong[] ReadUInt64Array(int count) => ReadPrimitiveArray<ulong>(count);

        public List<ulong> ReadUInt64List(int count) => ReadPrimitiveList<ulong>(count);

        private T[] ReadPrimitiveArray<T>(int count) where T : unmanaged
        {
            var values = new T[count];
            Read(MemoryMarshal.AsBytes(new Span<T>(values)));
            return values;
        }

        private List<T> ReadPrimitiveList<T>(int count) where T : unmanaged
        {
            var values = new List<T>(count);
            Read(MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(values)));
            return values;
        }
    }
}
