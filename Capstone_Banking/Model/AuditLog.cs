using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Action is required.")]
        [StringLength(100, ErrorMessage = "Action must be at most 100 characters long.")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime Timestamp { get; set; }

        [StringLength(500, ErrorMessage = "Details must be at most 500 characters long.")]
        public string Details { get; set; }
    }
