using System;

namespace XX.Template.Repository.Contracts.Model
{
    public class BookingLogModel
    {
        public string RecordLocator { get; set; }

        public DateTime Date { get; set; }

        public string Email { get; set; }
    }
}