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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {

        static readonly HttpClient client = new HttpClient();

        public static string config = "default config";
        private Int32 recordsNumber = 10;
        private volatile Int32 remainRecordsNumber = 4;
        private volatile Int32 sendedRecordsNumber = 0;
        private Int32 channelNumber = 3;
        readonly private static string dataFilePath = Path.Combine(
                System.Environment.CurrentDirectory,
                ".\\data");
        public string Configure(string config)
        {
            Console.WriteLine("Configuration info: {0}",config);
            Service1.config = config;
            Console.WriteLine("Configuration is completed.");
            return "Configuration is completed.";
        }

        public string Generate()
        {
            Console.WriteLine("Generating data according to configuration: {0}", config);
            if (!File.Exists(dataFilePath))
                File.Create(dataFilePath).Close();
            string data = "some data...";
            StreamWriter streamWriter = new StreamWriter(dataFilePath);
            streamWriter.WriteLine(data);
            streamWriter.Close();
            Console.WriteLine("Store data at {0}", dataFilePath);
            Console.WriteLine("Data is generated.");
            
            return "Data is generated.";
        }

        public string SendData(string type)
        {
            Console.WriteLine("Sending data to cosumer: {0}", type);
            remainRecordsNumber = recordsNumber;
            sendedRecordsNumber = 0;
            for (Int32 i = 0; i < channelNumber; i++)
            {
                postData(i);
            }   
            while(sendedRecordsNumber < recordsNumber) { }
            Console.WriteLine("Sending data is completed");
            return "Sending data is completed";
        }

        public async Task postData(Int32 channelNumber)
        {
            await Task.Run(async () =>
            {
                try
                {
                    FileStream fileStream = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    // Call asynchronous network methods in a try/catch block to handle exceptions.
                    try
                    {
                        remainRecordsNumber--;
                        if (remainRecordsNumber >= 0)
                        {
                            Console.WriteLine("Channel:{0} start sending... at thread: {1}", channelNumber,Thread.CurrentThread.ManagedThreadId);                           
                            Thread.Sleep(500);                          
                            HttpResponseMessage response = await client.PostAsync("http://localhost:8081/ReceiveData", new StreamContent(fileStream));
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("{0} at channel: {1}", responseBody, channelNumber);
                            Console.WriteLine("A record is successfully sended at channel:{0}", channelNumber);
                            if (fileStream != null)
                                fileStream.Close();
                            sendedRecordsNumber++;
                            if (remainRecordsNumber > 0)
                                postData(channelNumber);
                        }

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            });
        }
    }
}
