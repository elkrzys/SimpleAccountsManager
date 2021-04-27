using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAccountsManager.Models
{
    public class StoredAccount
    {
        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }
    }
}
