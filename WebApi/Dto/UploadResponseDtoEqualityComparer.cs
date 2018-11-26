using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dto
{
    public class UploadResponseDtoEqualityComparer : EqualityComparer<UploadResponseDto>
    {
        public override bool Equals(UploadResponseDto x, UploadResponseDto y)
        {
            if (x == null && y == null) return true;
            else if (x == null || y == null) return false;
            else if (object.ReferenceEquals(x, y)) return true;
            else return Enumerable.SequenceEqual(x.Items, y.Items);
        }

        public override int GetHashCode(UploadResponseDto obj) => obj.Items.Aggregate(1, (acc, item) => acc * item.GetHashCode());
    }

    public class UploadResponseDtoComparer : Comparer<UploadResponseDto>
    {
        public override int Compare(UploadResponseDto x, UploadResponseDto y)
        {
            if (object.ReferenceEquals(x, y)) return 0;
            else if (x == null && y == null) return 0;
            else if (x == null && y != null) return -1;
            else if (x != null && y == null) return 1;
            else return StructuralComparisons.StructuralComparer.Compare(x.items, y.items);
        }
    }
}
