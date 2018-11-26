namespace Tests.Integration
{
    using RestSharp;
    using RestSharp.Deserializers;
    using System.Collections.Generic;
    using System.IO;
    using WebApi.Dto;

    internal class NewtonsoftJsonDeSerializer : IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer srlz;

        public NewtonsoftJsonDeSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.srlz = serializer;
        }

        public string DateFormat { get; set; }
        public string Namespace { get; set; }
        public string RootElement { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(stringReader))
                {
                    return srlz.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public static NewtonsoftJsonDeSerializer Default => new NewtonsoftJsonDeSerializer(
                Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    Converters = new List<Newtonsoft.Json.JsonConverter>(new[] { new UploadResponseDtoConverter() })
                }));        
    }
}