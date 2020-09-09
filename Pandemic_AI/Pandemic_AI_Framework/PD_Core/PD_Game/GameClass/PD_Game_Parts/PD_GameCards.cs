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

        public List<List<int>> divided_deck_of_infection_cards;
        public List<int> active_infection_cards;
        public List<int> deck_of_discarded_infection_cards;
        public List<List<int>> divided_deck_of_player_cards;
        public List<int> deck_of_discarded_player_cards;
        public Dictionary<int, List<int>> player_hand__per__player;

        #region constructors
        // normal constructor
        public PD_GameCards(
            List<int> players
            )
        {
            divided_deck_of_infection_cards = new List<List<int>>();
            active_infection_cards = new List<int>();
            deck_of_discarded_infection_cards = new List<int>();

            divided_deck_of_player_cards = new List<List<int>>();
            deck_of_discarded_player_cards = new List<int>();
            player_hand__per__player = new Dictionary<int, List<int>>();
            foreach (var player in players)
                player_hand__per__player.Add(player, new List<int>());
        }

        // special constructor, for use with the JSON serializer
        [JsonConstructor]
        public PD_GameCards(
            List<List<int>> dividedDeckOfInfectionCards,
            List<int> activeInfectionCards,
            List<int> deckOfDiscardedInfectionCards,
            List<List<int>> dividedDeckOfPlayerCards,
            List<int> deckOfDiscardedPlayerCards,
            Dictionary<int, List<int>> playerCardsPerPlayerID
            )
        {
            divided_deck_of_infection_cards = dividedDeckOfInfectionCards.CustomDeepCopy();
            active_infection_cards = activeInfectionCards.CustomDeepCopy();
            deck_of_discarded_infection_cards = deckOfDiscardedInfectionCards.CustomDeepCopy();
            divided_deck_of_player_cards = dividedDeckOfPlayerCards.CustomDeepCopy();
            deck_of_discarded_player_cards = deckOfDiscardedPlayerCards.CustomDeepCopy();
            player_hand__per__player = playerCardsPerPlayerID.CustomDeepCopy();
        }

        // private constructor, for deep copy purposes only
        private PD_GameCards(
            PD_GameCards gameCardsToCopy
            )
        {
            divided_deck_of_infection_cards = gameCardsToCopy.divided_deck_of_infection_cards.CustomDeepCopy();
            active_infection_cards = gameCardsToCopy.active_infection_cards.CustomDeepCopy();
            deck_of_discarded_infection_cards = gameCardsToCopy.deck_of_discarded_infection_cards.CustomDeepCopy();
            divided_deck_of_player_cards = gameCardsToCopy.divided_deck_of_player_cards.CustomDeepCopy();
            deck_of_discarded_player_cards = gameCardsToCopy.deck_of_discarded_player_cards.CustomDeepCopy();
            player_hand__per__player = gameCardsToCopy.player_hand__per__player.CustomDeepCopy();
        }

        public PD_GameCards GetCustomDeepCopy()
        {
            return new PD_GameCards(this);
        }
        #endregion

        #region equality overrides
        public bool Equals(PD_GameCards other)
        {
            if (divided_deck_of_infection_cards.List_Equals(other.divided_deck_of_infection_cards) == false)
            {
                return false;
            }
            else if (active_infection_cards.List_Equals(other.active_infection_cards) == false)
            {
                return false;
            }
            else if (deck_of_discarded_infection_cards.List_Equals(other.deck_of_discarded_infection_cards) == false)
            {
                return false;
            }
            else if (divided_deck_of_player_cards.List_Equals(other.divided_deck_of_player_cards) == false)
            {
                return false;
            }
            else if (deck_of_discarded_player_cards.List_Equals(other.deck_of_discarded_player_cards) == false)
            {
                return false;
            }
            else if (deck_of_discarded_player_cards.List_Equals(other.deck_of_discarded_player_cards) == false)
            {
                return false;
            }
            else if (player_hand__per__player.Dictionary_Equals(other.player_hand__per__player) == false)
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

            hash = hash * 31 + divided_deck_of_infection_cards.Custom_HashCode();
            hash = hash * 31 + active_infection_cards.Custom_HashCode();
            hash = hash * 31 + deck_of_discarded_infection_cards.Custom_HashCode();
            hash = hash * 31 + divided_deck_of_player_cards.Custom_HashCode();
            hash = hash * 31 + deck_of_discarded_player_cards.Custom_HashCode();
            hash = hash * 31 + player_hand__per__player.Custom_HashCode();

            return hash;
        }

        

        #endregion

    }
}
