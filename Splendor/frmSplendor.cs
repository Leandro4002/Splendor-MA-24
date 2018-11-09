/**
 * \file      frmSplendor.cs
 * \author    Leandro Saraiva Maia & Alexandre Baseia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     The famous Splendor card game
 *
 * \details   This file used to handle the game mechanics, the label/buttons click and to refresh the display.
 *            
 *            First, the program needs at least 2 players to start the game, a player can be added by clicking the "Entrer joueur" button.
 *            To start a game, click on the "Jouer" button.
 *            During a game, the program alternate between every player turn.
 *            
 *            Possible actions during a turn :
 *            - Get 2 gems of the same type
 *            - Get 3 gems of different types
 *            - Select a card
 *            
 *            The first player with 15 prestige points win the game
 */

using Microsoft.VisualBasic;
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
        #region Private attributes

        private int[] gemsInBank = { 5, 7, 7, 7, 7, 7 };

        //Used to store the number of coins selected for the current round of game
        private int nbRubis;
        private int nbOnyx;
        private int nbEmeraude;
        private int nbDiamant;
        private int nbSaphir;

        private int totalGems => nbRubis + nbEmeraude + nbOnyx + nbSaphir + nbDiamant;

        //Used to set new players into the game
        private string newPlayer;
        private int playerNumber = 0;

        private List<Card>[] cardLists = new List<Card>[4];

        //Player
        private List<Player> players = new List<Player>();
        private bool isPlaying = false;
        private int currentPlayerId; //Id of the player that is playing

        //Card selection
        private Card selectedCard;
        private System.Drawing.Color selectedCardColor = System.Drawing.Color.FromArgb(255, 102, 178, 255);
        private System.Drawing.Color unSelectedCardColor = System.Drawing.Color.FromArgb(255, 255, 255);

        //Boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;

        //Store the emplacement id of the choiced card
        private string choiceCard;

        //Connection to the database
        private ConnectionDB conn;

        #endregion Private attributes

        /// <summary>
        /// Constructor
        /// </summary>
        public frmSplendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the form and initialize data in it
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
            txtLevel14.Text = cardLists[0][3].ToString();
            txtLevel13.Text = cardLists[0][2].ToString();
            txtLevel12.Text = cardLists[0][1].ToString();
            txtLevel11.Text = cardLists[0][0].ToString();

            //Level 2
            txtLevel24.Text = cardLists[1][3].ToString();
            txtLevel23.Text = cardLists[1][2].ToString();
            txtLevel22.Text = cardLists[1][1].ToString();
            txtLevel21.Text = cardLists[1][0].ToString();

            //Level 3
            txtLevel34.Text = cardLists[2][3].ToString();
            txtLevel33.Text = cardLists[2][2].ToString();
            txtLevel32.Text = cardLists[2][1].ToString();
            txtLevel31.Text = cardLists[2][0].ToString();

            //Level 4
            txtLevel44.Text = cardLists[3][3].ToString();
            txtLevel43.Text = cardLists[3][1].ToString();
            txtLevel42.Text = cardLists[3][1].ToString();
            txtLevel41.Text = cardLists[3][0].ToString();

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

            //We wire the click on noble cards to the same event
            txtLevel41.Click += ClickOnNobleCard;
            txtLevel42.Click += ClickOnNobleCard;
            txtLevel43.Click += ClickOnNobleCard;
            txtLevel44.Click += ClickOnNobleCard;
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
                for (int i = 1; i < conn.GetNumberOfPlayers() + 1; i++)
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

                RefreshTurnDisplay(1);

                Width = 680;
                Height = 780;

                isPlaying = true;
                cmdPlay.Enabled = false;
                cmdInsertPlayer.Enabled = false;
            }
        }

        /// <summary>
        /// Click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {

            //If the selection is incorrect
            if (!(totalGems == 2 && GetNumberOfDifferentGems() == 1) && !(totalGems == 3 && GetNumberOfDifferentGems() == 3))
            {
                SelectionError();
                return;
            }


            //Correct selection
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = true;
            enableClicLabel = false;

            //Add gems to player
            players[currentPlayerId - 1].Coins[1] += nbRubis;
            players[currentPlayerId - 1].Coins[2] += nbEmeraude;
            players[currentPlayerId - 1].Coins[3] += nbOnyx;
            players[currentPlayerId - 1].Coins[4] += nbSaphir;
            players[currentPlayerId - 1].Coins[5] += nbDiamant;

            //Delete gems from bank
            gemsInBank[1] -= nbRubis;
            gemsInBank[2] -= nbEmeraude;
            gemsInBank[3] -= nbOnyx;
            gemsInBank[4] -= nbSaphir;
            gemsInBank[5] -= nbDiamant;

            RefreshBankDisplay();

            RefreshPlayerDisplay(currentPlayerId - 1);

            //TO DO Check if card or coins are selected, impossible to do both at the same time
        }

        /// <summary>
        /// Click on the insert button to insert player in the game
        /// Check for the field completion
        /// This function also show a message box to ensure that the player has been created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            newPlayer = Interaction.InputBox("Enter the name ", "Add player", "", 500, 500);
            //Call the object "conn" to send player name's string to DB
            conn.AddPlayer(newPlayer);
            if (!string.IsNullOrWhiteSpace(newPlayer)) //if the field is empty or only contain white spaces
            {
                //Displays a MessageBox to inform that the player is done.
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

            int numberOfPlayer = conn.GetNumberOfPlayers();

            if (currentPlayerId == numberOfPlayer + 1)
            {
                currentPlayerId = 1;
            }

            RefreshTurnDisplay(currentPlayerId);

            cmdValidateChoice.Visible = true;
            cmdNextPlayer.Visible = false;
            enableClicLabel = true;

            RefreshPlayerDisplay(currentPlayerId - 1);
        }

        #region Private methods

        /// <summary>
        /// Load data about a player and put this data in variable
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id)
        {
            string name = conn.GetPlayerName(id);

            Player player = new Player();
            player.Name = name;
            player.Id = id;
            player.Ressources = new int[] { 0, 0, 0, 0, 0, 0 };
            player.Coins = new int[] { 0, 0, 0, 0, 0, 0 };

            players.Add(player);
        }

        /// <summary>
        /// Show an error saying that the selection is false and reset the actual selection (gems and [TODO : cards])
        /// </summary>
        private void SelectionError()
        {
            MessageBox.Show("Pour suprimmer une gemme sélectionnée cliquez dessus dans la ligne du bas\nPour suprimmer une carte sélectionné, recliquer dessus\n\n3 options :\n-Choisir 3 gemmes de type différents\n-Choisir 2 gemmes du même type\n-Choisir une carte", "Sélection incorrecte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            nbRubis = 0;
            nbEmeraude = 0;
            nbOnyx = 0;
            nbSaphir = 0;
            nbDiamant = 0;
            RefreshChoiceDisplay();
            RefreshBankDisplay();
        }

        /// <summary>
        /// Get the number of different gems type in the current player's turn choice
        /// </summary>
        /// <returns>Number of different gems</returns>
        private int GetNumberOfDifferentGems()
        {
            int val = 0;

            if (nbRubis > 0) val++;
            if (nbEmeraude > 0) val++;
            if (nbOnyx > 0) val++;
            if (nbSaphir > 0) val++;
            if (nbDiamant > 0) val++;

            return val;
        }

        /// <summary>
        /// Select a card and highlight it if it is selectable
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <returns>bool if the card is selectable</returns>
        private bool SelectCard(int level, int val)
        {
            if (enableClicLabel)
            {
                if (Tools.CheckEnoughtToBuy(players[currentPlayerId - 1].Coins, cardLists[level - 1][val - 1].Price))
                {
                    //Set the selected card
                    selectedCard = cardLists[level - 1][val - 1];

                    //Refresh the selected card display
                    choiceCard = (level - 1).ToString() + (val - 1).ToString();
                    RefreshChoiceDisplay(6);

                    //Clear the highlight of all cards (the selected card will be highlighted just after)
                    SelectedCardHighlightClear();

                    //The selected card is picked (replace by another same level card and added to the player ressources)
                    PickCard(cardLists[level - 1][val - 1], currentPlayerId - 1, level, val);

                    return true;
                }
                else
                {
                    MessageBox.Show("Vous n'avez pas assez de gemmes pour acheter cette carte");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace by another same level card and added to a player ressources
        /// </summary>
        /// <param name="card"></param>
        /// <param name="player"></param>
        private void PickCard(Card card, int playerId, int level, int val)
        {
            //Remove the card from the database
            conn.RemoveCardById(card.Id);

            List<Card> cardList = conn.GetListCardAccordingToLevel(card.Level);

            string replaceText;

            //If there is a card with the same level in the database
            if (cardList.Count > 0)
            {
                //Replace the display of the card
                replaceText = cardList[0].ToString();
            }
            //If there is no more card in the database
            else
            {
                //Clean the display of the card
                replaceText = "";
            }

            switch (level)
            {
                case 1:
                    switch (val)
                    {
                        case 1: txtLevel11.Text = replaceText; break;
                        case 2: txtLevel12.Text = replaceText; break;
                        case 3: txtLevel13.Text = replaceText; break;
                        case 4: txtLevel14.Text = replaceText; break;
                    }
                    break;
                case 2:
                    switch (val)
                    {
                        case 1: txtLevel21.Text = replaceText; break;
                        case 2: txtLevel22.Text = replaceText; break;
                        case 3: txtLevel23.Text = replaceText; break;
                        case 4: txtLevel24.Text = replaceText; break;
                    }
                    break;
                case 3:
                    switch (val)
                    {
                        case 1: txtLevel31.Text = replaceText; break;
                        case 2: txtLevel32.Text = replaceText; break;
                        case 3: txtLevel33.Text = replaceText; break;
                        case 4: txtLevel34.Text = replaceText; break;
                    }
                    break;
                case 4:
                    switch (val)
                    {
                        case 1: txtLevel41.Text = replaceText; break;
                        case 2: txtLevel42.Text = replaceText; break;
                        case 3: txtLevel43.Text = replaceText; break;
                        case 4: txtLevel44.Text = replaceText; break;
                    }
                    break;
            }

            //Add the ressource to the player
            players[playerId].Ressources[(int)card.Ress] ++;
        }

        /// <summary>
        /// Remove the highlight of all the cards
        /// </summary>
        private void SelectedCardHighlightClear()
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

        #endregion Private methods

        #region Refresh display

        /// <summary>
        /// Refresh the display related to a new turn
        /// </summary>
        /// <param name="id"></param>
        private void RefreshTurnDisplay(int id)
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
            RefreshPlayerDisplay(id);

            lblPlayer.Text = "Au tour de " + players[id].Name + " [" + id + "]";
        }

        /// <summary>
        /// Refresh the number of gems of the bank
        /// </summary>
        private void RefreshBankDisplay()
        {
            lblGoldCoin.Text = gemsInBank[0].ToString();
            lblRubisCoin.Text = gemsInBank[1].ToString();
            lblEmeraudeCoin.Text = gemsInBank[2].ToString();
            lblOnyxCoin.Text = gemsInBank[3].ToString();
            lblSaphirCoin.Text = gemsInBank[4].ToString();
            lblDiamantCoin.Text = gemsInBank[5].ToString();
        }

        /// <summary>
        /// Refresh the player's choice
        /// </summary>
        /// <param name="id"></param>
        private void RefreshChoiceDisplay(int id = 0)
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
                    lblChoiceCard.Visible = true;
                    lblChoiceCard.Text = choiceCard;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Refresh the player's ressources
        /// </summary>
        /// <param name="playerId"></param>
        private void RefreshPlayerDisplay(int playerId)
        {
            lblPlayerRubisCoin.Text = players[playerId].Coins[1].ToString();
            lblPlayerEmeraudeCoin.Text = players[playerId].Coins[2].ToString();
            lblPlayerOnyxCoin.Text = players[playerId].Coins[3].ToString();
            lblPlayerSaphirCoin.Text = players[playerId].Coins[4].ToString();
            lblPlayerDiamantCoin.Text = players[playerId].Coins[5].ToString();
            lblNbPtPrestige.Text = players[playerId].GetPrestigeScore();
        }

        #endregion Refresh display

        #region Click on gems
        
        /// <summary>
        /// Handle the click on the gems
        /// </summary>
        /// <param name="id"></param>
        private void gemClick(int id)
        {
            if (enableClicLabel)
            {
                if (GetNumberOfDifferentGems() > 2 || totalGems > 2)
                {
                    SelectionError();
                    return;
                }

                switch (id)
                {
                    case 1:
                        nbRubis++;
                        lblRubisCoin.Text = (gemsInBank[1] - nbRubis).ToString();
                        break;
                    case 2:
                        nbEmeraude++;
                        lblEmeraudeCoin.Text = (gemsInBank[2] - nbEmeraude).ToString();
                        break;
                    case 3:
                        nbOnyx++;
                        lblOnyxCoin.Text = (gemsInBank[3] - nbOnyx).ToString();
                        break;
                    case 4:
                        nbSaphir++;
                        lblSaphirCoin.Text = (gemsInBank[4] - nbSaphir).ToString();
                        break;
                    case 5:
                        nbDiamant++;
                        lblDiamantCoin.Text = (gemsInBank[5] - nbDiamant).ToString();
                        break;
                    default: return;
                }

                RefreshChoiceDisplay(id);
            }
        }

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
        /// click on the white coin (diamant) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamantCoin_Click(object sender, EventArgs e)
        {
            gemClick((int)Ressources.Diamant);
        }
        
        #endregion Click on gems

        #region Click on cards

        //Event when a noble card is selected
        private void ClickOnNobleCard(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                //Show a message that to tell that a noble card is not selectable
                MessageBox.Show("Vous ne pouvez pas sélectionner des cartes nobles\nElles viendront a vous quand vous aurez les ressources nécessaires");
            }
        }

        private void txtLevel34_Click(object sender, EventArgs e)
        {
            if (SelectCard(3, 4))
            {
                //Highlight the card if the selection is possible
                txtLevel34.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel33_Click(object sender, EventArgs e)
        {
            if (SelectCard(3, 3))
            {
                //Highlight the card if the selection is possible
                txtLevel33.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel32_Click(object sender, EventArgs e)
        {
            if (SelectCard(3, 2))
            {
                //Highlight the card if the selection is possible
                txtLevel32.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel31_Click(object sender, EventArgs e)
        {
            if (SelectCard(3, 1))
            {
                //Highlight the card if the selection is possible
                txtLevel31.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel24_Click(object sender, EventArgs e)
        {
            if (SelectCard(2, 4))
            {
                //Highlight the card if the selection is possible
                txtLevel24.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel23_Click(object sender, EventArgs e)
        {
            if (SelectCard(2, 3))
            {
                //Highlight the card if the selection is possible
                txtLevel23.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel22_Click(object sender, EventArgs e)
        {
            if (SelectCard(2, 2))
            {
                //Highlight the card if the selection is possible
                txtLevel22.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel21_Click(object sender, EventArgs e)
        {
            if (SelectCard(2, 1))
            {
                //Highlight the card if the selection is possible
                txtLevel21.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel14_Click(object sender, EventArgs e)
        {
            if (SelectCard(1, 4))
            {
                //Highlight the card if the selection is possible
                txtLevel14.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel13_Click(object sender, EventArgs e)
        {
            if (SelectCard(1, 3))
            {
                //Highlight the card if the selection is possible
                txtLevel13.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel12_Click(object sender, EventArgs e)
        {
            if (SelectCard(1, 2))
            {
                //Highlight the card if the selection is possible
                txtLevel12.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }

        private void txtLevel11_Click(object sender, EventArgs e)
        {
            if (SelectCard(1, 1))
            {
                //Highlight the card if the selection is possible
                txtLevel11.BackColor = (enableClicLabel) ? selectedCardColor : unSelectedCardColor;
            }
        }
        
        #endregion Click on cards
    }
}
