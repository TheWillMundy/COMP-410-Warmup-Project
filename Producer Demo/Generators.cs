using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerDemo
{
    class Generators : IGenerators
    {
        public bool Generate()
        {
            Console.WriteLine("Generated!");
            return true;
        }
    }
}
