namespace Microsoft.Extensions.Logging
{
    public interface IEventIdAccessor
    {
        EventId EventId { get; }
    }
}
