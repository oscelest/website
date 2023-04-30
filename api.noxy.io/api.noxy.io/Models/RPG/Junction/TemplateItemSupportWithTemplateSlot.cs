﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.noxy.io.Models.RPG;

namespace Database.Models.RPG.Junction
{
    [Table("$TemplateItemSupport-TemplateSlot")]
    public class TemplateItemSupportWithTemplateSlot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public required TemplateItemSupport TemplateItemSupport { get; set; }

        public required TemplateSlot TemplateSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
