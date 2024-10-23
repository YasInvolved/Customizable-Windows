using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customizable_Windows.UUP
{
    public class Version
    {
        public Version(string name, string uuid) 
        {
            Name = name;
            UUID = uuid;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; }
        public string UUID { get; }
    };
}