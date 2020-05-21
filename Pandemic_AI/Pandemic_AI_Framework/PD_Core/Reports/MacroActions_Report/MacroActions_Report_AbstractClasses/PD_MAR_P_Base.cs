using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public abstract class PD_MAR_P_Base : IDescribable, ICSVRowPart
    {
        public abstract void Update(
            PD_Game game,
            List<PD_MacroAction> allMacros
            );

        public string GetDescription()
        {
            string description = "";

            var myProperties = this.GetType().GetProperties();

            for (int i = 0; i < myProperties.Length; i++)
            {
                var propertyInfo = myProperties[i];
                var propertyType = propertyInfo.PropertyType;
                var propertyValue = propertyInfo.GetValue(this);
                if (
                    typeof(ICSVRowPart).IsAssignableFrom(propertyType)
                    &&
                    typeof(IDescribable).IsAssignableFrom(propertyType)
                    )
                {
                    description += "\n";
                    description += ((IDescribable)propertyValue).GetDescription();
                }
                else
                {
                    description += String.Format(
                    "{0} : {1}\n",
                    propertyInfo.Name,
                    propertyValue.ToString()
                    );
                }
            }

            return description;
        }

        public string Get_CSV_HeaderPart(bool isLastPart)
        {
            string csvHeader = "";

            var myProperties = this.GetType().GetProperties();

            for (int i = 0; i < myProperties.Length; i++)
            {
                var propertyInfo = myProperties[i];
                var propertyType = propertyInfo.PropertyType;
                var propertyValue = propertyInfo.GetValue(this);
                if (
                    typeof(ICSVRowPart).IsAssignableFrom(propertyType)
                    )
                {
                    if (i < myProperties.Length - 1)
                    {
                        csvHeader += ((ICSVRowPart)propertyValue).Get_CSV_HeaderPart(false);
                    }
                    else
                    {
                        csvHeader += ((ICSVRowPart)propertyValue).Get_CSV_HeaderPart(isLastPart);
                    }
                }
                else
                {
                    csvHeader += propertyInfo.Name;
                    if (i < myProperties.Length - 1)
                    {
                        csvHeader += ",";
                    }
                    else
                    {
                        if (isLastPart)
                        {
                            csvHeader += "\n";
                        }
                        else
                        {
                            csvHeader += ",";
                        }
                    }
                }
            }


            return csvHeader;
        }

        public string Get_CSV_RowPart(bool isLastPart)
        {
            string csvRow = "";

            var myProperties = this.GetType().GetProperties();

            for (int i = 0; i < myProperties.Length; i++)
            {
                var propertyInfo = myProperties[i];
                var propertyType = propertyInfo.PropertyType;
                var propertyValue = propertyInfo.GetValue(this);
                if (
                    typeof(ICSVRowPart).IsAssignableFrom(propertyType)
                    )
                {
                    if (i < myProperties.Length - 1)
                    {
                        csvRow += ((ICSVRowPart)propertyValue).Get_CSV_RowPart(false);
                    }
                    else
                    {
                        csvRow += ((ICSVRowPart)propertyValue).Get_CSV_RowPart(isLastPart);
                    }
                }
                else
                {
                    csvRow += propertyInfo.GetValue(this).ToString();
                    if (i < myProperties.Length - 1)
                    {
                        csvRow += ",";
                    }
                    else
                    {
                        if (isLastPart)
                        {
                            csvRow += "\n";
                        }
                        else
                        {
                            csvRow += ",";
                        }
                    }
                }
            }

            return csvRow;
        }
    }
}
