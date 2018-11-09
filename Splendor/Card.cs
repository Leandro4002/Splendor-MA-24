/**
 * \file      Card.cs
 * \author    Leandro Saraiva Maia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     The model of card used in the game
 *
 * \details   Insert a complete description of the card here
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{
    /// <summary>
    /// Class Card : attributes and methods to deal with a card
    /// </summary>
    class Card
    {
        #region Private attributes

        private Ressources ress;
        private int prestigePt;
        private int level;
        private int id;
        private int[] price = new int[6];

        #endregion Private attributes

        #region Public attributes

        /// <summary>
        /// The precious stone that the card gives
        /// </summary>
        public Ressources Ress
        {
            get
            {
                return ress;
            }
            set
            {
                ress = value;
            }

        }

        /// <summary>
        /// Number of prestige point of the card
        /// </summary>
        public int PrestigePt
        {
            get
            {
                return prestigePt;
            }
            set
            {
                prestigePt = value;
            }

        }

        /// <summary>
        /// Level of the card : 1, 2 or 3
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        /// <summary>
        /// Id of the card in the database
        /// </summary>
        public int Id {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// All the precious stones that are needed to buy the card
        /// </summary>
        public int[] Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        #endregion Public attributes

        #region Public methods

        /// <summary>
        /// Displays information about the card
        /// </summary>
        /// <returns>A representation of the card in a string</returns>
        public override string ToString()
        {
            string res = "";
            //If there is a prestige score, write it down
            if (prestigePt != 0)
            {
                res += "[" + prestigePt + "]";
            }

            //Write the name of the ressource of the card, if it exists (noble for example)
            if (Ress != 0)
            {
                res += " " + Enum.GetName(typeof(Ressources), Ress);
            } 
            
            res += "\t";
            res += "\r\n\r\n";
            int loop = 0;
            
            foreach (int i in price)
            {
                
                string ressource = "";

                //If there is a ressource, write it down
                if (i != 0)
                {
                    ressource = "    ";
                    ressource += Enum.GetName(typeof(Ressources), loop) + " ";
                    ressource += i + "\r\n";
                }
                
                res += ressource;
                loop++;

            }

            return res;
        }

        #endregion Public methods
    }
}