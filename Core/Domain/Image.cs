﻿namespace Core.Domain
{
    public class Image : EntityBase
    {
        public byte[]? Content { get; set; }
        public int? CategoryId { get; set; }
        public int? PlayerId { get; set; }
        public Category? Category { get; set; }
        public Player? Player { get; set; }
    }
}
