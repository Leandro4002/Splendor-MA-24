/**
 * \file      frmSplendor.cs
 * \author    Leandro Saraiva Maia & Alexandre Baseia
 * \version   1.0
 * \date      September 14. 2018
 * \brief     The famous Splendor card game
 *
 * \details   Insert a complete description of the program here
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

        //Used to display the rules
        string rulesText = "Pour déselectionner une gemme, cliquez dessus dans la section \"Choix actuel\" \n" +
        "Pour déselectionner une carte, recliquez dessus\n" +
        "\n" +
        "3 options :\n" +
        "-Choisir 3 gemmes de type différents\n" +
        "-Choisir 2 gemmes du même type\n" +
        "-Choisir une carte\n" +
        "\n" +
        "Autres infos :\n" +
        "-Vous ne pouvez pas sélectionnez une gemme qui a moins de 4 unités dans la banque\n" +
        "-Les ressources acquises en achentant des cartes permettent d'obtenir une réduction sur le prix des cartes";

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

        //Contains all the cards
        private List<Card>[] cardLists = new List<Card>[4];

        //Player
        private List<Player> players = new List<Player>();
        private bool isPlaying = false;
        private int currentPlayerId; //Id of the player that is playing
        private int selectedCardLevel;
        private int selectedCardVal;

        //Card selection
        private Card selectedCard;
        private bool isCardSelected;
        private System.Drawing.Color selectedCardColor = System.Drawing.Color.FromArgb(255, 102, 178, 255);
        private System.Drawing.Color unSelectedCardColor = System.Drawing.Color.FromArgb(255, 255, 255);

        //Boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;

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

            RefreshCardsDisplay();

            this.Width = 680;
            this.Height = 510;

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

        #region Buttons click

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
                    MessageBox.Show("il faut au moins 2 joueurs pour commencer une partie", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                enableClicLabel = true;
                cmdValidateChoice.Visible = true;

                RefreshTurnDisplay(1);

                Width = 680;
                Height = 710;

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
            //If a card is selected and there is no gems selected, the program ignore the card selection
            if (!(isCardSelected && totalGems == 0))
            {
                //If the gems selection is incorrect
                if (!(totalGems == 2 && GetNumberOfDifferentGems() == 1) && !(totalGems == 3 && GetNumberOfDifferentGems() == 3))
                {
                    SelectionError();
                    return;
                }
                //If the gems selection is correct
                else
                {
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
                }
            }
            //If the card is selected and there is no gems selected
            else
            {
                //The selected card is picked (replace by another same level card and added to the player ressources)
                PickCard(selectedCard, currentPlayerId - 1, selectedCardLevel, selectedCardVal);
            }

            CheckIfPlayerWin(players[currentPlayerId - 1]);

            RefreshBankDisplay();

            //Correct selection
            cmdValidateChoice.Visible = false;
            SelectedCardHighlightClear();
            cmdNextPlayer.Visible = true;
            enableClicLabel = false;
            isCardSelected = false;

            RefreshPlayerDisplay(currentPlayerId - 1);
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
            newPlayer = Interaction.InputBox("Entrez un nom : ", "Ajouter un joueur", "", 500, 500);
            
            if (!string.IsNullOrWhiteSpace(newPlayer)) //if the field is empty or only contain white spaces
            {
                //Displays a MessageBox to inform that the player is done.
                MessageBox.Show("Le joueur " + newPlayer + " a été ajouté", "Ajout joueur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                playerNumber++;
                lblPlayerNumber.Text = "Nombre de joueurs : " + playerNumber;
                conn.AddPlayer(newPlayer);
            }
            else
            {
                MessageBox.Show("Veuillez entrer un joueur", "Ajout de joueur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            currentPlayerId++;

            int numberOfPlayer = conn.GetNumberOfPlayers();

            if (currentPlayerId == numberOfPlayer + 1)
            {
                currentPlayerId = 1;
            }

            //Move the nobles that can go to the player
            foreach (int index in Tools.NobleGoingToPlayer(players[currentPlayerId - 1], cardLists[3]))
            {
                Card nobleGoingToPlayer = cardLists[3][index];

                //If there is more cards after the one going to the player
                if (cardLists[3].Count > 4)
                {
                    cardLists[3][index] = cardLists[3][4];

                    cardLists[3].RemoveAt(4);
                }
                //If there is no more cards
                else
                {
                    Card card = new Card();
                    card.IsEmpty = true;
                    cardLists[3][index] = card;
                }

                //Add the prestige score to the player
                players[currentPlayerId - 1].PrestigeScore += nobleGoingToPlayer.PrestigePt;

                MessageBox.Show("La carte noble numéro" + (index + 1) + " viens avec vous car vous avez assez de ressources\nVous gagnez " + nobleGoingToPlayer.PrestigePt + " point(s) de prestige !", "Un noble vient !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CheckIfPlayerWin(players[currentPlayerId - 1]);

            RefreshTurnDisplay(currentPlayerId);

            cmdValidateChoice.Visible = true;
            cmdNextPlayer.Visible = false;
            enableClicLabel = true;

            RefreshPlayerDisplay(currentPlayerId - 1);
        }

        /// <summary>
        /// Display a help message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(rulesText, "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion Buttons click

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
        /// Show an error saying that the selection is false and reset the actual selection (gems and [TODO : cards]).
        /// This method show all the rules
        /// </summary>
        private void SelectionError()
        {
            MessageBox.Show("Vous ne pouvez pas faire cette action, voici un rappel des règles :\n\n" + rulesText, "Sélection incorrecte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            isCardSelected = false;
            SelectedCardHighlightClear();
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
                //If the player click on an already selected card, unselected it
                if (selectedCardLevel == level && selectedCardVal == val && isCardSelected)
                {
                    SelectedCardHighlightClear();
                    isCardSelected = false;
                    return false;
                }

                //Set the selected card
                selectedCard = cardLists[level - 1][val - 1];

                //Test if the card is empty
                if (selectedCard.IsEmpty)
                {
                    MessageBox.Show("Il n'y a plus assez de cartes de ce niveau !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (Tools.CheckEnoughtToBuy(players[currentPlayerId - 1].Coins, selectedCard.Price, players[currentPlayerId - 1].Ressources))
                {
                    isCardSelected = true;

                    //Values saved in memory to be used in another part of the code
                    selectedCardLevel = level;
                    selectedCardVal = val;

                    RefreshChoiceDisplay(6);

                    //Clear the highlight of all cards (the selected card will be highlighted just after)
                    SelectedCardHighlightClear();

                    return true;
                }
                else
                {
                    isCardSelected = false;
                    MessageBox.Show("Vous n'avez pas assez de gemmes pour acheter cette carte", "Selection gemme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SelectedCardHighlightClear();
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
        private void PickCard(Card pickedCard, int playerId, int level, int val)
        {
            if (cardLists[level - 1].Count > 4)
            {
                cardLists[level - 1][val - 1] = cardLists[level - 1][4];

                cardLists[level - 1].RemoveAt(4);
            }
            //If there is no more cards
            else
            {
                Card card = new Card();
                card.IsEmpty = true;
                cardLists[level - 1][val - 1] = card;
            }

            RefreshCardsDisplay();

            //Add the ressource to the player
            players[playerId].Ressources[(int)pickedCard.Ress] ++;

            //Add the prestige to the player
            players[playerId].PrestigeScore += pickedCard.PrestigePt;

            CheckIfPlayerWin(players[playerId]);

            int[] neededGems = { 0, 0, 0, 0, 0, 0 };

            //Get all the gems needed by the player to pay the card
            for (int i = 0; i < pickedCard.Price.Length; i++)
            {
                int discount = pickedCard.Price[i] - players[playerId].Ressources[i];

                if (0 < discount)
                {
                    neededGems[i] = discount;
                }
            }

            //Add the gems to the bank
            gemsInBank[0] += neededGems[0];
            gemsInBank[1] += neededGems[1];
            gemsInBank[2] += neededGems[2];
            gemsInBank[3] += neededGems[3];
            gemsInBank[4] += neededGems[4];
            gemsInBank[5] += neededGems[5];

            //Remove the player's gems (if he paid)
            players[playerId].Coins[0] -= neededGems[0];
            players[playerId].Coins[1] -= neededGems[1];
            players[playerId].Coins[2] -= neededGems[2];
            players[playerId].Coins[3] -= neededGems[3];
            players[playerId].Coins[4] -= neededGems[4];
            players[playerId].Coins[5] -= neededGems[5];
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

        /// <summary>
        /// If the player win, show a message and exit the application
        /// </summary>
        /// <param name="player"></param>
        private void CheckIfPlayerWin(Player player)
        {
            if(player.PrestigeScore >= 15)
            {
                MessageBox.Show("Le joueur " + player.Name + " a gagné\nFélicitations !", "Fin du jeu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
        }

        #endregion Private methods

        #region Refresh display

        /// <summary>
        /// Display the fourth cards for each level
        /// </summary>
        private void RefreshCardsDisplay()
        {
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
        }

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
            lblNbPtPrestige.Text = "Prestige : " + players[playerId].PrestigeScore.ToString();

            string playerRessources = "";
            
            if (players[playerId].Ressources[1] > 0) playerRessources += "Rubis : " + players[playerId].Ressources[1] + Environment.NewLine;
            if (players[playerId].Ressources[2] > 0) playerRessources += "Emeraude : " + players[playerId].Ressources[2] + Environment.NewLine;
            if (players[playerId].Ressources[3] > 0) playerRessources += "Onyx : " + players[playerId].Ressources[3] + Environment.NewLine;
            if (players[playerId].Ressources[4] > 0) playerRessources += "Saphir : " + players[playerId].Ressources[4] + Environment.NewLine;
            if (players[playerId].Ressources[5] > 0) playerRessources += "Diamant : " + players[playerId].Ressources[5] + Environment.NewLine;

            //Deletes the last character (to not have empty line)
            if (playerRessources.Length > 0) playerRessources = playerRessources.Remove(playerRessources.Length - 1);

            txtPlayerRessources.Text = playerRessources;
        }

        #endregion Refresh display

        #region Click on gems
        
        /// <summary>
        /// Handle the click on the gems in the bank
        /// </summary>
        /// <param name="id"></param>
        private void gemClickBank(int id)
        {
            if (enableClicLabel)
            {
                //Test if the gem selection is correct
                if (GetNumberOfDifferentGems() > 2 || totalGems > 2 || gemsInBank[id] < 4)
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
        /// Handle the click on the gems in the player's choice
        /// </summary>
        /// <param name="id"></param>
        private void gemClickChoice(int id)
        {
            if (enableClicLabel)
            {
                switch (id)
                {
                    case 1:
                        if (nbRubis > 0) nbRubis--;
                        lblRubisCoin.Text = (gemsInBank[1] - nbRubis).ToString();
                        break;
                    case 2:
                        if (nbEmeraude > 0) nbEmeraude--;
                        lblEmeraudeCoin.Text = (gemsInBank[2] - nbEmeraude).ToString();
                        break;
                    case 3:
                        if (nbOnyx > 0) nbOnyx--;
                        lblOnyxCoin.Text = (gemsInBank[3] - nbOnyx).ToString();
                        break;
                    case 4:
                        if (nbSaphir > 0) nbSaphir--;
                        lblSaphirCoin.Text = (gemsInBank[4] - nbSaphir).ToString();
                        break;
                    case 5:
                        if (nbDiamant > 0) nbDiamant--;
                        lblDiamantCoin.Text = (gemsInBank[5] - nbDiamant).ToString();
                        break;
                    default: return;
                }

                RefreshChoiceDisplay(id);
            }
        }

        //Bank gems

        /// <summary>
        /// Click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            gemClickBank((int)Ressources.Rubis);
        }

        /// <summary>
        /// Click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            gemClickBank((int)Ressources.Emeraude);
        }

        /// <summary>
        /// Click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            gemClickBank((int)Ressources.Onyx);
        }

        /// <summary>
        /// Click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            gemClickBank((int)Ressources.Saphir);
        }

        /// <summary>
        /// Click on the white coin (diamant) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamantCoin_Click(object sender, EventArgs e)
        {
            gemClickBank((int)Ressources.Diamant);
        }

        //Choice gems

        /// <summary>
        /// Click on the red coin choice (rubis)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            gemClickChoice((int)Ressources.Rubis);
        }

        /// <summary>
        /// Click on the green coin choice (emeraude)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            gemClickChoice((int)Ressources.Emeraude);
        }

        /// <summary>
        /// Click on the black coin choice (onyx)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            gemClickChoice((int)Ressources.Onyx);
        }

        /// <summary>
        /// Click on the blue coin choice (saphir)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            gemClickChoice((int)Ressources.Saphir);
        }

        /// <summary>
        /// Click on the white coin choice (diamant)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceDiamant_Click(object sender, EventArgs e)
        {
            gemClickChoice((int)Ressources.Diamant);
        }

        #endregion Click on gems

        #region Click on cards

        //Event when a noble card is selected
        private void ClickOnNobleCard(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                //Show a message that to tell that a noble card is not selectable
                MessageBox.Show("Vous ne pouvez pas sélectionner des cartes nobles\nElles viendront a vous quand vous aurez les ressources nécessaires", "Nobles", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
