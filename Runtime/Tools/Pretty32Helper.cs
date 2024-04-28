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
            Atlas ??= ReadableAtlas;

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
        /// Does this look like a Pretty32 string for the supplied <paramref name="Atlas"/> (or by default <see cref="ReadableAtlas"/>)?
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="Atlas"></param>
        /// <returns>True if every character in <paramref name="slug"/> is in the supplied Atlas</returns>
        public static bool Match(string slug, char[] Atlas = null)
        {
            Atlas ??= ReadableAtlas;

            return slug.All(c => ReadableAtlas.Contains(c));
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
