using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameCards : 
        PD_GameParts_Base, 
        ICustomDeepCopyable<PD_GameCards>
    {

        public List<List<int>> DividedDeckOfInfectionCards { get; private set; }
        public List<int> ActiveInfectionCards { get; private set; }
        public List<int> DeckOfDiscardedInfectionCards { get; private set; }
        public List<List<int>> DividedDeckOfPlayerCards { get; private set; }
        public List<int> DeckOfDiscardedPlayerCards { get; private set; }
        public Dictionary<int, List<int>> PlayerCardsPerPlayerID { get; private set; }
        public List<int> InactiveRoleCards { get; private set; }

        #region constructors
        // normal constructor
        public PD_GameCards(
            List<int> players,
            List<int> allRoleCards
            )
        {
            DividedDeckOfInfectionCards = new List<List<int>>();
            ActiveInfectionCards = new List<int>();
            DeckOfDiscardedInfectionCards = new List<int>();

            DividedDeckOfPlayerCards = new List<List<int>>();
            DeckOfDiscardedPlayerCards = new List<int>();
            PlayerCardsPerPlayerID = new Dictionary<int, List<int>>();
            foreach (var player in players)
                PlayerCardsPerPlayerID.Add(player, new List<int>());
            InactiveRoleCards = allRoleCards.CustomDeepCopy();
        }

        // special constructor, for use with the JSON serializer
        [JsonConstructor]
        public PD_GameCards(
            List<List<int>> dividedDeckOfInfectionCards,
            List<int> activeInfectionCards,
            List<int> deckOfDiscardedInfectionCards,
            List<List<int>> dividedDeckOfPlayerCards,
            List<int> deckOfDiscardedPlayerCards,
            Dictionary<int, List<int>> playerCardsPerPlayerID,
            List<int> inactiveRoleCards
            )
        {
            DividedDeckOfInfectionCards = dividedDeckOfInfectionCards.CustomDeepCopy();
            ActiveInfectionCards = activeInfectionCards.CustomDeepCopy();
            DeckOfDiscardedInfectionCards = deckOfDiscardedInfectionCards.CustomDeepCopy();
            DividedDeckOfPlayerCards = dividedDeckOfPlayerCards.CustomDeepCopy();
            DeckOfDiscardedPlayerCards = deckOfDiscardedPlayerCards.CustomDeepCopy();
            PlayerCardsPerPlayerID = playerCardsPerPlayerID.CustomDeepCopy();
            InactiveRoleCards = inactiveRoleCards.CustomDeepCopy();
        }

        // private constructor, for deep copy purposes only
        private PD_GameCards(
            PD_GameCards gameCardsToCopy
            )
        {
            DividedDeckOfInfectionCards = gameCardsToCopy.DividedDeckOfInfectionCards.CustomDeepCopy();
            ActiveInfectionCards = gameCardsToCopy.ActiveInfectionCards.CustomDeepCopy();
            DeckOfDiscardedInfectionCards = gameCardsToCopy.DeckOfDiscardedInfectionCards.CustomDeepCopy();
            DividedDeckOfPlayerCards = gameCardsToCopy.DividedDeckOfPlayerCards.CustomDeepCopy();
            DeckOfDiscardedPlayerCards = gameCardsToCopy.DeckOfDiscardedPlayerCards.CustomDeepCopy();
            PlayerCardsPerPlayerID = gameCardsToCopy.PlayerCardsPerPlayerID.CustomDeepCopy();
            InactiveRoleCards = gameCardsToCopy.InactiveRoleCards.CustomDeepCopy();
        }

        public PD_GameCards GetCustomDeepCopy()
        {
            return new PD_GameCards(this);
        }
        #endregion

        #region equality overrides
        public bool Equals(PD_GameCards other)
        {
            if (DividedDeckOfInfectionCards.List_Equals(other.DividedDeckOfInfectionCards) == false)
            {
                return false;
            }
            else if (ActiveInfectionCards.List_Equals(other.ActiveInfectionCards) == false)
            {
                return false;
            }
            else if (DeckOfDiscardedInfectionCards.List_Equals(other.DeckOfDiscardedInfectionCards) == false)
            {
                return false;
            }
            else if (DividedDeckOfPlayerCards.List_Equals(other.DividedDeckOfPlayerCards) == false)
            {
                return false;
            }
            else if (DeckOfDiscardedPlayerCards.List_Equals(other.DeckOfDiscardedPlayerCards) == false)
            {
                return false;
            }
            else if (DeckOfDiscardedPlayerCards.List_Equals(other.DeckOfDiscardedPlayerCards) == false)
            {
                return false;
            }
            else if (PlayerCardsPerPlayerID.Dictionary_Equals(other.PlayerCardsPerPlayerID) == false)
            {
                return false;
            }
            else if (InactiveRoleCards.List_Equals(other.InactiveRoleCards) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GameCards other_game_cards)
            {
                return Equals(other_game_cards);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + DividedDeckOfInfectionCards.Custom_HashCode();
            hash = hash * 31 + ActiveInfectionCards.Custom_HashCode();
            hash = hash * 31 + DeckOfDiscardedInfectionCards.Custom_HashCode();
            hash = hash * 31 + DividedDeckOfPlayerCards.Custom_HashCode();
            hash = hash * 31 + DeckOfDiscardedPlayerCards.Custom_HashCode();
            hash = hash * 31 + PlayerCardsPerPlayerID.Custom_HashCode();
            hash = hash * 31 + InactiveRoleCards.Custom_HashCode();

            return hash;
        }

        

        #endregion

    }
}
