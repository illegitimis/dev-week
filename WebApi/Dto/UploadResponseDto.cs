namespace WebApi.Dto
{
    using DevWeek.Algo;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    [Serializable]
    [JsonConverter(typeof(UploadResponseDtoConverter))]
    public class UploadResponseDto
    {
        List<UploadItemResponseDto> items;

        public UploadResponseDto(IList<ProcessZipItemModel> completedTasksResponses)
        {
            items = new List<UploadItemResponseDto>();

            foreach (var x in completedTasksResponses)
            {
                AddItem(x.File, x.Min, x.Max);
            }
        }

        public IReadOnlyCollection<UploadItemResponseDto> Items
        {
            get => items.AsReadOnly();
            set => items = new List<UploadItemResponseDto>(value);
        }

        private void AddItem(string fn, float min, float max)
        {
            items.Add(new UploadItemResponseDto(fn, min, max));
        }
    }
}