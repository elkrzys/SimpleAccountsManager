using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAccountsManager.Models
{
    public class StoredAccountPassword
    {
        [Key]
        public int PasswordId { get; set; }
        public int AccountId { get; set; }
        public string Password { get; set; }
    }
}
