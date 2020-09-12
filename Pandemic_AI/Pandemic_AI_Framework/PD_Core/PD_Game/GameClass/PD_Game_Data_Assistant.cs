using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Data_Assistant
    {
        public static List<int> Default__Cities()
        {
            List<int> cities = new List<int>();
            for (int c = 0; c < 48; c++)
            {
                cities.Add(c);
            }
            return cities;
        }

        public static Dictionary<int, List<int>> Default__Neighbors__Per__City()
        {
            Dictionary<int, List<int>> neighbors__per__city = new Dictionary<int, List<int>>() {
                { 0,  new List<int>(){  11, 21, 1              } },
                { 1,  new List<int>(){  9,  0,  6              } },
                { 2,  new List<int>(){  3,  8,  5,  10         } },
                { 3,  new List<int>(){  7,  4,  8,  2          } },
                { 4,  new List<int>(){  7,  3,  8,  24, 23     } },
                { 5,  new List<int>(){  29, 8,  2              } },
                { 6,  new List<int>(){  11, 7,  1              } },
                { 7,  new List<int>(){  6,  11, 3,  4          } },
                { 8,  new List<int>(){  3,  4,  2,  5,  24     } },
                { 9,  new List<int>(){  1,  19, 47, 41         } },
                { 10, new List<int>(){  32, 29, 2              } },
                { 11, new List<int>(){  0,  21, 6,  7          } },

                { 12, new List<int>(){  21, 20, 18, 13, 23     } },
                { 13, new List<int>(){  12, 23                 } },
                { 14, new List<int>(){  16, 15                 } },
                { 15, new List<int>(){  17, 16, 14, 26         } },
                { 16, new List<int>(){  17, 15, 14             } },
                { 17, new List<int>(){  23, 15, 16             } },
                { 18, new List<int>(){  12, 20, 22             } },
                { 19, new List<int>(){  20, 45, 9,  1          } },
                { 20, new List<int>(){  21, 1,  19, 12         } },
                { 21, new List<int>(){  0,  11, 20, 12         } },
                { 22, new List<int>(){  18                     } },
                { 23, new List<int>(){  12, 13, 17, 4          } },

                { 24, new List<int>(){  8,  4,  29, 26         } },
                { 25, new List<int>(){  29, 34, 26, 30, 35     } },
                { 26, new List<int>(){  24, 15, 34, 25, 29     } },
                { 27, new List<int>(){  33, 28, 31, 36, 40     } },
                { 28, new List<int>(){  30, 35, 33, 27, 31     } },
                { 29, new List<int>(){  5,  10, 32, 25, 26, 24 } },
                { 30, new List<int>(){  28, 25, 35, 34, 33     } },
                { 31, new List<int>(){  36, 39, 28, 27         } },
                { 32, new List<int>(){  10, 29, 35             } },
                { 33, new List<int>(){  30, 27, 28             } },
                { 34, new List<int>(){  25, 26, 30             } },
                { 35, new List<int>(){  32, 25, 28, 30         } },

                { 36, new List<int>(){  31, 27,39,38,40        } },
                { 37, new List<int>(){  44, 43                 } },
                { 38, new List<int>(){  36, 39, 41, 40         } },
                { 39, new List<int>(){  31, 36, 46, 44, 38, 41 } },
                { 40, new List<int>(){  38, 36, 27, 45         } },
                { 41, new List<int>(){  45, 9,  39, 38, 46     } },
                { 42, new List<int>(){  47, 46                 } },
                { 43, new List<int>(){  47, 37, 44             } },
                { 44, new List<int>(){  37, 47, 43, 46, 39     } },
                { 45, new List<int>(){  40, 41, 19             } },
                { 46, new List<int>(){  39, 42, 41, 44         } },
                { 47, new List<int>(){  42, 9, 43, 44          } }
            };

            return neighbors__per__city;
        }

        public static Dictionary<int, int> Default__InfectionType__Per__City()
        {
            Dictionary<int, int> infection_type__per__city = new Dictionary<int, int>() {
                {0,   0},
                {1,   0},
                {2,   0},
                {3,   0},
                {4,   0},
                {5,   0},
                {6,   0},
                {7,   0},
                {8,   0},
                {9,   0},
                {10,  0},
                {11,  0},

                {12,  1},
                {13,  1},
                {14,  1},
                {15,  1},
                {16,  1},
                {17,  1},
                {18,  1},
                {19,  1},
                {20,  1},
                {21,  1},
                {22,  1},
                {23,  1},

                {24,  2},
                {25,  2},
                {26,  2},
                {27,  2},
                {28,  2},
                {29,  2},
                {30,  2},
                {31,  2},
                {32,  2},
                {33,  2},
                {34,  2},
                {35,  2},

                {36,  3},
                {37,  3},
                {38,  3},
                {39,  3},
                {40,  3},
                {41,  3},
                {42,  3},
                {43,  3},
                {44,  3},
                {45,  3},
                {46,  3},
                {47,  3}
            };

            return infection_type__per__city;
        }

        public static List<int> Default__CityCards()
        {
            List<int> city_cards = new List<int>();
            for (int cc = 0; cc < 48; cc++)
            {
                city_cards.Add(cc);
            }
            return city_cards;
        }

        public static List<int> Default__InfectionCards() {
            List<int> infection_cards = new List<int>();
            for (int ic = 0; ic < 48; ic++)
            {
                infection_cards.Add(ic);
            }
            return infection_cards;
        }

        public static List<int> Default__EpidemicCards() {
            List<int> epidemic_cards = new List<int>();
            for (int ic = 0; ic < 6; ic++)
            {
                epidemic_cards.Add(ic + 128);
            }
            return epidemic_cards;
        }
    }
}
