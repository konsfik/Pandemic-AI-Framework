using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class Pandemic_Extra_Data
    {
        public Dictionary<int, string> default__name__per__city;
        public Dictionary<int, PD_Point> default__position__per__city;


        public Pandemic_Extra_Data()
        {
            default__name__per__city = new Dictionary<int, string>() {
                {0,   "Atlanta"          },
                {1,   "Chicago"          },
                {2,   "Essen"            },
                {3,   "London"           },
                {4,   "Madrid"           },
                {5,   "Milan"            },
                {6,   "Montreal"         },
                {7,   "New York"         },
                {8,   "Paris"            },
                {9,   "San Francisco"    },
                {10,  "St. Petersburg"   },
                {11,  "Washington"       },

                {12,  "Bogota"           },
                {13,  "Buenos Aires"     },
                {14,  "Johannesburg"     },
                {15,  "Khartoum"         },
                {16,  "Kinshasha"        },
                {17,  "Lagos"            },
                {18,  "Lima"             },
                {19,  "Los Angeles"      },
                {20,  "Mexico City"      },
                {21,  "Miami"            },
                {22,  "Santiago"         },
                {23,  "Sao Paolo"        },

                {24,  "Algiers"          },
                {25,  "Baghdad"          },
                {26,  "Cairo"            },
                {27,  "Chennai"          },
                {28,  "Delhi"            },
                {29,  "Istanbul"         },
                {30,  "Karachi"          },
                {31,  "Kolkata"          },
                {32,  "Moscow"           },
                {33,  "Mumbai"           },
                {34,  "Riyadh"           },
                {35,  "Tehran"           },

                {36,  "Bangkok"          },
                {37,  "Beijing"          },
                {38,  "Ho Chi Minh City" },
                {39,  "Hong Kong"        },
                {40,  "Jakarta"          },
                {41,  "Manila"           },
                {42,  "Osaka"            },
                {43,  "Seoul"            },
                {44,  "Shanghai"         },
                {45,  "Sydney"           },
                {46,  "Taipei"           },
                {47,  "Tokyo"            }
            };


            default__position__per__city = new Dictionary<int, PD_Point>() {
                {0,   new PD_Point( 379,  837  ) },
                {1,   new PD_Point( 330,  934  ) },
                {2,   new PD_Point( 942,  1026 ) },
                {3,   new PD_Point( 809,  1002 ) },
                {4,   new PD_Point( 790,  872  ) },
                {5,   new PD_Point( 997,  963  ) },
                {6,   new PD_Point( 462,  939  ) },
                {7,   new PD_Point( 564,  924  ) },
                {8,   new PD_Point( 910,  938  ) },
                {9,   new PD_Point( 161,  885  ) },
                {10,  new PD_Point( 1089, 1054 ) },
                {11,  new PD_Point( 519,  844  ) },

                {12,  new PD_Point( 452,  595  ) },
                {13,  new PD_Point( 561,  331  ) },
                {14,  new PD_Point( 1055, 392  ) },
                {15,  new PD_Point( 1065, 643  ) },
                {16,  new PD_Point( 980,  526  ) },
                {17,  new PD_Point( 896,  619  ) },
                {18,  new PD_Point( 405,  452  ) },
                {19,  new PD_Point( 188,  753  ) },
                {20,  new PD_Point( 311,  709  ) },
                {21,  new PD_Point( 463,  729  ) },
                {22,  new PD_Point( 423,  305  ) },
                {23,  new PD_Point( 638,  432  ) },

                {24,  new PD_Point( 938,  800  ) },
                {25,  new PD_Point( 1150, 818  ) },
                {26,  new PD_Point( 1037, 773  ) },
                {27,  new PD_Point( 1392, 605  ) },
                {28,  new PD_Point( 1373, 812  ) },
                {29,  new PD_Point( 1056, 887  ) },
                {30,  new PD_Point( 1276, 775  ) },
                {31,  new PD_Point( 1466, 780  ) },
                {32,  new PD_Point( 1160, 964  ) },
                {33,  new PD_Point( 1289, 677  ) },
                {34,  new PD_Point( 1165, 699  ) },
                {35,  new PD_Point( 1250, 901  ) },

                {36,  new PD_Point( 1486, 668  ) },
                {37,  new PD_Point( 1543, 931  ) },
                {38,  new PD_Point( 1567, 574  ) },
                {39,  new PD_Point( 1563, 727  ) },
                {40,  new PD_Point( 1486, 490  ) },
                {41,  new PD_Point( 1697, 582  ) },
                {42,  new PD_Point( 1764, 786  ) },
                {43,  new PD_Point( 1660, 938  ) },
                {44,  new PD_Point( 1551, 839  ) },
                {45,  new PD_Point( 1773, 310  ) },
                {46,  new PD_Point( 1668, 746  ) },
                {47,  new PD_Point( 1753, 885  ) }
            };
        }
    }
}
