using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAccountsManager.Models;
using SimpleAccountsManager.Data;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using X.PagedList;

namespace SimpleAccountsManager.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IDataProtector Protector;

        public ContentController(ApplicationDbContext _dbContext, UserManager<ApplicationUser> _userManager, IDataProtectionProvider _provider)
        {
            DbContext = _dbContext;
            UserManager = _userManager;
            Protector = _provider.CreateProtector(GetType().FullName);
        }
        public IActionResult Index(int? page)
        {
            var user = User.Identity;

            List<OutputAccount> accountsList = new List<OutputAccount>();
            List<StoredAccount> dbAccounts = new List<StoredAccount>();
            if (user.IsAuthenticated)
            {
                foreach (var account in DbContext.StoredAccounts.ToList())
                {
                    if(account.UserId.Equals(int.Parse(UserManager.GetUserId(User))))
                    {
                        dbAccounts.Add(account);

                        var accountDetails = DbContext.StoredAccountsDetails
                            .Where(a => a.AccountId == account.AccountId)
                            .FirstOrDefault();

                        var accountPassword = DbContext.StoredPasswords
                            .Where(password => password.AccountId == account.AccountId)
                            .FirstOrDefault();

                        var decryptedPassword = 
                            (accountPassword.Password != null) && (accountPassword.Password != "") && (accountPassword.Password != " ") ? 
                            CredentialsProcessor.Decrypt(accountPassword.Password, user.Name) : " ";

                        var accountOutput = new OutputAccount()
                        {
                            Id = Protector.Protect(account.AccountId.ToString()),
                            StoredAccountDetails = accountDetails,
                            Password = decryptedPassword
                        };

                        accountsList.Add(accountOutput);
                    }   
                }
            
                ViewBag.UserName = user.Name;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(accountsList.ToPagedList(pageNumber, pageSize));

        }

        // GET method to display View only
        public IActionResult Create()
        {
            return View();
        }

        // POST form action for creating user
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(OutputAccount newAccount)
        {
            if (ModelState.IsValid)
            {
                StoredAccount storedAccount = new StoredAccount
                {
                    UserId = int.Parse(UserManager.GetUserId(User))
                };

                DbContext.StoredAccounts.Add(storedAccount);
                DbContext.SaveChanges();

                int currentAccountId = storedAccount.AccountId;

                StoredAccountDetails storedAccountDetails = newAccount.StoredAccountDetails;
                storedAccountDetails.AccountId = currentAccountId;

                
                string encryptedPassword = (newAccount.Password != null) && (newAccount.Password != "") && (newAccount.Password != " ") ? 
                    CredentialsProcessor.Encrypt(newAccount.Password, User.Identity.Name) : " ";

                StoredAccountPassword storedAccountPassword = new StoredAccountPassword
                {
                    AccountId = currentAccountId,
                    Password = encryptedPassword
                };

                DbContext.StoredPasswords.Add(storedAccountPassword);
                DbContext.StoredAccountsDetails.Add(storedAccountDetails);
                DbContext.SaveChanges();

                return RedirectToAction("Index");
            }
           
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }

            int decryptedId = Convert.ToInt32(Protector.Unprotect(id));

            if (DbContext.StoredAccounts.Find(decryptedId) != null)
            {
                var detailsObj = DbContext.StoredAccountsDetails
                                            .Where(details => details.AccountId == decryptedId)
                                            .FirstOrDefault();
                var passwordObj = DbContext.StoredPasswords
                                            .Where(password => password.AccountId == decryptedId)
                                            .FirstOrDefault();

                OutputAccount outputAccount = new OutputAccount
                {
                    StoredAccountDetails = detailsObj,
                    Password = CredentialsProcessor.Decrypt(passwordObj.Password, User.Identity.Name)
                };

                return View(outputAccount);
            }
            else
                return NotFound();
            
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(OutputAccount editedAccount)
        {
            int id = editedAccount.StoredAccountDetails.AccountId;

            DbContext.StoredAccountsDetails.Update(editedAccount.StoredAccountDetails);

            string newPassword = (editedAccount.Password != null) && (editedAccount.Password != "") && (editedAccount.Password != " ") ?
                CredentialsProcessor.Encrypt(editedAccount.Password, User.Identity.Name) : " ";

            var passwordObj = DbContext.StoredPasswords
                .Where(password => password.AccountId == editedAccount.StoredAccountDetails.AccountId)
                .FirstOrDefault();

            if (passwordObj == null) 
                return NotFound();

            passwordObj.Password = newPassword;

            DbContext.StoredPasswords.Update(passwordObj);
            DbContext.SaveChanges();
                                                        
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            if(id == null || id == "")
            {
                return NotFound();
            }

            int decryptedId = int.Parse(Protector.Unprotect(id));

            var account = DbContext.StoredAccounts.Find(decryptedId);

            if (account == null)
                return NotFound();

            var accountDetails = DbContext.StoredAccountsDetails
                .Where(details => details.AccountId == decryptedId)
                .FirstOrDefault();

            var passwordObj = DbContext.StoredPasswords
                .Where(password => password.AccountId == decryptedId)
                .FirstOrDefault();

            DbContext.StoredAccounts.Remove(account);
            DbContext.StoredAccountsDetails.Remove(accountDetails);
            DbContext.StoredPasswords.Remove(passwordObj);

            DbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
