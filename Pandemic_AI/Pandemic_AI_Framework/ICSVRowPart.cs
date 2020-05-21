using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public interface ICSVRowPart
    {
        string Get_CSV_HeaderPart(bool isLastPart);
        string Get_CSV_RowPart(bool isLastPart);
    }
}
