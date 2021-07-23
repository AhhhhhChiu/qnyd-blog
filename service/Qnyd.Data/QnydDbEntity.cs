using System;
using System.ComponentModel.DataAnnotations;

namespace Qnyd.Data
{
    public abstract class QnydDbEntity
    {
        protected QnydDbEntity()
        {
            Enable = true;
        }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public bool Enable { get; set; }
    }
}
