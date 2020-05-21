using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public interface ICustomDeepCopyable<T>
    {
        T GetCustomDeepCopy();
    }
}
