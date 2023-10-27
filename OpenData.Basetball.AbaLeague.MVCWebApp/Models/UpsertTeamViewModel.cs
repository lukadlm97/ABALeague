using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basketball.AbaLeague.Application.DTOs.Country;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class UpsertTeamViewModel
    {
        public int? Id { get; set; } = null;
        public string Name { get; set; }
        [StringLength(3)]
        [DisplayName("Short code")]
        public string ShortCode { get; set; }
        public string SelectedCountryId { get; set; }
        public SelectList Countries { get; set; }
    }
}
