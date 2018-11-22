namespace WebApi.Extensions
{
    using Microsoft.Extensions.Primitives;

    public static class StringSegmentExtensions
    {
        public static bool IsNullOrWhiteSpace(this StringSegment stringSegment) => string.IsNullOrWhiteSpace(stringSegment.Value);

        public static bool IsNullOrEmpty(this StringSegment stringSegment) => string.IsNullOrEmpty(stringSegment.Value);
    }
}
