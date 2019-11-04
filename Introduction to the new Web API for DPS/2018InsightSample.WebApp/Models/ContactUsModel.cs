using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InsightAPISample.WebApp.Models
{
    public class ContactUsModel
    {
        [DisplayName("First Name"), Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name"), Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [DisplayName("Email"), DataType(DataType.EmailAddress),
         Required(AllowEmptyStrings = false, ErrorMessage = "We will need your email to contact you")]
        public string Email { get; set; }

        [DisplayName("Telehone"), DataType(DataType.PhoneNumber)] public string Phone { get; set; }
        [DisplayName("Your Title")] public string Title { get; set; }
        [DisplayName("Your Company")] public string FirmDescription { get; set; }
        [DisplayName("How Can We Help You?"), DataType(DataType.MultilineText)] public string Memo { get; set; }
    }
}
