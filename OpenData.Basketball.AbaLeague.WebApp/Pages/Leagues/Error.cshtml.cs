using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Leagues
{
    public class ErrorModel:PageModel
    {
        [TempData]
        public string ErrorMessage { get; set; }

    }
}
