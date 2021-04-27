using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAccountsManager.Models
{
    public class StoredAccountDetails
    {
        [Key]
        public int DetailsId { get; set; }
        public int AccountId { get; set; }

        [Required(ErrorMessage = "You need to name your account")]
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Your account should have a brief description")]
        [StringLength(100, ErrorMessage = "Description length must be between {2} and {1}", MinimumLength = 2)]
        public string AccountDescription { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
