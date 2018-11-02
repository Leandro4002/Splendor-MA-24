using Microsoft.VisualBasic;
/**
 * \file      frmAddVideoGames.cs
 * \author    F. Andolfatto
 * \version   1.0
 * \date      August 22. 2018
 * \brief     Form to play.
 *
 * \details   This form enables to choose coins or cards to get ressources (precious stones) and prestige points 
 * to add and to play with other players
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Splendor
{
    /// <summary>
    /// Manages the form that enables to play with the Splendor
    /// </summary>
    public partial class frmSplendor : Form
    {
        //Used to store the number of coins selected for the current round of game
        private int nbRubis;
        private int nbOnyx;
        private int nbEmeraude;
        private int nbDiamant;
        private int nbSaphir;

        //Used to set new players into the game
        private string newPlayer;

        private int playerNumber = 0;

        private List<Card>[] cardLists = new List<Card>[4];

        private List<Player> players = new List<Player>();

        private System.Drawing.Color selectedCardColor = System.Drawing.Color.FromArgb(255, 102, 178, 255);

        private System.Drawing.Color unSelectedCardColor = System.Drawing.Color.FromArgb(255, 255, 255);

        //Used to store cards temporary
        private Card[] buffer = new Card[4];

        //Id of the player that is playing
        private int currentPlayerId;

        private bool isPlaying = false;

        //Boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;

        //Store the id of the choiced card
        private string choiceCard;

        //Connection to the database
        private ConnectionDB conn;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public frmSplendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the form and initialize data in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSplendor_Load(object sender, EventArgs e)
        {
            lblGoldCoin.Text = "5";

            lblDiamantCoin.Text = "7";
            lblEmeraudeCoin.Text = "7" ;
            lblOnyxCoin.Text = "7";
            lblRubisCoin.Text = "7";
            lblSaphirCoin.Text = "7";

            //Create object conn to call "ConnectionDB" class
            conn = new ConnectionDB();

            //Get the cards from DB and shuffle them
            cardLists[0] = Tools.Shuffle(conn.GetListCardAccordingToLevel(1));
            cardLists[1] = Tools.Shuffle(conn.GetListCardAccordingToLevel(2));
            cardLists[2] = Tools.Shuffle(conn.GetListCardAccordingToLevel(3));
            cardLists[3] = Tools.Shuffle(conn.GetListCardAccordingToLevel(4));
            
            //Display the fourth cards for each level
            #region First card display

            //Level 1
            txtLevel14.Text = cardLists[0][0].ToString();
            txtLevel13.Text = cardLists[0][1].ToString();
            txtLevel12.Text = cardLists[0][2].ToString();
            txtLevel11.Text = cardLists[0][3].ToString();

            //Level 2
            txtLevel24.Text = cardLists[1][0].ToString();
            txtLevel23.Text = cardLists[1][1].ToString();
            txtLevel22.Text = cardLists[1][2].ToString();
            txtLevel21.Text = cardLists[1][3].ToString();

            //Level 3
            txtLevel34.Text = cardLists[2][0].ToString();
            txtLevel33.Text = cardLists[2][1].ToString();
            txtLevel32.Text = cardLists[2][2].ToString();
            txtLevel31.Text = cardLists[2][3].ToString();

            //Level 4
            txtLevel44.Text = cardLists[3][0].ToString();
            txtLevel43.Text = cardLists[3][1].ToString();
            txtLevel42.Text = cardLists[3][2].ToString();
            txtLevel41.Text = cardLists[3][3].ToString();

            #endregion First card display

            this.Width = 680;
            this.Height = 540;

            enableClicLabel = false;

            lblChoiceDiamant.Visible = false;
            lblChoiceOnyx.Visible = false;
            lblChoiceRubis.Visible = false;
            lblChoiceSaphir.Visible = false;
            lblChoiceEmeraude.Visible = false;
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = false;

            //We wire the click on all cards to the same event
            //TO DO for all cards
            txtLevel11.Click += ClickOnCard;
            txtLevel12.Click += ClickOnCard;
            txtLevel13.Click += ClickOnCard;
            txtLevel21.Click += ClickOnCard;
            txtLevel22.Click += ClickOnCard;
            txtLevel23.Click += ClickOnCard;
            txtLevel24.Click += ClickOnCard;
            txtLevel31.Click += ClickOnCard;
            txtLevel32.Click += ClickOnCard;
            txtLevel33.Click += ClickOnCard;
            txtLevel34.Click += ClickOnCard;
            txtLevel41.Click += ClickOnCard;
            txtLevel42.Click += ClickOnCard;
            txtLevel43.Click += ClickOnCard;
            txtLevel44.Click += ClickOnCard;
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"
            //TO DO
        }

        /// <summary>
        /// Click on the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPlay_Click(object sender, EventArgs e)
        {
            if (!isPlaying) {
                //Initialize every player
                for (int i = 1; i < conn.getNumberOfPlayers() + 1; i++)
                    LoadPlayer(i);

                currentPlayerId = 1;

                //Test if there is at least 2 players
                if (players.Count() < 2)
                {
                    MessageBox.Show("il faut au moins 2 joueurs pour commencer une partie", "Erreur");
                    return;
                }

                enableClicLabel = true;
                cmdValidateChoice.Visible = true;

                changeTurn(1);

                Width = 680;
                Height = 780;

                isPlaying = true;
                cmdPlay.Enabled = false;
                cmdInsertPlayer.Enabled = false;
            }
        }

        /// <summary>
        /// Load data about a player and put this data in variable
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id) {

            string name = conn.GetPlayerName(id);

            Player player = new Player();
            player.Name = name;
            player.Id = id;
            player.Ressources = new int[] { 0, 0, 0, 0, 0 };
            player.Coins = new int[] { 0, 0, 0, 0, 0 };

            players.Add(player);
        }


        /// <summary>
        /// Click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {

            int numberOfDifferentGems = getNumberOfDifferentGems();
            int totalGems = nbRubis + nbEmeraude + nbOnyx + nbSaphir + nbDiamant;

            //If the selection is correct
            if (!(totalGems == 2 && numberOfDifferentGems == 1) && !(totalGems == 3 && numberOfDifferentGems == 3))
            {
                MessageBox.Show("3 options :\n-Choisir 3 gemmes de type différents\n-Choisir 2 gemmes du même type\n-Choisir une carte", "Sélection incorrecte");
                nbRubis = 0;
                nbEmeraude = 0;
                nbOnyx = 0;
                nbSaphir = 0;
                nbDiamant = 0;
                refreshChoiceDisplay();
                return;
            }


            //Sélection correcte
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = true;
            enableClicLabel = false;

            players[currentPlayerId - 1].Coins[0] += nbRubis;
            players[currentPlayerId - 1].Coins[1] += nbEmeraude;
            players[currentPlayerId - 1].Coins[2] += nbOnyx;
            players[currentPlayerId - 1].Coins[3] += nbSaphir;
            players[currentPlayerId - 1].Coins[4] += nbDiamant;

            refreshPlayerDisplay(currentPlayerId - 1);

            //TO DO Check if card or coins are selected, impossible to do both at the same time
        }

        /// <summary>

        /// click on the insert button to insert player in the game
        /// Check for the field completion
        /// This function also show a message box to ensure that the player has been created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            newPlayer = Interaction.InputBox("Enter the name ", "Add player", "", 500, 500);
            //call the object "conn" to send player name's string to DB
            conn.AddPlayer(newPlayer);
            if (!string.IsNullOrWhiteSpace(newPlayer)) //if the field is empty or only contain white spaces
            {
                // Displays a MessageBox to inform that the player is done.
                MessageBox.Show("The player " + newPlayer + " has been added");
                playerNumber++;
                lblPlayerNumber.Text = "Nombre de joueurs : " + playerNumber;
            }
            else
            {
                MessageBox.Show("It seem that there is no name in the field, try again", "Empty name field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            //TO DO in release 1.0 : 3 is hard coded (number of players for the game), it shouldn't.
            //TO DO Get the id of the player : in release 0.1 there are only 3 players
            //Reload the data of the player
            //We are not allowed to click on the next button

            currentPlayerId++;

            int numberOfPlayer = conn.getNumberOfPlayers();

            if (currentPlayerId == numberOfPlayer + 1)
            {
                currentPlayerId = 1;
            }

            changeTurn(currentPlayerId);

            cmdValidateChoice.Visible = true;
            cmdNextPlayer.Visible = false;
            enableClicLabel = true;

            refreshPlayerDisplay(currentPlayerId - 1);
        }

        private int getNumberOfDifferentGems()
        {
            int val = 0;

            if (nbRubis > 0) val++;
            if (nbEmeraude > 0) val++;
            if (nbOnyx > 0) val++;
            if (nbSaphir > 0) val++;
            if (nbDiamant > 0) val++;

            return val;
        }

        #region private methods
        private void changeTurn(int id)
        {
            id--;

            //No coins or card selected yet, labels are empty
            lblChoiceDiamant.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphir.Text = "";
            lblChoiceEmeraude.Text = "";

            lblChoiceCard.Text = "";

            //Selected coins
            nbDiamant = 0;
            nbOnyx = 0;
            nbRubis = 0;
            nbSaphir = 0;
            nbEmeraude = 0;

            //Draw the player's coins
            refreshPlayerDisplay(id);

            lblPlayer.Text = "Au tour de "+ players[id].Name + " ["+id+"]";
            return;
        }

        private void gemClick(int id)
        {
            if (enableClicLabel)
            {
                switch (id)
                {
                    case 1: nbRubis++; break;
                    case 2: nbEmeraude++; break;
                    case 3: nbOnyx++; break;
                    case 4: nbSaphir++; break;
                    case 5: nbDiamant++; break;
                    default: return;
                }

                refreshChoiceDisplay(id);
            }
        }

        /// <summary>
        /// Select a card and highlight it if it is selectable
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <returns>bool if the card is selectable</returns>
        private bool selectCard(int level, int val)
        {
            if (enableClicLabel)
            {
                //cardLists[level - 1][val - 1];

                //Refresh the selected card
                choiceCard = level.ToString() + val.ToString();

                SelectedCardHiglightClear();

                refreshChoiceDisplay(0);
                return true;
            }
            return false;
        }

        #region refreshDisplay
        private void refreshChoiceDisplay(int id = 0)
        {
            switch (id)
            {
                case 0://Refresh all
                    lblChoiceRubis.Visible = (nbRubis > 0) ? true : false;
                    lblChoiceEmeraude.Visible = (nbEmeraude > 0) ? true : false;
                    lblChoiceOnyx.Visible = (nbOnyx > 0) ? true : false;
                    lblChoiceSaphir.Visible = (nbSaphir > 0) ? true : false;
                    lblChoiceDiamant.Visible = (nbDiamant > 0) ? true : false;
                    lblChoiceRubis.Text = nbRubis + "\r\n";
                    lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                    lblChoiceOnyx.Text = nbOnyx + "\r\n";
                    lblChoiceSaphir.Text = nbSaphir + "\r\n";
                    lblChoiceDiamant.Text = nbDiamant + "\r\n";
                    break;
                case 1://Refresh Rubis
                    lblChoiceRubis.Visible = (nbRubis > 0) ? true : false;
                    lblChoiceRubis.Text = nbRubis + "\r\n";
                    break;
                case 2://Refresh Emeraude
                    lblChoiceEmeraude.Visible = (nbEmeraude > 0) ? true : false;
                    lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                    break;
                case 3://Refresh Onyx
                    lblChoiceOnyx.Visible = (nbOnyx > 0) ? true : false;
                    lblChoiceOnyx.Text = nbOnyx + "\r\n";
                    break;
                case 4://Refresh Saphir
                    lblChoiceSaphir.Visible = (nbSaphir > 0) ? true : false;
                    lblChoiceSaphir.Text = nbSaphir + "\r\n";
                    break;
                case 5://Refresh Diamant
                    lblChoiceDiamant.Visible = (nbDiamant > 0) ? true : false;
                    lblChoiceDiamant.Text = nbDiamant + "\r\n";
                    break;
                case 6://Refresh Card
                    lblChoiceDiamant.Visible = true;
                    lblChoiceCard.Text = choiceCard;
                    break;
                default: break;
            }
        }

        private void refreshPlayerDisplay(int playerId)
        {
            lblPlayerRubisCoin.Text = players[playerId].Coins[0].ToString();
            lblPlayerEmeraudeCoin.Text = players[playerId].Coins[1].ToString();
            lblPlayerOnyxCoin.Text = players[playerId].Coins[2].ToString();
            lblPlayerSaphirCoin.Text = players[playerId].Coins[3].ToString();
            lblPlayerDiamantCoin.Text = players[playerId].Coins[4].ToString();
            lblNbPtPrestige.Text = players[playerId].GetPrestigeScore();
        }
        #endregion refreshDisplay

        //Remove the highlight of the cards
        private void SelectedCardHiglightClear()
        {
            txtLevel44.BackColor = unSelectedCardColor;
            txtLevel43.BackColor = unSelectedCardColor;
            txtLevel42.BackColor = unSelectedCardColor;
            txtLevel41.BackColor = unSelectedCardColor;

            txtLevel34.BackColor = unSelectedCardColor;
            txtLevel33.BackColor = unSelectedCardColor;
            txtLevel32.BackColor = unSelectedCardColor;
            txtLevel31.BackColor = unSelectedCardColor;

            txtLevel24.BackColor = unSelectedCardColor;
            txtLevel23.BackColor = unSelectedCardColor;
            txtLevel22.BackColor = unSelectedCardColor;
            txtLevel21.BackColor = unSelectedCardColor;

            txtLevel14.BackColor = unSelectedCardColor;
            txtLevel13.BackColor = unSelectedCardColor;
            txtLevel12.BackColor = unSelectedCardColor;
            txtLevel11.BackColor = unSelectedCardColor;
        }

        #endregion private methods

        #region Click on gems
        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Rubis);
        }

        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Saphir);
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Onyx);
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Emeraude);
        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Diamant);
        }
        #endregion Click on gems

        #region Click on cards

        private void txtLevel44_Click(object sender, EventArgs e)
        {
            selectCard(4, 4);

            //Change txt back color if the selection is active
            txtLevel44.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel43_Click(object sender, EventArgs e)
        {
            selectCard(4, 3);

            //Change txt back color if the selection is active
            txtLevel43.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel42_Click(object sender, EventArgs e)
        {
            selectCard(4, 2);

            //Change txt back color if the selection is active
            txtLevel42.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel41_Click(object sender, EventArgs e)
        {
            selectCard(4, 1);

            //Change txt back color if the selection is active
            txtLevel41.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel34_Click(object sender, EventArgs e)
        {
            selectCard(3, 4);

            //Change txt back color if the selection is active
            txtLevel34.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel33_Click(object sender, EventArgs e)
        {
            selectCard(3, 3);

            //Change txt back color if the selection is active
            txtLevel33.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel32_Click(object sender, EventArgs e)
        {
            selectCard(3, 2);

            //Change txt back color if the selection is active
            txtLevel32.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel31_Click(object sender, EventArgs e)
        {
            selectCard(3, 1);

            //Change txt back color if the selection is active
            txtLevel31.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel24_Click(object sender, EventArgs e)
        {
            selectCard(2, 4);

            //Change txt back color if the selection is active
            txtLevel24.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel23_Click(object sender, EventArgs e)
        {
            selectCard(2, 3);

            //Change txt back color if the selection is active
            txtLevel23.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel22_Click(object sender, EventArgs e)
        {
            selectCard(2, 2);

            //Change txt back color if the selection is active
            txtLevel22.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel21_Click(object sender, EventArgs e)
        {
            selectCard(2, 1);

            //Change txt back color if the selection is active
            txtLevel21.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel14_Click(object sender, EventArgs e)
        {
            selectCard(1, 4);

            //Change txt back color if the selection is active
            txtLevel14.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel13_Click(object sender, EventArgs e)
        {
            selectCard(1, 3);

            //Change txt back color if the selection is active
            txtLevel13.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel12_Click(object sender, EventArgs e)
        {
            selectCard(1, 2);

            //Change txt back color if the selection is active
            txtLevel12.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }

        private void txtLevel11_Click(object sender, EventArgs e)
        {
            selectCard(1, 1);

            //Change txt back color if the selection is active
            txtLevel11.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
        }
        #endregion Click on cards
    }
}
