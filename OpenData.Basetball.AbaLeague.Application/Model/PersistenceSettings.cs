using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Application.Model
{
    public class PersistenceSettings
    {
        public bool UseInMemory { get; set; }
        public bool MSSQL { get; set; }
        public bool PostgreSQL { get; set; }
    }
}
