using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Repository;
using GSCrm.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.CommonConsts;
using static GSCrm.Repository.AccountRepository;

namespace GSCrm.Controllers
{
    [Authorize]
    [Route(ACCOUNT)]
    public class AccountController
        : MainController<Account, AccountViewModel, AccountValidatior, AccountTransformer, AccountRepository>
    {
        public AccountController(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager)
            : base(context, viewsInfo, resManager, new AccountTransformer(context, resManager), new AccountRepository(context, viewsInfo, resManager))
        { }

        [HttpGet("ListOfAllAccounts/{pageNumber}")]
        public ViewResult AllAccounts(int pageNumber)
        {
            AccountRepository accountRepository = new AccountRepository(context, viewsInfo, resManager, HttpContext);
            AccountsViewModel accountsViewModel = new AccountsViewModel();
            transformer.InitializeAccountsViewModel(accountsViewModel, HttpContext);
            accountRepository.SetViewInfo(ALL_ACCS, pageNumber);
            accountRepository.AttachAccounts(ref accountsViewModel);
            return View(ACCOUNTS, accountsViewModel);
        }

        [HttpGet("ListOfCurrentAccounts/{pageNumber}")]
        public ViewResult CurrentAccounts(int pageNumber)
        {
            AccountRepository accountRepository = new AccountRepository(context, viewsInfo, resManager, HttpContext);
            AccountsViewModel accountsViewModel = new AccountsViewModel();
            transformer.InitializeAccountsViewModel(accountsViewModel, HttpContext);
            accountRepository.SetViewInfo(CURRENT_ACCS, pageNumber);
            accountRepository.AttachAccounts(ref accountsViewModel);
            return View(ACCOUNTS, accountsViewModel);
        }

        [HttpGet("BackToAccounts")]
        public IActionResult BackToAccounts()
        {
            // Возврат назад в зависимости от того, на какой вкладке находился пользователь перед проваливанием в карточку
            return SelectedAccountsTab switch
            {
                // Проваливание со списка всех клиентов
                ALL_ACCS => RedirectToAction(ALL_ACCS, ACCOUNT, new { pageNumber = viewsInfo.Get(ALL_ACCS)?.CurrentPageNumber }),

                // Проваливание со списка клиентов основной организации текущего пользователя
                _ => RedirectToAction(CURRENT_ACCS, ACCOUNT, new { pageNumber = viewsInfo.Get(CURRENT_ACCS)?.CurrentPageNumber }),
            };
        }

        [HttpGet("GetManagers/{accId}/{managerPart}")]
        public IActionResult GetManagers(string accId, string managerPart)
        {
            if (!string.IsNullOrEmpty(accId) && !string.IsNullOrEmpty(managerPart) && Guid.TryParse(accId, out Guid accountId))
            {
                Account account = context.Accounts
                    .Include(acc => acc.AccountManagers)
                        .ThenInclude(man => man.Manager)
                    .FirstOrDefault(i => i.Id == accountId);
                if (account?.AccountManagers.Count > 0)
                {
                    List<EmployeeViewModel> employeeViewModels = account.AccountManagers.Select(man => man.Manager).ToList()
                        .TransformToViewModels<Employee, EmployeeViewModel, EmployeeTransformer>(
                            transformer: new EmployeeTransformer(context, resManager),
                            limitingFunc: n => n.GetFullName().ToLower().Contains(managerPart.ToLower().TrimStartAndEnd()));
                    return Json(employeeViewModels);
                }
                return Json("");
            }
            return Json("");
        }

        [HttpGet("{id}")]
        public ViewResult Account(string id)
        {
            if (!repository.TryGetItemById(id, out Account account))
                return View("Error");

            AccountRepository accountRepository = new AccountRepository(context, viewsInfo, resManager, HttpContext);
            AccountTransformer accountTransformer = new AccountTransformer(context, resManager, HttpContext);
            AccountViewModel accountViewModel = accountTransformer.DataToViewModel(account);
            accountViewModel = accountTransformer.UpdateViewModelFromCash(accountViewModel);
            accountRepository.AttachContacts(accountViewModel);
            accountRepository.AttachAddresses(accountViewModel);
            accountRepository.AttachInvoices(accountViewModel);
            accountRepository.AttachQuotes(accountViewModel);
            accountRepository.AttachManagers(accountViewModel);
            CurrentAccount = accountViewModel;
            return View(ACCOUNT, accountViewModel);
        }

        [HttpGet("/GetAccount/{id}")]
        public IActionResult GetAccount(string id, string selectedAccountsTab)
        {
            SelectedAccountsTab = selectedAccountsTab;
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id });
        }

        [HttpGet("HasAccNotLegalAddress/{id}")]
        public IActionResult HasAccNotLegalAddress(string id)
        {
            if (!repository.TryGetItemById(id, out Account account))
                return Json(false);

            if (account.GetAddresses(context).Count <= 1)
                return Json(false);
            return Json(true);
        }

        [HttpPost("Create")]
        public override IActionResult Create(AccountViewModel accountViewModel)
        {
            ModelStateDictionary modelState = ModelState;
            AccountRepository accountRepository = new AccountRepository(context, viewsInfo, resManager, HttpContext);
            if (accountRepository.TryCreate(ref accountViewModel, modelState))
                return Json(Url.Action(ACCOUNT, new { id = accountRepository.newRecord.Id.ToString() }));
            return BadRequest(modelState);
        }

        [HttpPost("Update")]
        public override IActionResult Update(AccountViewModel accountViewModel)
        {
            ModelStateDictionary modelState = ModelState;
            AccountRepository accountRepository = new AccountRepository(context, viewsInfo, resManager, HttpContext);
            if (accountRepository.TryUpdate(ref accountViewModel, modelState))
                return Json(Url.Action(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id }));
            return BadRequest(modelState);
        }

        [HttpGet("ChangeSite/{accountId}/{newSite?}")]
        public IActionResult ChangeSite(string accountId, string newSite = null)
        {
            ModelStateDictionary modelState = ModelState;
            if (!repository.TryChangeSite(accountId, out Dictionary<string, string> errors, newSite))
            {
                foreach (KeyValuePair<string, string> error in errors)
                    modelState.AddModelError(error.Key, error.Value);
                return BadRequest(modelState);
            }
            return Ok();
        }

        [HttpPost("SearchAllAccounts")]
        public IActionResult SearchAllAccounts(AccountsViewModel accountsViewModel)
        {
            repository.SearchAllAccounts(accountsViewModel);
            return RedirectToAction(ALL_ACCS, ACCOUNT, new { pageNumber = DEFAULT_MIN_PAGE_NUMBER });
        }

        [HttpGet("ClearAllAccountsSearch")]
        public IActionResult ClearAllAccountsSearch()
        {
            repository.ClearAllAccountsSearch();
            return RedirectToAction(ALL_ACCS, ACCOUNT, new { pageNumber = DEFAULT_MIN_PAGE_NUMBER });
        }

        [HttpPost("SearchCurrentAccounts")]
        public IActionResult SearchCurrentAccounts(AccountsViewModel accountsViewModel)
        {
            repository.SearchCurrentAccounts(accountsViewModel);
            return RedirectToAction(CURRENT_ACCS, ACCOUNT, new { pageNumber = DEFAULT_MIN_PAGE_NUMBER });
        }

        [HttpGet("ClearCurrentAccountsSearch")]
        public IActionResult ClearCurrentAccountsSearch()
        {
            repository.ClearCurrentAccountsSearch();
            return RedirectToAction(CURRENT_ACCS, ACCOUNT, new { pageNumber = DEFAULT_MIN_PAGE_NUMBER });
        }

        [HttpPost("ChangePrimaryContact")]
        public IActionResult ChangePrimaryContact(AccountViewModel accountViewModel)
        {
            Account account = context.Accounts.Include(acCon => acCon.AccountContacts).FirstOrDefault(i => i.Id == CurrentAccount.Id);
            ModelStateDictionary modelState = ModelState;
            if (repository.TryChangePrimaryContact(accountViewModel, account, modelState))
                return Json("");
            return BadRequest(modelState);
        }

        [HttpPost("SearchContact")]
        public IActionResult SearchContact(AccountViewModel accountViewModel)
        {
            repository.SearchContact(accountViewModel);
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpGet("ClearContactSearch")]
        public IActionResult ClearContactSearch()
        {
            repository.ClearContactSearch();
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpPost("SearchAddress")]
        public IActionResult SearchAddress(AccountViewModel accountViewModel)
        {
            repository.SearchAddress(accountViewModel);
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpGet("ClearAddressSearch")]
        public IActionResult ClearAddressSearch()
        {
            repository.ClearAddressSearch();
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpPost("SearchInvoice")]
        public IActionResult SearchInvoice(AccountViewModel accountViewModel)
        {
            repository.SearchInvoice(accountViewModel);
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpGet("ClearInvoiceSearch")]
        public IActionResult ClearInvoiceSearch()
        {
            repository.ClearInvoiceSearch();
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpPost("SearchQuote")]
        public IActionResult SearchQuote(AccountViewModel accountViewModel)
        {
            repository.SearchQuote(accountViewModel);
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpGet("ClearQuoteSearch")]
        public IActionResult ClearQuoteSearch()
        {
            repository.ClearQuoteSearch();
            return RedirectToAction(ACCOUNT, ACCOUNT, new { id = CurrentAccount.Id });
        }

        [HttpPost("ChangeLegalAddress")]
        public IActionResult ChangeLegalAddress(AccountAddressViewModel addressViewModel)
        {
            if (!repository.TryChangeLegalAddress(addressViewModel, out Dictionary<string, string> errors))
                return BadRequest(errors);
            return Json("");
        }

        [HttpPost("AddAccountManager")]
        public IActionResult AddAccountManager(AccountViewModel accountViewModel)
        {
            if (!repository.TryAddAccountManager(accountViewModel, out Dictionary<string, string> errors))
                return BadRequest(errors);
            return Json("");
        }
    }
}
