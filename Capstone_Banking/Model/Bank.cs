using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class Bank
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters long.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must be at most 500 characters long.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Contact Person is required.")]
        [StringLength(100, ErrorMessage = "Contact Person must be at most 100 characters long.")]
        public string ContactPerson { get; set; }

        [StringLength(300, ErrorMessage = "Address must be at most 300 characters long.")]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = "Contact Info must be at most 100 characters long.")]
        public string ContactInfo { get; set; }
    }
}
