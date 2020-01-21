using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProducerDemo
{
    public interface IGenerators
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/Generate",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        bool Generate();
    }
}