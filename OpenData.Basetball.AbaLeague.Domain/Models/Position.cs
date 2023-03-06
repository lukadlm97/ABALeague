using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Domain.Models
{
    public class Position
    {
        [Key]
        public short Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }
    }
}
