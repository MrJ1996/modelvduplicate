using System.ComponentModel.DataAnnotations;

namespace modelvduplicate.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public  string FirstName { get; set; }

        public  string MiddleName { get; set; }

        public  string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public  string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public  string EmailAddress { get; set; }

        [Required]
        public  string Gender { get; set; }

        [Required]
        public  string Country { get; set; }

        


    }
}
