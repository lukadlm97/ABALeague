using System.ComponentModel.DataAnnotations;

namespace OpenData.Basetball.AbaLeague.Application.Model
{
    public class PersistenceSettings
    {
        public bool UseInMemory { get; set; } = false;
        [Required]
        public string Database { get; set; }
    }
}
