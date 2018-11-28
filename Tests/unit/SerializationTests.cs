namespace Tests.Unit
{
    using DevWeek.Algo;
    using Newtonsoft.Json;
    using System.Linq;
    using WebApi.Dto;
    using Xunit;

    public class SerializationTests
    {
        [Fact]
        public void CustomConversion()
        {
            var pzi1 = new ProcessZipItemModel(0, 1, "01");
            var pzi2 = new ProcessZipItemModel(0.5f, 2, "0.5f2");
            UploadResponseDto dto = new UploadResponseDto(new[] { pzi1, pzi2 }.ToList());
            var jsonString = JsonConvert.SerializeObject(dto);
            UploadResponseDto dto2 = JsonConvert.DeserializeObject<UploadResponseDto>(jsonString, new UploadResponseDtoConverter());

            Assert.Equal(dto.Items.Count, dto2.Items.Count);
            
            // TODO: equality
            // Assert.True (Enumerable.SequenceEqual(dto.Items, dto2.Items));            
            // Assert.Equal(dto, dto2);
        }       
    }
}
