﻿using TextSearchDemo.Trie.Interfaces;

namespace TextSearchDemo.Models
{
    public class Group : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
