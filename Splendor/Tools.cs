using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{
    class Tools
    {
        /// <summary>
        /// https://www.csharp-console-examples.com/loop/c-shuffle-list/
        /// Shuffle a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>Shuffled list</returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            Random rnd = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int k = rnd.Next(0, i);
                T value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
            return list;
        }
    }
}
