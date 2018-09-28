using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{
    class Functions
    {
        /// <summary>
        /// Insert a player in the database
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>True if the insert succeed and false if the insert failed</returns>
        public bool AddPlayer(string playerName)
        {
            //Create a new player object


            //Generate an unique ID


            try
            {
                //Insert the player in the database
                return true;
            }
            catch
            {
                //The adding of the database failed
                return false;
            }
        }
    }
}
