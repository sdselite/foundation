using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SDSFoundation.Security.OpenIdDict.Flows.Password
{
 
    internal class PasswordFlowClaimConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(System.Security.Claims.Claim));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string type = (string)jo["Type"];
            string value = (string)jo["Value"];
            string valueType = (string)jo["ValueType"];
            string issuer = (string)jo["Issuer"];
            string originalIssuer = (string)jo["OriginalIssuer"];
            var claim = new Claim(type, value, valueType, issuer, originalIssuer);

            if (jo.ContainsKey("Properties"))
            {
                try
                {
                    var claimProperties = JsonConvert.DeserializeObject<Dictionary<string, string>>(jo["Properties"].ToString());

                    if (claimProperties != null && claimProperties.Count > 0)
                    {
                        foreach (var claimProperty in claimProperties)
                        {
                            claim.Properties.Add(claimProperty);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failure at ReadJson: " + ex.Message);
                    //Ignore
                }

            }


            return claim;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
