namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class AddAnotherNameViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string DraftAnotherName { get; set; }
        public IList<string> ExistingAnotherNames { get; set; }
    }
}
