using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Domain.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(125)]
        public string Name { get; set; }

        public string CodeIso { get; set; }
        public string CodeIso2 { get; set; }
    }
}
