namespace WebApi.Dto
{
    using DevWeek.Algo;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // collections  
    // System.Collections.Generic.EqualityComparer<T>
    // public abstract class EqualityComparer<T> : System.Collections.Generic.IEqualityComparer<T>, System.Collections.IEqualityComparer

    [Serializable]
    [JsonConverter(typeof(UploadResponseDtoConverter))]
    public sealed class UploadResponseDto 
        : IEquatable<UploadResponseDto>
        , IComparable<UploadResponseDto>
    {
        #region ctor

        public UploadResponseDto()
        {
        }

        public UploadResponseDto(List<UploadItemResponseDto> list)
        {
            Items = list;
        }

        public UploadResponseDto(IList<ProcessZipItemModel> completedTasksResponses)
        {
            items = new List<UploadItemResponseDto>();

            foreach (var x in completedTasksResponses)
            {
                AddItem(x.File, x.Min, x.Max);
            }
        }

        #endregion

        #region items
        internal List<UploadItemResponseDto> items;

        public IReadOnlyCollection<UploadItemResponseDto> Items
        {
            get => items.AsReadOnly();
            set => items = new List<UploadItemResponseDto>(value);
        }

        private void AddItem(string fn, float min, float max)
        {
            items.Add(new UploadItemResponseDto(fn, min, max));
        }
        #endregion

        #region equality

        static readonly EqualityComparer<UploadResponseDto> equalityComparer = new UploadResponseDtoEqualityComparer();

        public bool Equals(UploadResponseDto other) => equalityComparer.Equals(this, other);

        public override bool Equals(object obj) => (obj is UploadResponseDto dto) && Equals(dto);

        public override int GetHashCode() => equalityComparer.GetHashCode(this);

        /*
        public bool Equals(object other, IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }
        */

        #endregion

        static readonly IComparer<UploadResponseDto> comparer = new UploadResponseDtoComparer();

        public int CompareTo(UploadResponseDto other)
        {
            throw new NotImplementedException();
        }

    }
}