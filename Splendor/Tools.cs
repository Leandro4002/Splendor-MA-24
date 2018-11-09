/**
 * \file      Tools.cs
 * \author    Leandro Saraiva Maia & Alexandre Baseia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     Set of functions used in multiple ways
 */

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

        /// <summary>
        /// Takes money and price parameters and check if there is enought money to pay the price
        /// </summary>
        /// <param name="money"></param>
        /// <param name="price"></param>
        /// <returns>True if there is enought money to pay the price and false if not</returns>
        public static bool CheckEnoughtToBuy(int[] money, int[] price)
        {
            //Check if price format is incorrect
            if (price.Length != 6)
            {
                throw new ArrayTypeMismatchException("The price must be int[6]");
            }

            //Check if money format is incorrect
            if (money.Length != 6)
            {
                throw new ArrayTypeMismatchException("The money must be int[6]");
            }

            Console.WriteLine("Player : ");
            for (int i = 0; i < money.Length; i++)
            {
                Console.WriteLine((Enum.GetName(typeof(Ressources), i)).ToString() + " : " + money[i]);
            }

            Console.WriteLine("");

            Console.WriteLine("Card : ");
            for (int i = 0; i < price.Length; i++)
            {
                Console.WriteLine((Enum.GetName(typeof(Ressources), i)).ToString() + " : " + price[i]);
            }

            for (int i = 0; i < price.Length; i++)
            {
                if (money[i] < price[i])
                {
                    //Console.WriteLine("money["+i+"] : " + money[i] + "   price ["+i+"] : " + price[i]);
                    return false;
                }
            }

            return true;
        }
    }
}
