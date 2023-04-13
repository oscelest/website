﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("ItemMaterial")]
    public class ItemMaterial : Item
    {
        [Required]
        public int Count { get; set; }
    }
}