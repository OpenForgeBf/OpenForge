// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenForge.Server.Messages
{
    public class MessageWriter : BinaryWriter
    {
        public MessageWriter(Stream output) : base(output)
        {
        }

        public MessageWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
        }

        public MessageWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
        }

        public void Serialize(object value) => MessageSerializer.Serialize(this, value);

        public override void Write(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                Write(bytes.Length);
                WritePrimitiveArray(bytes);
            }
            else
            {
                Write((int)0);
            }
        }

        public void Write(sbyte[] values) => WritePrimitiveArray(values);

        public void Write(short[] values) => WritePrimitiveArray(values);

        public void Write(int[] values) => WritePrimitiveArray(values);

        public void Write(long[] values) => WritePrimitiveArray(values);

        public void Write(ushort[] values) => WritePrimitiveArray(values);

        public void Write(uint[] values) => WritePrimitiveArray(values);

        public void Write(ulong[] values) => WritePrimitiveArray(values);

        public void Write(float[] values) => WritePrimitiveArray(values);

        public void Write(double[] values) => WritePrimitiveArray(values);

        public void Write(bool[] values) => WritePrimitiveArray(values);

        public void Write(decimal[] values) => WritePrimitiveArray(values);

        public void Write(List<sbyte> values) => WritePrimitiveList(values);

        public void Write(List<short> values) => WritePrimitiveList(values);

        public void Write(List<int> values) => WritePrimitiveList(values);

        public void Write(List<long> values) => WritePrimitiveList(values);

        public void Write(List<byte> values) => WritePrimitiveList(values);

        public void Write(List<ushort> values) => WritePrimitiveList(values);

        public void Write(List<uint> values) => WritePrimitiveList(values);

        public void Write(List<ulong> values) => WritePrimitiveList(values);

        public void Write(List<char> values) => WritePrimitiveList(values);

        public void Write(List<float> values) => WritePrimitiveList(values);

        public void Write(List<double> values) => WritePrimitiveList(values);

        public void Write(List<bool> values) => WritePrimitiveList(values);

        public void Write(List<decimal> values) => WritePrimitiveList(values);

        private void WritePrimitiveArray<T>(T[] values) where T : unmanaged
        {
            Write(MemoryMarshal.AsBytes(new ReadOnlySpan<T>(values)));
        }

        private void WritePrimitiveList<T>(List<T> values) where T : unmanaged
        {
            Write(MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(values)));
        }
    }
}
