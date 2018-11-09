/**
 * \file      Program.cs
 * \author    Leandro Saraiva Maia & Alexandre Baseia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     Entry point of the program
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splendor
{
    static class Program
    {
        /// <summary>
        /// Principal entry point of the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSplendor());
        }
    }
}
