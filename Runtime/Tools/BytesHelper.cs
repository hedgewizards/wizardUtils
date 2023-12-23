using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Tools
{
    public static class BytesHelper
    {
        public static byte[] GetDeviceAgnosticBytes(long value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetDeviceAgnosticBytes(ulong value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetDeviceAgnosticBytes(int value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetDeviceAgnosticBytes(uint value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetDeviceAgnosticBytes(short value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetDeviceAgnosticBytes(ushort value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                value = BinaryPrimitives.ReverseEndianness(value);
            }
            return BitConverter.GetBytes(value);
        }
    }
}
