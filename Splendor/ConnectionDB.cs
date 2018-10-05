using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Splendor
{
    /// <summary>
    /// Contains methods and attributes to connect and deal with the database
    /// </summary>
    class ConnectionDB
    {
        //Connection to the database
        private SQLiteConnection m_dbConnection;

        /// <summary>
        /// Constructor : Creates the connection to the database SQLite
        /// </summary>
        public ConnectionDB()
        {

            SQLiteConnection.CreateFile("Splendor.sqlite");

            m_dbConnection = new SQLiteConnection("Data Source=Splendor.sqlite;Version=3;");
            m_dbConnection.Open();

            //Create the tables and insert data
            CreateInsertRessources();

            CreateInsertPlayer();

            CreateInsertNbCoin();

            CreateInsertCards();

            CreateInsertCost();
        }

        #region Create and insert data

        /// <summary>
        /// Create the "Player" table and insert data (Hardcoded (temp))
        /// </summary>
        private void CreateInsertPlayer()
        {
            string sql = "CREATE TABLE Player (id INTEGER PRIMARY KEY, pseudo VARCHAR(20));";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create the "Cost" table and insert data
        /// </summary>
        private void CreateInsertCost()
        {
            string[] queries = {
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(1, 6, 1, 4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(2, 7, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(3, 9, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(4, 11, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(5, 13, 1, 7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(6, 14, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(7, 15, 1, 5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(8, 16, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(9, 23, 1, 7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(10, 25, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(11, 27, 1, 6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(12, 29, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(13, 30, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(14, 31, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(15, 32, 1, 5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(16, 33, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(17, 34, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(18, 35, 1, 4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(19, 36, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(20, 38, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(21, 39, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(22, 42, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(23, 48, 1, 6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(24, 51, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(25, 53, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(26, 57, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(27, 59, 1, 5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(28, 62, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(29, 63, 1, 3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(30, 64, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(31, 66, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(32, 67, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(33, 70, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(34, 72, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(35, 76, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(36, 81, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(37, 84, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(38, 85, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(39, 86, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(40, 88, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(41, 91, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(42, 93, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(43, 94, 1, 4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(44, 96, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(45, 97, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(46, 98, 1, 2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values(47, 100, 1, 1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (48,2,2,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (49,3,2,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (50,8,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (51,9,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (52,11,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (53,15,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (54,16,2,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (55,17,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (56,20,2,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (57,22,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (58,24,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (59,25,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (60,27,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (61,29,2,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (62,31,2,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (63,34,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (64,35,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (65,37,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (66,39,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (67,41,2,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (68,42,2,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (69,47,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (70,49,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (71,51,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (72,55,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (73,57,2,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (74,58,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (75,60,2,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (76,62,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (77,66,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (78,70,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (79,71,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (80,72,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (81,73,2,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (82,74,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (83,77,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (84,78,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (85,79,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (86,82,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (87,83,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (88,84,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (89,85,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (90,86,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (91,88,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (92,91,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (93,92,2,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (94,93,2,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (95,95,2,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (96,4,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (97,5,3,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (98,6,3,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (99,7,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (100,11,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (101,13,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (102,14,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (103,15,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (104,18,3,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (105,19,3,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (106,21,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (107,24,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (108,25,3,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (109,27,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (110,30,3,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (111,33,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (112,34,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (113,35,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (114,38,3,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (115,40,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (116,43,3,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (117,46,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (118,47,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (119,49,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (120,53,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (121,54,3,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (122,59,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (123,61,3,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (124,62,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (125,64,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (126,65,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (127,66,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (128,67,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (129,68,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (130,70,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (131,71,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (132,72,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (133,74,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (134,78,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (135,79,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (136,88,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (137,89,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (138,90,3,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (139,92,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (140,96,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (141,97,3,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (142,100,3,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (143,2,4,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (144,4,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (145,8,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (146,9,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (147,10,4,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (148,12,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (149,14,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (150,15,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (151,16,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (152,17,4,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (153,21,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (154,22,4,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (155,24,4,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (156,26,4,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (157,31,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (158,33,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (159,36,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (160,37,4,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (161,39,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (162,40,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (163,45,4,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (164,46,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (165,49,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (166,52,4,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (167,55,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (168,56,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (169,57,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (170,58,4,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (171,65,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (172,68,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (173,69,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (174,70,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (175,71,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (176,72,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (177,74,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (178,77,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (179,79,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (180,81,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (181,85,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (182,86,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (183,87,4,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (184,93,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (185,95,4,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (186,96,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (187,97,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (188,98,4,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (189,99,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (190,100,4,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (191,101,4,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (192,4,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (193,5,5,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (194,7,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (195,8,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (196,10,5,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (197,12,5,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (198,14,5,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (199,17,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (200,19,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (201,21,5,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (202,24,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (203,25,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (204,28,5,7)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (205,30,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (206,31,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (207,36,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (208,38,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (209,40,5,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (210,43,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (211,44,5,6)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (212,46,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (213,47,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (214,50,5,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (215,51,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (216,53,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (217,55,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (218,56,5,5)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (219,58,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (220,64,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (221,65,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (222,66,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (223,74,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (224,75,5,3)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (225,76,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (226,78,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (227,79,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (228,80,5,4)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (229,81,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (230,82,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (231,85,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (232,86,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (233,88,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (234,89,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (235,91,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (236,95,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (237,97,5,1)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (238,99,5,2)",
            "insert into Cost(idCost, fkCard, fkRessource, nbRessource) values (239,100,5,1)"
            };

            //Create the table that contains costs
            string sql = "CREATE TABLE Cost (idCost INT PRIMARY KEY, fkCard INT, fkRessource INT, nbRessource INT, FOREIGN KEY (fkCard) REFERENCES Card(idCard), FOREIGN KEY (fkRessource) REFERENCES Ressource(idRessource))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            //Execute all the cost insert queries
            foreach (string query in queries)
            {
                command = new SQLiteCommand(query, m_dbConnection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create the "Ressources" table and insert data
        /// </summary>
        private void CreateInsertRessources()
        {
            SQLiteCommand command = new SQLiteCommand(
                "CREATE TABLE Ressource (idRessource INT PRIMARY KEY);" +
                "insert into Ressource(idRessource) values(0);" +
                "insert into Ressource(idRessource) values(1);" +
                "insert into Ressource(idRessource) values(2);" +
                "insert into Ressource(idRessource) values(3);" +
                "insert into Ressource(idRessource) values(4);" +
                "insert into Ressource(idRessource) values(5);"
            , m_dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create the "NbCoin" table and insert data
        /// </summary>
        private void CreateInsertNbCoin()
        {
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE NbCoin (idNbCoin INT PRIMARY KEY , fkPlayer INT, fkRessource INT, nbCoin INT, FOREIGN KEY (fkPlayer) REFERENCES Player(idPlayer), FOREIGN KEY (fkRessource) REFERENCES Rssource(idRessource))",
            m_dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create the "Card" table and insert data
        /// </summary>
        private void CreateInsertCards()
        {
            string[] queries = {
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(2, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(3, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(4, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(5, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(6, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(7, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(8, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(9, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(10, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(11, 0, 4, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(12, 4, 3, 5)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(13, 3, 3, 5)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(14, 2, 3, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(15, 5, 3, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(16, 1, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(17, 2, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(18, 5, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(19, 5, 3, 5)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(20, 1, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(21, 4, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(22, 2, 3, 5)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(23, 3, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(24, 1, 3, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(25, 4, 3, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(26, 2, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(27, 3, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(28, 4, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(29, 1, 3, 5)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(30, 5, 3, 4)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(31, 3, 3, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(32, 5, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(33, 1, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(34, 5, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(35, 5, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(36, 5, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(37, 2, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(38, 4, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(39, 4, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(40, 2, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(41, 2, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(42, 3, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(43, 1, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(44, 5, 2, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(45, 4, 2, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(46, 2, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(47, 3, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(48, 1, 2, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(49, 4, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(50, 3, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(51, 2, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(52, 4, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(53, 1, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(54, 1, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(55, 3, 2, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(56, 4, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(57, 3, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(58, 1, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(59, 5, 2, 2)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(60, 2, 2, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(61, 3, 2, 3)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(62, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(63, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(64, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(65, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(66, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(67, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(68, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(69, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(70, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(71, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(72, 5, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(73, 5, 1, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(74, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(75, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(76, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(77, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(78, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(79, 1, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(80, 1, 1, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(81, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(82, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(83, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(84, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(85, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(86, 3, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(87, 3, 1, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(88, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(89, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(90, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(91, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(92, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(93, 4, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(94, 4, 1, 1)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(95, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(96, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(97, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(98, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(99, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(100, 2, 1, 0)",
            "insert into Card(idCard, fkRessource, level, nbPtPrestige) values(101, 2, 1, 1)"
            };

            //Create the table that contains cards
            string sql = "CREATE TABLE Card (idCard INT PRIMARY KEY, fkRessource INT, level INT, nbPtPrestige INT, FOREIGN KEY (fkRessource) REFERENCES Ressource(idRessource))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            //Execute all the card insert queries
            foreach (string query in queries)
            {
                command = new SQLiteCommand(query, m_dbConnection);
                command.ExecuteNonQuery();
            }
        }

        #endregion Create and insert data

        #region Get query

        /// <summary>
        /// add a new player to the DB
        /// that's an easter egg
        /// </summary>
        
        public void AddPlayer(string playerName)
        {
            string sql = "insert into player ( pseudo) values ('"+playerName+"')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get the name of the player according to his id
        /// </summary>
        /// <param name="id">Id of the player</param>
        /// <returns>Player name</returns>
        public string GetPlayerName(int id)
        {
            string sql = "SELECT pseudo FROM player WHERE id = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = reader["pseudo"].ToString();
            }
            return name;
        }

        /// <summary>
        /// Get the list of cards according to the level
        /// </summary>
        /// <returns>Cards stack</returns>
        public Stack<Card> GetListCardAccordingToLevel(int level)
        {
            Stack<Card> listCard = new Stack<Card>();

            string sql = "SELECT * FROM Card WHERE level = " + level;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Card card = new Card();
                card.PrestigePt = int.Parse(reader["nbPtPrestige"].ToString());
                card.Level = int.Parse(reader["level"].ToString());

                //Get the ressource of the card by casting to Ressource type the int value got by reading fkRessource
                card.Ress = (Ressources)int.Parse(reader["fkRessource"].ToString());

                string sql2 = "SELECT * FROM Cost WHERE fkCard = " + reader["idCard"];
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    card.Price[int.Parse(reader2["fkRessource"].ToString())] = int.Parse(reader2["nbRessource"].ToString());
                }

                listCard.Push(card);
            }

            return listCard;
        }

        /// <summary>
        /// Get the list of all the cards
        /// </summary>
        /// <returns>Cards stack</returns>
        public Stack<Card> GetListCard()
        {
            Stack<Card> listCard = new Stack<Card>();

            string sql = "SELECT * FROM Card";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Card card = new Card();
                card.PrestigePt = int.Parse(reader["nbPtPrestige"].ToString());
                card.Level = int.Parse(reader["level"].ToString());

                string sql2 = "SELECT * FROM Cost WHERE fkCard = " + reader["idCard"];
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    card.Price[int.Parse(reader["fkRessource"].ToString())] = int.Parse(reader2["nbRessource"].ToString());
                }

                listCard.Push(card);
            }

            return listCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Card GetCardById(int id)
        {
            string sql = "SELECT * FROM Card WHERE IdCard = " + id + " LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            Card card = new Card();

            while (reader.Read())
            {
                card.PrestigePt = int.Parse(reader["nbPtPrestige"].ToString());
                card.Level = int.Parse(reader["level"].ToString());

                string sql2 = "SELECT * FROM Cost WHERE fkCard = " + reader["idCard"];
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    card.Price[int.Parse(reader["fkRessource"].ToString())] = int.Parse(reader2["nbRessource"].ToString());
                }
            }

            return card;
        }

        #endregion Get query
    }
}