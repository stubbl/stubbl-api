namespace Stubbl.Api.Queries.CountTeamStubs.Version1
{
    public class TagCount
    {
        public TagCount(string tag, long count)
        {
            Tag = tag;
            Count = count;
        }

        public long Count { get; }
        public string Tag { get; }
    }
}