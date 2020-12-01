namespace Vueling.XXX.Contracts.DTO.ServiceLibrary
{
    public class SeatDTO
    {
        public SeatDTO(string row, string column)
        {
            Row = row;
            Column = column;
        }

        public string Row { get; set; }

        public string Column { get; set; }
    }
}
