﻿using System.Drawing;

namespace DTOs.BodyDtos
{
    public class PlayerBodyDto : BaseBody
    {
        public string? ShortName { get; set; }
        public string? FullName { get; set; }
        public string? PlayStoreLink { get; set; }
        public string? AppStoreLink { get; set; }
        public string? LinkDescription { get; set; }
        public string? Color { get; set; }
    }
}
