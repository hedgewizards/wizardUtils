using System;
using System.Collections.Generic;

namespace WizardUtils
{
    public static class RandomHelper
    {
        public static T FromCollection<T>(IList<T> source)
        {
            return FromCollection<T>(new System.Random(), source);
        }

        public static T FromCollection<T>(this System.Random random, IList<T> source)
        {
            if (source.Count == 0) throw new ArgumentOutOfRangeException($"source array is empty");

            return source[random.Next(source.Count)];
        }

        /// <summary>
        /// Returns K different integers between 0 (inclusive) and n (exclusive)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int[] ChooseKFromN(int k, int n)
        {
            return ChooseKFromN(new System.Random(), k, n);
        }

        /// <summary>
        /// Returns K different integers between 0 (inclusive) and n (exclusive)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int[] ChooseKFromN(this System.Random random, int k, int n)
        {
            int[] options = new int[n];
            int[] choices = new int[k];
            for (int index = 0; index < n; index++)
            {
                options[index] = index;
            }

            for (int choice = 0; choice < k; choice++)
            {
                var chosenIndex = random.Next(choice, n);
                choices[choice] = options[chosenIndex];

                options[chosenIndex] = options[choice];
            }

            return choices;
        }
        public static int[] ChooseNFromN(this System.Random random, int n)
        {
            int[] choices = new int[n];
            for (int index = 0; index < n; index++)
            {
                choices[index] = index;
            }

            for (int choice = 0; choice < n; choice++)
            {
                var chosenIndex = random.Next(choice, n);
                var temp = choices[choice];
                choices[choice] = choices[chosenIndex];
                choices[chosenIndex] = temp;
            }

            return choices;
        }

        /// <summary>
        /// assigns to buffer k different integers between 0 (inclusive) and n (exclusive)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static void ChooseKFromN(System.Random random, int[] buffer, int k, int n)
        {
            for (int x = 0; x < n; x++)
            {
                buffer[x] = x;
            }

            for (int thisIndex = 0; thisIndex < k; thisIndex++)
            {
                int otherIndex = random.Next(thisIndex, n);
                int temp = buffer[thisIndex];
                buffer[thisIndex] = buffer[otherIndex];
                buffer[otherIndex] = temp;
            }
        }

        public static void ChooseNFromN(System.Random random, int[] buffer, int n)
        {
            for (int x = 0; x < n; x++)
            {
                buffer[x] = x;
            }

            for (int thisIndex = 0; thisIndex < n; thisIndex++)
            {
                int otherIndex = random.Next(thisIndex, n);
                int temp = buffer[thisIndex];
                buffer[thisIndex] = buffer[otherIndex];
                buffer[otherIndex] = temp;
            }
        }
    }
}
