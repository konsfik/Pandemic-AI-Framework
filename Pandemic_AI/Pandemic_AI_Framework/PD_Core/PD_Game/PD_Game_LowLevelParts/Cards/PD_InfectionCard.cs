using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_InfectionCard : PD_PlayerCardBase, ICustomDeepCopyable<PD_InfectionCard>
    {
        public PD_City City { get; private set; }
        public int Type { get { return City.Type; } }

        [JsonConstructor]
        public PD_InfectionCard(int id, PD_City city) : base(id)
        {
            City = city;
        }

        public override string GetDescription()
        {
            return "_" + City.Name;
        }

        public PD_InfectionCard GetCustomDeepCopy()
        {
            return new PD_InfectionCard(
                this.ID,
                this.City.GetCustomDeepCopy()
                );
        }
    }
}
