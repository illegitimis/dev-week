namespace WebApi.Dto
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;

    public class UploadResponseDtoConverter : JsonConverter<UploadResponseDto>
    {
        public override UploadResponseDto ReadJson(JsonReader reader, Type objectType, UploadResponseDto existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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