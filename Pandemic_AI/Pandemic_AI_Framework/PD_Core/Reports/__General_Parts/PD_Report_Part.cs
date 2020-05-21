using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public abstract class PD_Report_Part : ICSVRowPart
    {

        public abstract void Update(PD_Game game, PD_AI_PathFinder pathFinder);

        public static string Get_Joined_CSV_Header(
            List<PD_Report_Part> allParts
            )
        {
            string header = "";
            for (int i = 0; i < allParts.Count; i++)
            {
                if (i < allParts.Count - 1)
                {
                    header += allParts[i].Get_CSV_HeaderPart(false);
                    header += " ,";
                }
                else
                {
                    header += allParts[i].Get_CSV_HeaderPart(true);
                }
            }
            return header;
        }

        public static string Get_Joined_CSV_Row(
            List<PD_Report_Part> allParts
            )
        {
            string row = "";
            for (int i = 0; i < allParts.Count; i++)
            {
                if (i < allParts.Count - 1)
                {
                    row += allParts[i].Get_CSV_RowPart(false);
                    row += " ,";
                }
                else
                {
                    row += allParts[i].Get_CSV_RowPart(true);
                }
            }
            return row;
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
                    if (i > 0)
                    {
                        csvHeader += " ,";
                    }
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
                    if (i > 0)
                    {
                        csvRow += " ,";
                    }
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
