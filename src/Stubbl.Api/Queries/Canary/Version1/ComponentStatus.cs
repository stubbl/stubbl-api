namespace Stubbl.Api.Queries.Canary.Version1
{
    public enum ComponentStatus
    {
        Unknown = 0,
        Operational = 1,
        DegradedPerformance = 2,
        MajorOutage = 3
    }
}