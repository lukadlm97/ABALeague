﻿namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public decimal Height { get; set; }
        public string Country { get; set; }
        public string Position { get; set; }
        public string PositionColor { get; set; }
    }
}
