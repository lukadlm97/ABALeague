using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class Position:EnumBaseEntity
    {
       

        [Required, MaxLength(25)]
        public string Name { get; set; }
    }
}
