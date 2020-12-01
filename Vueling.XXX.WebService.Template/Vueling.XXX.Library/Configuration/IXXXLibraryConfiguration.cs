namespace Vueling.XXX.Library.Configuration
{
    public interface IXXXLibraryConfiguration
    {
        int TimeSalesCloseBeforeFlight { get; }

        int MaxJourneysAllowedByBooking { get; }

        string PartialCodeForAgencyAgent { get; }

        string PartialCodeForCorporateAgent { get; }
    }
}
