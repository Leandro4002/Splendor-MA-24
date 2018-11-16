/**
 * \file      Tools.cs
 * \author    Leandro Saraiva Maia & Alexandre Baseia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     Set of functions used in multiple ways
 *
 * \details   Insert a complete description of the tools here
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
        public static bool CheckEnoughtToBuy(int[] money, int[] price, int[] discount)
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

            //Check if discount format is incorrect
            if (money.Length != 6)
            {
                throw new ArrayTypeMismatchException("The discount must be int[6]");
            }

            for (int i = 0; i < price.Length; i++)
            {
                if (money[i] + discount[i] < price[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if a noble want to go with a player. If there is one, add the noble index in a list
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>A list of the noble index that go to the player</returns>
        public static List<int> NobleGoingToPlayer(Player player, List<Card> cardList)
        {
            List<int> indexOfNobles = new List<int>();

            for(int i = 0; i < 4; i++)
            {
                if (!cardList[i].IsEmpty)
                {
                    if (Tools.CheckEnoughtToBuy(player.Ressources, cardList[i].Price, new int[] { 0, 0, 0, 0, 0, 0 }))
                    {
                        indexOfNobles.Add(i);
                    }
                }
            }

            return indexOfNobles;
        }
    }
}
