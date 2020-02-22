using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.State
{
    [Serializable]
    [DataContract]
    public enum ServiceAction
    {
        None,
        Start,
        Stop
    }
}
