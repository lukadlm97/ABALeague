using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class Player:AuditEntity
    {

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
        public ICollection<AnotherNameItem> AnotherNameItems { get; set; }
    }
}
