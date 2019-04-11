namespace Vueling.XXX.Library.Entities
{
    public class Seat
    {

        public string Row
        { 
            get;
            set; 
        }

        public string Column 
        { 
            get; 
            set; 
        }

        public AvailabilityEnum Availability
        { 
            get; 
            set;
        }

        public bool IsNull { 
            get 
            { 
                return string.IsNullOrEmpty(Row) || string.IsNullOrEmpty(Column); 
            } 
        }

        public bool IsAvailable 
        { 
            get { 
                return this.Availability == AvailabilityEnum.Available; 
            } 
        }

    }
}
