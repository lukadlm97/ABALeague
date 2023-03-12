using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Domain.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public int Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public virtual Position Position { get; set; }
        public short PositionId { get; set; }
        public virtual Country Country { get; set; }
        public int CountryId { get; set; }
        [NotMapped]
        public PositionEnum PositionEnum => (PositionEnum)PositionId;
    }
}
