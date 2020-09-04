﻿using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_CityCard :
        PD_PlayerCardBase,
        IEquatable<PD_CityCard>,
        ICustomDeepCopyable<PD_CityCard>
    {
        public PD_City City { get; private set; }

        public string Name { get { return City.Name; } }
        public int Type { get { return City.Type; } }

        [JsonConstructor]
        public PD_CityCard(int id, PD_City city) : base(id)
        {
            City = city;
        }

        public PD_CityCard GetCustomDeepCopy()
        {
            return new PD_CityCard(
                this.ID,
                this.City.GetCustomDeepCopy()
                );
        }

        public override string GetDescription()
        {
            return "." + City.Name;
        }

        #region equality overrides
        public bool Equals(PD_CityCard other)
        {
            if (this.ID != other.ID)
            {
                return false;
            }
            else if (this.City != other.City)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(PD_PlayerCardBase other)
        {
            if (other is PD_CityCard other_city_card)
            {
                return Equals(other_city_card);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object other)
        {
            if (other is PD_CityCard other_city_card)
            {
                return Equals(other_city_card);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + ID;
            hash = (hash * 13) + City.GetHashCode();

            return hash;
        }
        #endregion

        public override string ToString()
        {
            return String.Format("City Card {0}:{1}", ID, City.ToString());
        }


    }
}
