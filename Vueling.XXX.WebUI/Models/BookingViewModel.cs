namespace Vueling.XXX.WebUI.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public string SalesAgent { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
        public string RecordLocator { get; set; }
        public int JourneysCount { get; set; }
        public int PassengersCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}