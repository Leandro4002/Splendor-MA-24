using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{

    /// <summary>
    /// class Player : attributes and methods to deal with a player
    /// </summary>
    class Player
    {
        #region private attributes
        private string name;
        private int id;
        private int[] ressources;
        private int[] coins;
        #endregion private attributes

        #region public accessor
        /// <summary>
        /// name of the player
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// all the precious stones he has
        /// </summary>
        public int[] Ressources
        {
            get
            {
                return ressources;
            }
            set
            {
                ressources = value;
            }
        }

        /// <summary>
        /// all the coins he has
        /// </summary>
        public int[] Coins
        {
            get
            {
                return coins;
            }
            set
            {
                coins = value;
            }
        }

        /// <summary>
        /// id of the player
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        #endregion public accessor

        #region private methods
        /// <summary>
        /// The player takes gems from the bank
        /// </summary>
        /// <returns></returns>
        private static bool TakeRessourcesFromBank()
        {
            try {
            //If the player takes 2 gems of the same type

            //If there is 4 or less gems of the same type in the bank 
                //Refuse the action
                return false;
            //Else if there is 5 or more gem of the same type in the bank
                //Transfer the gems
                return true;
            //

            //If the player tales 3 gems of differents types each
                //If the player wants to take gold gem
                return false;

                //if one of the selected gem is not in the bank (equal 0)
                return false;

                //Transfer the gems
                return true;
            //
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The player takes a card from the bank
        /// </summary>
        /// <returns></returns>
        private static bool TakeCardFromBank()
        {
            try
            {
                //If the player have all the necessary ressources to take the card
                    //Transfer the player's ressources to the bank
                    return true;
                //else
                    return true;
                //
            }
            catch
            {
                return false;
            }
        }

        private static bool ReserveCard()
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion private methods
    }
}
