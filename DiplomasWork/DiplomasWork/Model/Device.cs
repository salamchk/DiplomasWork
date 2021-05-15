using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomasWork.Model
{
    public class Device
    {
        public string Name { get; }
        public string Address { get; }

        public  Device(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
