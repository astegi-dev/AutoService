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
    public class ServiceEntry
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ServiceAction Action { get; set; }

        public ServiceEntry()
        {

        }

        public ServiceEntry(string name, ServiceAction action)
        {
            Name = name;
            Action = action;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return "Service entry";

            return $"{Action}: {Name}";
        }
    }
}
