namespace WebApi.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DevWeek.Algo;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class UploadResponseDtoConverter : JsonConverter<UploadResponseDto>
    {
        public override UploadResponseDto ReadJson(JsonReader reader, Type objectType, UploadResponseDto existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var list = new List<UploadItemResponseDto>();
            foreach(KeyValuePair<string,JToken> kvp in jsonObject)
            {
                UploadItemResponseDto item = kvp.Value.ToObject<UploadItemResponseDto>();
                item.FileName = kvp.Key;
                list.Add(item);                
            }
            return new UploadResponseDto(list);
        }

        public override void WriteJson(JsonWriter writer, UploadResponseDto value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var uploadResponseItem in value.Items.Where(item => item.IsValid))
            {
                writer.WritePropertyName(uploadResponseItem.FileName);
                serializer.Serialize(writer, uploadResponseItem);
            }
            writer.WriteEndObject();
        }
    }
}