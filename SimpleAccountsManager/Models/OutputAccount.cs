using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAccountsManager.Models
{
    public class OutputAccount
    {
        public string Id { get; set; }
        public StoredAccountDetails StoredAccountDetails { get; set; }
        public string Password { get; set; }
    }
}
