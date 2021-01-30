using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gestionecentralino.Core;

namespace gestionecentralino.Db
{
    public class PhoneCallLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        
        [Required]
        public string InternalNumber { get; set; }
        
        [Required]
        public string ExternalNumber { get; set; }
        
        [Required]
        public string CoCode { get; set; }

        public string CdCode { get; set; }
        
        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public Decimal Cost { get; set; }

        [Required]
        public bool Incoming { get; set; }

        [Required]
        public SedeEnum Sede { get; set; }
    }
}