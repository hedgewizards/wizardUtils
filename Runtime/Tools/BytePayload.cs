using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils
{
    public class BytePayload
    {
        private byte[] data;
        private int Offset;

        public byte[] Data => data;

        public BytePayload(int length)
        {
            data = new byte[length];
            Offset = 0;
        }

        public void WriteBytes(byte[] bytes)
        {
            for (int n = 0; n < bytes.Length; n++)
            {
                WriteByte(bytes[n]);
            }
        }

        public void WriteByte(byte b)
        {
            if (Offset >= data.Length)
            {
                throw new OutOfMemoryException($"Tried to write too much data to the BytePayload ({Offset}/{data.Length} aleady written)");
            }
            data[Offset++] = b;
        }

    }
}
