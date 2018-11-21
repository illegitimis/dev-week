namespace WebApi.Dto
{
    using Newtonsoft.Json;
    using System;

    [Serializable]
    public class UploadItemResponseDto
    {
        public UploadItemResponseDto(string fn, float min, float max)
        {
            FileName = fn;
            BuyPoint = min;
            SellPoint = max;
        }

        [JsonProperty("buyPoint", Order = 1)]
        public float BuyPoint { get; set; }

        [JsonProperty("sellPoint", Order = 2)]
        public float SellPoint { get; set; }

        [JsonIgnore]
        public string FileName { get; set; }

        [JsonIgnore]
        public bool IsValid => !float.IsNaN(BuyPoint) && !float.IsNaN(SellPoint);
    }
}
