using OpenData.Basetball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class StatsProperty : EnumBaseEntity
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }
    }
}
