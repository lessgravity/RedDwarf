using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LessGravity.Common
{
    public partial class DataStream
    {
        #region Read

        public byte ReadUInt8()
        {
            var value = BaseStream.ReadByte();
            if (value == -1)
            {
                throw new EndOfStreamException();
            }
            return (byte)value;
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        public Int32 ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public Int64 ReadInt64()
        {
            return (long)ReadUInt64();
        }

        public byte[] ReadUInt8Array(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be bigger than 0.", "length");
            }
            var result = new byte[length];
            var bytesRead = length;
            while (true)
            {
                bytesRead -= Read(result, length - bytesRead, bytesRead);
                if (bytesRead == 0)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            return result;
        }

        public UInt32 ReadUInt32()
        {
            return (uint)(
                (ReadUInt8() << 24) |
                (ReadUInt8() << 16) |
                (ReadUInt8() << 8) |
                 ReadUInt8());
        }

        public UInt64 ReadUInt64()
        {
            return unchecked(
                   ((UInt64)ReadUInt8() << 56) |
                   ((UInt64)ReadUInt8() << 48) |
                   ((UInt64)ReadUInt8() << 40) |
                   ((UInt64)ReadUInt8() << 32) |
                   ((UInt64)ReadUInt8() << 24) |
                   ((UInt64)ReadUInt8() << 16) |
                   ((UInt64)ReadUInt8() << 8) |
                    ReadUInt8());
        }

        #endregion Read
        #region Write

        public void WriteInt32(Int32 value)
        {
            WriteUInt32((UInt32)value);
        }

        public void WriteInt64(Int64 value)
        {
            WriteUInt64((UInt64)value);
        }

        public void WriteUInt32(UInt32 value)
        {
            Write(new[]
            {
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) (value & 0xFF)
            }, 0, 4);
        }

        public void WriteUInt64(UInt64 value)
        {
            Write(new[]
            {
                (byte)((value & 0xFF00000000000000) >> 56),
                (byte)((value & 0xFF000000000000) >> 48),
                (byte)((value & 0xFF0000000000) >> 40),
                (byte)((value & 0xFF00000000) >> 32),
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            }, 0, 8);
        }

        #endregion Write
    }
}
