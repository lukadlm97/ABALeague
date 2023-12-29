using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class AnotherNameItem : BaseEntity
    {
        public virtual Player Player { get; set; }
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }
}
