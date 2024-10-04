using System;
using System.ComponentModel.DataAnnotations;

public class Documents
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Upload date is required.")]
    public DateTime UploadedOn { get; set; }

    [Required(ErrorMessage = "Document name is required.")]
    [StringLength(200, ErrorMessage = "Document name must be at most 200 characters long.")]
    public string DocumentName { get; set; }

    [Required(ErrorMessage = "Document URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string DocumentUrl { get; set; }

    [Required(ErrorMessage = "Document type is required.")]
    [StringLength(100, ErrorMessage = "Document type must be at most 100 characters long.")]
    public string DocumentType { get; set; }
}

