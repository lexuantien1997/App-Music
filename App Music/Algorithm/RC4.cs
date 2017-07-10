using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Algorithm
{
    public static class RC4
    {
        // https://www.slideshare.net/lehung123/h-mt-rc4-presentation
        /* RC4 using:
         * 2 registers with 8 bits : Q1 & Q2 
         * S_Block length=256 bit=8 . Value's S is random from 0..255
         *  Based procedure of RC4 is create Gramma. Because When you have Gramma you have just been XOR with S
         */

        // There are 3 step to encode/decode RC4
        // 1. Initilize S_Block
        // 2. Find Gramma[i]
        // Encode _ Decode

        public static byte[] Encode(byte[] _key, byte[] _data)
        {
            byte[] cipher = new byte[_data.Length];

            // 1. Intilize S_Block
            int[] S = new int[256];
            int[] K = new int[256];
            // Algorithm
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
                K[i] = _key[i % _key.Length];
            }
            int j = 0;
            // Random S Block with Algorihm's formular
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + K[i]) % 256; // Formular
                int tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
                // swap(ref S[i], ref S[j]);
            }
            // 2. Find Gramma
            int Q1, Q2, T;
            Q1 = Q2 = T = 0;
            // Algorithm
            for (int i = 0; i < _data.Length; i++)
            {
                Q1++;
                Q1 = (Q1) % 256;
                Q2 = (Q2 + S[Q1]) % 256;

                int tmp = S[Q1];
                S[Q1] = S[Q2];
                S[Q2] = tmp;
                // swap(ref S[Q1], ref S[Q2]);

                T = (S[Q1] + S[Q2]) % 256;

                int Gamma = S[T];

                cipher[i] = (Byte)(_data[i] ^ Gamma);
            }

            return cipher;
        }

        public  static byte[] Decode(byte[] key, byte[] data)
        {
            return Encode(key, data);
        }

        private static void swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static Byte[] HexStringToByteArray(string HexString)
        {
            byte[] result=new byte[HexString.Length/2];
            for (int i = 0; i < HexString.Length; i+=2)
            {
                result[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }

            return result;
        }

        
    }
}
