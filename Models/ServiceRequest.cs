using System;
using System.ComponentModel.DataAnnotations;

namespace Project_11.Models
{
    public class ServiceRequest
    {
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Enrollment number is required.")]
        [Display(Name = "Enrollment No")]
        public string StudentEnrollment { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Service category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}