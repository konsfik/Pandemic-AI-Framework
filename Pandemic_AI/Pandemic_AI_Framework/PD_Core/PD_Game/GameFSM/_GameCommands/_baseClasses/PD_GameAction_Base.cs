using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_GameAction_Base : IGameAction, ICustomDeepCopyable<PD_GameAction_Base>
    {


        public abstract PD_GameAction_Base GetCustomDeepCopy();
        public abstract void Execute(
            Random randomness_provider, 
            PD_Game game
            );
        public abstract string GetDescription();

        #region equalityOverride
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_GameAction_Base)otherObject;

            Type type = this.GetType();

            //foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.PropertyType == typeof(List<PD_CityCard>))
                {
                    List<PD_CityCard> selfValue = (List<PD_CityCard>)type.GetProperty(pi.Name).GetValue(this, null);
                    List<PD_CityCard> otherValue = (List<PD_CityCard>)type.GetProperty(pi.Name).GetValue(other, null);
                    if (selfValue.Count != otherValue.Count)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < selfValue.Count; i++)
                        {
                            var element1 = selfValue[i];
                            var element2 = otherValue[i];
                            if (element1 != element2 &&
                                    (
                                    element1 == null
                                    || !element1.Equals(element2)
                                    )
                                )
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    object selfValue = type.GetProperty(pi.Name).GetValue(this, null);
                    object otherValue = type.GetProperty(pi.Name).GetValue(other, null);

                    if (
                        selfValue != otherValue
                        && (
                            selfValue == null
                            || !selfValue.Equals(otherValue)
                            )
                        )
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            Type type = this.GetType();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.PropertyType != typeof(PD_Game))
                {
                    object selfValue = type.GetProperty(pi.Name).GetValue(this, null);
                    if (selfValue.GetType() != typeof(List<PD_CityCard>))
                    {

                        hash = (hash * 13) + selfValue.GetHashCode();
                    }
                    else
                    {
                        var sv = (List<PD_CityCard>)selfValue;
                        foreach (var card in sv)
                        {
                            hash = (hash * 13) + card.GetHashCode();
                        }
                    }
                }
            }

            return hash;
        }


        public static bool operator ==(PD_GameAction_Base c1, PD_GameAction_Base c2)
        {
            if (Object.ReferenceEquals(c1, null))
            {
                if (Object.ReferenceEquals(c2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // c1 is not null
            {
                if (Object.ReferenceEquals(c2, null)) // c2 is null
                {
                    return false;
                }
            }
            // c1 is not null && c2 is not null
            // -> actually check equality
            return c1.Equals(c2);
        }

        public static bool operator !=(PD_GameAction_Base c1, PD_GameAction_Base c2)
        {
            return !(c1 == c2);
        }
        #endregion
    }
}