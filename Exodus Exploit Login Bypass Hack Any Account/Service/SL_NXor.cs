using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Exodus.Service
{
    public partial class _SL
    {      
        public static class NXor
        {
            private static long xorValue = 123767345285252;

            public static long Xor(long input)
            {
                var xor = BitConverter.GetBytes(xorValue);
                var xouInput = BitConverter.GetBytes(input);
                BitArray arrayOut = new BitArray(xor).Xor(new BitArray(xouInput));
                byte[] array = new byte[arrayOut.Length / 8];
                arrayOut.CopyTo(array, 0);
                long outValue = BitConverter.ToInt64(array, 0);
                return outValue;
            }
        }
    }
}