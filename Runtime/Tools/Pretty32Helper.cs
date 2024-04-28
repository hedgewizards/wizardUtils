using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Tools
{
    public static class Pretty32Helper
    {
        // sorry... there's no encode/decode for this because im lazy. I just needed raw generate for lobby codes

        public static string Generate(int length, char[] Atlas = null)
        {
            if (Atlas == null) Atlas = ReadableAtlas;

            string final = "";

            for(int n = 0; n < length; n++)
            {
                int choice = (byte)UnityEngine.Random.Range(0, 32);

                try
                {
                    final += Atlas[choice];
                }
                catch(IndexOutOfRangeException e)
                {
                    Debug.LogError($"Failed to index byte {choice}\n" + e.ToString());
                    final += '?';
                }
            }

            return final;
        }

        /// <summary>
        /// All digits, and all letters except I, O, V, and U
        /// </summary>
        public static char[] ReadableAtlas => new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', // 0-7
            'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', // 8-15
            'S', 'T', 'W', 'X', 'Y', 'Z', '0', '1', // 16-23
            '2', '3', '4', '5', '6', '7', '8', '9' // 24-31
        };
    }
}
