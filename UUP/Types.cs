using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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

    public class ApiResponse
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
        [JsonProperty("jsonApiVersion")]
        public string JsonApiVersion { get; set; }
    }

    public class Response
    {
        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }
        [JsonProperty("langFancyNames")]
        public Dictionary<string, string> LangFancyNames { get; set; }
    }

    public class Language
    {
        public Language(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public static async Task<Language[]> GetLanguages(string versionId)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{Constants.UUPDUMP_JSONAPI}/listlangs.php?id={versionId}");
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync();

                ApiResponse parsed = JsonConvert.DeserializeObject<ApiResponse>(body);
                List<Language> languages = new List<Language>();
                foreach (var keypair in parsed.Response.LangFancyNames)
                {
                    languages.Add(new Language((string)keypair.Key, (string)keypair.Value));
                }

                return languages.ToArray();
            }
        }

        public override string ToString()
        {
            return Name;
        }
        public string Name { get; }
        public string Code { get; }
    }
}