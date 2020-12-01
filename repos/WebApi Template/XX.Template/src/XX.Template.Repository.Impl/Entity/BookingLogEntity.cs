using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XX.Template.Repository.Impl.Entity
{
    [Table("BookingLog")]
    public class BookingLogEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] [StringLength(50)] public string RecordLocator { get; set; }

        public DateTime Date { get; set; }

        [StringLength(250)] public string Email { get; set; }
    }
}