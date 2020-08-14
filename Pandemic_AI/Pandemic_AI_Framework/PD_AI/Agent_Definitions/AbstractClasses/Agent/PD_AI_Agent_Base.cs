using System.Collections;
using System.Collections.Generic;
using System;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_AI_Agent_Base : IDescribable, ICSVRowPart
    {
        public virtual string Get_CSV_HeaderPart(bool csvHeaderFinished)
        {
            string csvHeader = "Agent_Name";

            if (csvHeaderFinished)
            {
                csvHeader += "\n";
            }
            else
            {
                csvHeader += ",";
            }

            return csvHeader;
        }

        public virtual string Get_CSV_RowPart(bool csvRowFinished)
        {
            string csvRow = this.GetType().Name.ToString();
            if (csvRowFinished)
            {
                csvRow += "\n";
            }
            else {
                csvRow += ",";
            }

            return csvRow;
        }
        public abstract string GetDescription();
    }
}