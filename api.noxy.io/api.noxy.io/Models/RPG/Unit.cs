﻿using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Unit
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Template.Unit TemplateUnit { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required int Experience { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Mappings
        public required List<UnitWithEquipmentInSlot> UnitEquipmentSlotList { get; set; }

        // Implementations

        [Table("UnitInitiated")]
        public class Initiated : Unit
        {
            [Required]
            public DateTime TimeInitiated { get; set; } = DateTime.UtcNow;
        }

        [Table("UnitAvailable")]
        public class Available : Unit
        {

        }

    }
}
