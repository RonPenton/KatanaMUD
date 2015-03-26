using KatanaMUD.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace KatanaMUD
{
    public static class MessageSerializer
    {

        public static string SerializeMessage(MessageBase message)
        {
            return JsonConvert.SerializeObject(message, Formatting.None, _settings);
        }

        static Assembly _assembly;
        static Assembly Assembly
        {
            get
            {
                if (_assembly == null)
                    _assembly = Assembly.Load(new AssemblyName("KatanaMUD.Messages"));
                return _assembly;
            }
        }

        static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ContractResolver = new OrderedContractResolver()
        };

        public static MessageBase DeserializeMessage(string message)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(message));

            while(reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == nameof(MessageBase.MessageName))
                {
                    reader.Read();
                    var messageName = reader.Value.ToString();
                    var type = Assembly.GetType("KatanaMUD.Messages." + messageName);
                    return (MessageBase)JsonConvert.DeserializeObject(message, type);
                }
            }

            throw new InvalidOperationException();
        }

        public class OrderedContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, memberSerialization).OrderByDescending(x => x.PropertyName == "MessageName").ToList();
            }
        }

    }
}