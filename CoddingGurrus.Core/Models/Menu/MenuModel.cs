﻿
namespace CoddingGurrus.Core.Models.Menu
{
    public class MenuModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int? ParentId { get; set; }
        public int? MenuOrder { get; set; }
        public string? MenuImage { get; set; }
        public bool? Archived { get; set; }
        public bool IsShow { get; set; }
    }
}
