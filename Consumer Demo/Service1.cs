using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ConsumerDemo
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string ReceiveData(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string data = reader.ReadToEnd();
            reader.Close();
            Console.WriteLine("receiving and store data:{0}", data);       
            Thread.Sleep(500);
            Console.WriteLine("CurrentThread:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("data:{0} received", data);
            
            return "success";
        }
    }
}
