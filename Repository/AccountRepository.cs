using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.CommonConsts;

namespace GSCrm.Repository
{
    public class AccountRepository : GenericRepository<Account, AccountViewModel, AccountValidatior, AccountTransformer>
    {
        private readonly User currentUser;
        public static AccountViewModel CurrentAccount { get; set; }
        public static string SelectedAccountsTab { get; set; }
        public static Account NewAccount { get; private set; }
        public AccountRepository(ApplicationDbContext context, ResManager resManager)
            : base(context, resManager, new AccountValidatior(context, resManager), new AccountTransformer(context, resManager))
        { }

        public AccountRepository(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager, HttpContext httpContext = null)
            : base(context, viewsInfo, resManager, new AccountValidatior(context, resManager, httpContext), new AccountTransformer(context, resManager, httpContext))
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        #region Override Methods
        public override void FailureUpdateHandler(AccountViewModel accountViewModel, Action<AccountViewModel> handler = null)
        {
            if (TryGetItemById(accountViewModel.Id, out Account account))
            {
                accountViewModel = transformer.DataToViewModel(account);
                accountViewModel = transformer.UpdateViewModelFromCash(accountViewModel);
                AttachManagers(accountViewModel);
                AttachContacts(accountViewModel);
                AttachAddresses(accountViewModel);
                AttachInvoices(accountViewModel);
                AttachQuotes(accountViewModel);
            }
        }
        #endregion

        #region Searching

        /// <summary>
        /// Метод устанавливает значения для поиска по всем клиентам
        /// </summary>
        /// <param name="accountsViewModel"></param>
        /// <returns></returns>
        public void SearchAllAccounts(AccountsViewModel accountsViewModel)
        {
            viewsInfo.Reset(ALL_ACCS);
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(ALL_ACCS);
            accountsViewModelCash.AllAccountsSearchName = accountsViewModel.AllAccountsSearchName?.ToLower().TrimStartAndEnd();
            accountsViewModelCash.AllAccountsSearchType = accountsViewModel.AllAccountsSearchType;
            ModelCash<AccountsViewModel>.SetViewModel(ALL_ACCS, accountsViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по всем клиентам
        /// </summary>
        public void ClearAllAccountsSearch()
        {
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(ALL_ACCS);
            accountsViewModelCash.AllAccountsSearchName = default;
            accountsViewModelCash.AllAccountsSearchType = default;
            ModelCash<AccountsViewModel>.SetViewModel(ALL_ACCS, accountsViewModelCash);
        }

        /// <summary>
        /// Метод устанавливает значения для поиска по клиентам основной организации пользователя
        /// </summary>
        /// <param name="accountsViewModel"></param>
        /// <returns></returns>
        public void SearchCurrentAccounts(AccountsViewModel accountsViewModel)
        {
            viewsInfo.Reset(CURRENT_ACCS);
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(CURRENT_ACCS);
            accountsViewModelCash.CurrentAccountsSearchName = accountsViewModel.CurrentAccountsSearchName?.ToLower().TrimStartAndEnd();
            accountsViewModelCash.CurrentAccountsSearchType = accountsViewModel.CurrentAccountsSearchType;
            ModelCash<AccountsViewModel>.SetViewModel(CURRENT_ACCS, accountsViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по клиентам основной организации пользователя
        /// </summary>
        public void ClearCurrentAccountsSearch()
        {
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(CURRENT_ACCS);
            accountsViewModelCash.CurrentAccountsSearchName = default;
            accountsViewModelCash.CurrentAccountsSearchType = default;
            ModelCash<AccountsViewModel>.SetViewModel(CURRENT_ACCS, accountsViewModelCash);
        }

        /// <summary>
        /// Поиск по контактам клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void SearchContact(AccountViewModel accountViewModel)
        {
            viewsInfo.Reset(ACC_CONTACTS);
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_CONTACTS);
            accountViewModelCash.Id = accountViewModel.Id;
            accountViewModelCash.SearchContactFullName = accountViewModel.SearchContactFullName?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchContactType = accountViewModel.SearchContactType;
            accountViewModelCash.SearchContactEmail = accountViewModel.SearchContactEmail?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchContactPhoneNumber = accountViewModel.SearchContactPhoneNumber?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchContactPrimary = accountViewModel.SearchContactPrimary;
            ModelCash<AccountViewModel>.SetViewModel(ACC_CONTACTS, accountViewModelCash);
        }

        /// <summary>
        /// Очистка поиска по контактам клиента
        /// </summary>
        public void ClearContactSearch()
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_CONTACTS);
            accountViewModelCash.Id = default;
            accountViewModelCash.SearchContactFullName = default;
            accountViewModelCash.SearchContactType = default;
            accountViewModelCash.SearchContactEmail = default;
            accountViewModelCash.SearchContactPhoneNumber = default;
            accountViewModelCash.SearchContactPrimary = default;
            ModelCash<AccountViewModel>.SetViewModel(ACC_CONTACTS, accountViewModelCash);
        }

        /// <summary>
        /// Поиск по адресам клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void SearchAddress(AccountViewModel accountViewModel)
        {
            viewsInfo.Reset(ACC_ADDRESSES);
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_ADDRESSES);
            accountViewModelCash.SearchAddressType = accountViewModel.SearchAddressType;
            accountViewModelCash.SearchAddressCountry = accountViewModel.SearchAddressCountry?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchAddressRegion = accountViewModel.SearchAddressRegion?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchAddressCity = accountViewModel.SearchAddressCity?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchAddressStreet = accountViewModel.SearchAddressStreet?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchAddressHouse = accountViewModel.SearchAddressHouse?.ToLower().TrimStartAndEnd();
            ModelCash<AccountViewModel>.SetViewModel(ACC_ADDRESSES, accountViewModelCash);
        }

        /// <summary>
        /// Очистка поиска по адресам клиента
        /// </summary>
        public void ClearAddressSearch()
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_ADDRESSES);
            accountViewModelCash.SearchAddressType = default;
            accountViewModelCash.SearchAddressCountry = default;
            accountViewModelCash.SearchAddressRegion = default;
            accountViewModelCash.SearchAddressCity = default;
            accountViewModelCash.SearchAddressStreet = default;
            accountViewModelCash.SearchAddressHouse = default;
            ModelCash<AccountViewModel>.SetViewModel(ACC_ADDRESSES, accountViewModelCash);
        }

        /// <summary>
        /// Поиск по банкосвким реквизитам клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void SearchInvoice(AccountViewModel accountViewModel)
        {
            viewsInfo.Reset(ACC_INVOICES);
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_INVOICES);
            accountViewModelCash.SearchInvoiceBankName = accountViewModel.SearchInvoiceBankName?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchInvoiceCity = accountViewModel.SearchInvoiceCity?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchInvoiceCheckingAccount = accountViewModel.SearchInvoiceCheckingAccount?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchInvoiceCorrespondentAccount = accountViewModel.SearchInvoiceCorrespondentAccount?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchInvoiceBIC = accountViewModel.SearchInvoiceBIC?.ToLower().TrimStartAndEnd();
            accountViewModelCash.SearchInvoiceSWIFT = accountViewModel.SearchInvoiceSWIFT?.ToLower().TrimStartAndEnd();
            ModelCash<AccountViewModel>.SetViewModel(ACC_INVOICES, accountViewModelCash);
        }

        /// <summary>
        /// Очистка поиска по банкосвким реквизитам клиента
        /// </summary>
        public void ClearInvoiceSearch()
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_INVOICES);
            accountViewModelCash.SearchInvoiceBankName = default;
            accountViewModelCash.SearchInvoiceCity = default;
            accountViewModelCash.SearchInvoiceCheckingAccount = default;
            accountViewModelCash.SearchInvoiceCorrespondentAccount = default;
            accountViewModelCash.SearchInvoiceBIC = default;
            accountViewModelCash.SearchInvoiceSWIFT = default;
            ModelCash<AccountViewModel>.SetViewModel(ACC_INVOICES, accountViewModelCash);
        }

        /// <summary>
        /// Поиск по сделкам клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void SearchQuote(AccountViewModel accountViewModel)
        {
            viewsInfo.Reset(ACC_QUOTES);
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_QUOTES);
            ModelCash<AccountViewModel>.SetViewModel(ACC_QUOTES, accountViewModelCash);
        }

        /// <summary>
        /// Очистка поиска по сделкам клиента
        /// </summary>
        public void ClearQuoteSearch()
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_QUOTES);
            ModelCash<AccountViewModel>.SetViewModel(ACC_QUOTES, accountViewModelCash);
        }

        #endregion

        #region Attaching Accounts

        /// <summary>
        /// Метод добавляет список моделей отображения клиента к модели "AccountsViewModel"
        /// </summary>
        /// <param name="accountsViewModel"></param>
        public void AttachAccounts(ref AccountsViewModel accountsViewModel)
        {
            accountsViewModel.AllAccounts = context.GetAllAccounts(currentUser)
                .TransformToViewModels<Account, AccountViewModel, AccountTransformer>(
                    transformer: new AccountTransformer(context, resManager),
                    limitingFunc: GetLimitedAllAccountsList);

            accountsViewModel.CurrentAccounts = context.GetCurrentAccounts(currentUser)
                .TransformToViewModels<Account, AccountViewModel, AccountTransformer>(
                    transformer: new AccountTransformer(context, resManager),
                    limitingFunc: GetLimitedCurrentAccountsList);
        }

        /// <summary>
        /// Метод ограничивает список клиентов всех организаций
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        private List<Account> GetLimitedAllAccountsList(List<Account> accounts)
        {
            List<Account> limitedAccounts = accounts;
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(ALL_ACCS);
            LimitAllAccsBySearchName(ref limitedAccounts, accountsViewModelCash);
            LimitAllAccsBySearchType(ref limitedAccounts, accountsViewModelCash);
            LimitListByPageNumber(ALL_ACCS, ref limitedAccounts);
            return limitedAccounts;
        }

        /// <summary>
        /// Ограничение списка всех клиентов по названию
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="accountsToLimit"></param>
        private void LimitAllAccsBySearchName(ref List<Account> accountsToLimit, AccountsViewModel accountsViewModel)
        {
            if (!string.IsNullOrEmpty(accountsViewModel.AllAccountsSearchName))
                accountsToLimit = accountsToLimit.Where(n => n.Name.ToLower().Contains(accountsViewModel.AllAccountsSearchName)).ToList();
        }

        /// <summary>
        /// Ограничение списка всех клиентов по типу
        /// </summary>
        /// <param name="accountsToLimit"></param>
        private void LimitAllAccsBySearchType(ref List<Account> accountsToLimit, AccountsViewModel accountsViewModel)
        {
            if (!string.IsNullOrEmpty(accountsViewModel.AllAccountsSearchType))
                accountsToLimit = accountsToLimit.Where(n => n.AccountType.ToString() == accountsViewModel.AllAccountsSearchType).ToList();
        }

        /// <summary>
        /// Метод ограничивает список клиентов основной организации пользователя
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        private List<Account> GetLimitedCurrentAccountsList(List<Account> accounts)
        {
            List<Account> limitedAccounts = accounts.Where(orgId => orgId.OrganizationId == currentUser.PrimaryOrganizationId).ToList();
            AccountsViewModel accountsViewModelCash = ModelCash<AccountsViewModel>.GetViewModel(CURRENT_ACCS);
            LimitCurrentAccsBySearchName(ref limitedAccounts, accountsViewModelCash);
            LimitCurrentAccsBySearchType(ref limitedAccounts, accountsViewModelCash);
            LimitListByPageNumber(CURRENT_ACCS, ref limitedAccounts);
            return limitedAccounts;
        }

        /// <summary>
        /// Ограничение списка клиентов основной организации текущего пользователя по названию
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="accountsToLimit"></param>
        private void LimitCurrentAccsBySearchName(ref List<Account> accountsToLimit, AccountsViewModel accountsViewModel)
        {
            if (!string.IsNullOrEmpty(accountsViewModel.CurrentAccountsSearchName))
                accountsToLimit = accountsToLimit.Where(n => n.Name.ToLower().Contains(accountsViewModel.CurrentAccountsSearchName)).ToList();
        }

        /// <summary>
        /// Ограничение списка клиентов основной орагнизации пользователя по типу
        /// </summary>
        /// <param name="accountsToLimit"></param>
        private void LimitCurrentAccsBySearchType(ref List<Account> accountsToLimit, AccountsViewModel accountsViewModel)
        {
            if (!string.IsNullOrEmpty(accountsViewModel.CurrentAccountsSearchType))
                accountsToLimit = accountsToLimit.Where(n => n.AccountType.ToString() == accountsViewModel.CurrentAccountsSearchType).ToList();
        }

        #endregion

        #region Attaching Contacts

        /// <summary>
        /// Добавляет контакты к клиенту
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void AttachContacts(AccountViewModel accountViewModel)
        {
            accountViewModel.AccountContacts = accountViewModel.GetContacts(context)
                .TransformToViewModels<AccountContact, AccountContactViewModel, AccountContactTransformer>(
                    transformer: new AccountContactTransformer(context, resManager),
                    limitingFunc: GetLimitedAccContactsList);
        }

        private List<AccountContact> GetLimitedAccContactsList(List<AccountContact> accountContacts)
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_CONTACTS);
            List<AccountContact> limitedAccContacts = accountContacts;
            LimitContactsByFullName(ref limitedAccContacts, accountViewModelCash);
            LimitContactsByType(ref limitedAccContacts, accountViewModelCash);
            LimitContactsByPhoneNumber(ref limitedAccContacts, accountViewModelCash);
            LimitContactsByEmail(ref limitedAccContacts, accountViewModelCash);
            LimitContactsByPrimarySign(ref limitedAccContacts, accountViewModelCash);
            LimitListByPageNumber(ACC_CONTACTS, ref limitedAccContacts);
            return limitedAccContacts;
        }

        /// <summary>
        /// Мето ограничивает список контактов клиента по полному имеени
        /// </summary>
        /// <param name="accountContactsToLimit"></param>
        private void LimitContactsByFullName(ref List<AccountContact> accountContactsToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchContactFullName))
                accountContactsToLimit = accountContactsToLimit.Where(accCon => accCon.GetFullName().ToLower().Contains(accountViewModel.SearchContactFullName)).ToList();
        }

        /// <summary>
        /// Мето ограничивает список контактов клиента по типу контакта
        /// </summary>
        /// <param name="accountContactsToLimit"></param>
        private void LimitContactsByType(ref List<AccountContact> accountContactsToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchContactType))
            {
                Func<AccountContact, bool> predicate = accCon => accCon.ContactType == (ContactType)Enum.Parse(typeof(ContactType), accountViewModel.SearchContactType);
                accountContactsToLimit = accountContactsToLimit.Where(predicate).ToList();
            }
        }

        /// <summary>
        /// Мето ограничивает список контактов клиента по телефону
        /// </summary>
        /// <param name="accountContactsToLimit"></param>
        private void LimitContactsByPhoneNumber(ref List<AccountContact> accountContactsToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchContactPhoneNumber))
                accountContactsToLimit = accountContactsToLimit.Where(accCon => accCon.PhoneNumber.ToLower().Contains(accountViewModel.SearchContactPhoneNumber)).ToList();
        }

        /// <summary>
        /// Мето ограничивает список контактов клиента по почту
        /// </summary>
        /// <param name="accountContactsToLimit"></param>
        private void LimitContactsByEmail(ref List<AccountContact> accountContactsToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchContactEmail))
                accountContactsToLimit = accountContactsToLimit.Where(accCon => accCon.PhoneNumber.ToLower().Contains(accountViewModel.SearchContactEmail)).ToList();
        }

        /// <summary>
        /// Мето ограничивает список контактов клиента по признаку основного
        /// </summary>
        /// <param name="accountContactsToLimit"></param>
        private void LimitContactsByPrimarySign(ref List<AccountContact> accountContactsToLimit, AccountViewModel accountViewModel)
        {
            Account account = context.Accounts.FirstOrDefault(i => i.Id == accountViewModel.Id);
            if (accountViewModel.SearchContactPrimary)
                accountContactsToLimit = accountContactsToLimit.Where(i => i.Id == account.PrimaryContactId).ToList();
        }

        #endregion

        #region Attaching Addresses

        /// <summary>
        /// Добавляет адреса к клиенту
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void AttachAddresses(AccountViewModel accountViewModel)
        {
            List<AccountAddress> accountAddresses = accountViewModel.GetAddresses(context);
            accountViewModel.AccountAddresses = accountAddresses.TransformToViewModels
                <AccountAddress, AccountAddressViewModel, AccountAddressTransformer>(
                    transformer: new AccountAddressTransformer(context, resManager, currentUser),
                    limitingFunc: GetLimitedAccAddressesList);
            accountViewModel.AllAccountAddresses = accountAddresses.GetViewModelsFromData
                <AccountAddress, AccountAddressViewModel, AccountAddressTransformer>(new AccountAddressTransformer(context, resManager, currentUser));
        }

        private List<AccountAddress> GetLimitedAccAddressesList(List<AccountAddress> accountAddresses)
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_ADDRESSES);
            List<AccountAddress> limitedAccAddresses = accountAddresses;
            LimitAddressesByCountry(ref limitedAccAddresses, accountViewModelCash);
            LimitAddressesByRegion(ref limitedAccAddresses, accountViewModelCash);
            LimitAddressesByCity(ref limitedAccAddresses, accountViewModelCash);
            LimitAddressesByStreet(ref limitedAccAddresses, accountViewModelCash);
            LimitAddressesByHouse(ref limitedAccAddresses, accountViewModelCash);
            LimitAddressesByType(ref limitedAccAddresses, accountViewModelCash);
            LimitListByPageNumber(ACC_ADDRESSES, ref limitedAccAddresses);
            return limitedAccAddresses;
        }

        /// <summary>
        /// Ограничение списка адресов клиента по стране
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByCountry(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressCountry))
                accountAddresses = accountAddresses.Where(addr => addr.Country.ToLower().Contains(accountViewModel.SearchAddressCountry)).ToList();
        }

        /// <summary>
        /// Ограничение списка адресов клиента по региону
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByRegion(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressRegion))
                accountAddresses = accountAddresses.Where(addr => !string.IsNullOrEmpty(addr.Region) && addr.Region.ToLower().Contains(accountViewModel.SearchAddressRegion)).ToList();
        }

        /// <summary>
        /// Ограничение списка адресов клиента по городу
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByCity(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressCity))
                accountAddresses = accountAddresses.Where(addr => !string.IsNullOrEmpty(addr.City) && addr.City.ToLower().Contains(accountViewModel.SearchAddressCity)).ToList();
        }

        /// <summary>
        /// Ограничение списка адресов клиента по улице
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByStreet(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressStreet))
                accountAddresses = accountAddresses.Where(addr => !string.IsNullOrEmpty(addr.Street) && addr.Street.ToLower().Contains(accountViewModel.SearchAddressStreet)).ToList();
        }

        /// <summary>
        /// Ограничение списка адресов клиента по дому
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByHouse(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressHouse))
                accountAddresses = accountAddresses.Where(addr => !string.IsNullOrEmpty(addr.House) && addr.House.ToLower().Contains(accountViewModel.SearchAddressHouse)).ToList();
        }

        /// <summary>
        /// Ограничение списка адресов клиента по типу
        /// </summary>
        /// <param name="accountAddresses"></param>
        /// <param name="accountViewModel"></param>
        private void LimitAddressesByType(ref List<AccountAddress> accountAddresses, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchAddressType) && Enum.TryParse(typeof(AddressType), accountViewModel.SearchAddressType, out object addressType))
                accountAddresses = accountAddresses.Where(addr => addr.AddressType == (AddressType)addressType).ToList();
        }

        #endregion

        #region Attaching Invoices

        /// <summary>
        /// Добавляет банковские реквизиты к клиенту
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void AttachInvoices(AccountViewModel accountViewModel)
        {
            accountViewModel.AccountInvoices = accountViewModel.GetInvoices(context)
                .TransformToViewModels<AccountInvoice, AccountInvoiceViewModel, AccountInvoiceTransformer>(
                    transformer: new AccountInvoiceTransformer(context, resManager),
                    limitingFunc: GetLimitedAccInvoicesList);
        }

        private List<AccountInvoice> GetLimitedAccInvoicesList(List<AccountInvoice> accountInvoices)
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_INVOICES);
            List<AccountInvoice> limitedAccInvoices = accountInvoices;
            LimitInvoicesByBankName(ref limitedAccInvoices, accountViewModelCash);
            LimitInvoicesByCity(ref limitedAccInvoices, accountViewModelCash);
            LimitInvoicesByCheckingAccount(ref limitedAccInvoices, accountViewModelCash);
            LimitInvoicesByCorrespondentAccount(ref limitedAccInvoices, accountViewModelCash);
            LimitInvoicesByBIC(ref limitedAccInvoices, accountViewModelCash);
            LimitInvoicesBySWIFT(ref limitedAccInvoices, accountViewModelCash);
            LimitListByPageNumber(ACC_INVOICES, ref limitedAccInvoices);
            return limitedAccInvoices;
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по названию банка
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesByBankName(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceBankName))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceBankName)).ToList();
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по городу банка
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesByCity(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceCity))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceCity)).ToList();
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по расчетному счету
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesByCheckingAccount(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceCheckingAccount))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceCheckingAccount)).ToList();
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по корреспонденскому счету
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesByCorrespondentAccount(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceCorrespondentAccount))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceCorrespondentAccount)).ToList();
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по БИКу
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesByBIC(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceBIC))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceBIC)).ToList();
        }

        /// <summary>
        /// Ограничение списка банковских реквизитов по коду SWIFT
        /// </summary>
        /// <param name="accountInvoicesToLimit"></param>
        /// <param name="accountViewModel"></param>
        private void LimitInvoicesBySWIFT(ref List<AccountInvoice> accountInvoicesToLimit, AccountViewModel accountViewModel)
        {
            if (!string.IsNullOrEmpty(accountViewModel.SearchInvoiceSWIFT))
                accountInvoicesToLimit = accountInvoicesToLimit.Where(acc => acc.BankName.ToLower().Contains(accountViewModel.SearchInvoiceSWIFT)).ToList();
        }

        #endregion

        #region Attaching Quotes

        /// <summary>
        /// Добавляет сделки к клиенту
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void AttachQuotes(AccountViewModel accountViewModel)
        {

        }

        private List<AccountQuote> GetLimitedAccQuotesList(List<AccountQuote> accountQuotes)
        {
            AccountViewModel accountViewModelCash = ModelCash<AccountViewModel>.GetViewModel(ACC_QUOTES);
            List<AccountQuote> limitedAccQuotes = accountQuotes;
            LimitListByPageNumber(ACC_QUOTES, ref limitedAccQuotes);
            return limitedAccQuotes;
        }

        #endregion

        #region Attaching Managers

        /// <summary>
        /// Добавляет менеджеров к клиенту
        /// </summary>
        /// <param name="accountViewModel"></param>
        public void AttachManagers(AccountViewModel accountViewModel)
        {
            accountViewModel.AccountManagers = accountViewModel.GetManagers(context).GetViewModelsFromData
                <AccountManager, AccountManagerViewModel, AccountManagerTransformer>(new AccountManagerTransformer(context, resManager));
        }

        #endregion

        #region Other
        /// <summary>
        /// Метод пытается изменить основной контакт на клиенте
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool TryChangePrimaryContact(AccountViewModel accountViewModel, Account account, ModelStateDictionary modelState)
        {
            Dictionary<string, string> errors = validator.ChangePrimaryContactCheck(accountViewModel, account);
            if (errors.Count > 0)
            {
                errors.Keys.ToList().ForEach(error => modelState.AddModelError(error, errors[error]));
                return false;
            }

            // В зависимости от типа клиента
            switch (account.AccountType)
            {
                // Для физических лиц основной контакт является обязательным
                case AccountType.Individual:
                    Guid primaryContactId = Guid.Parse(accountViewModel.PrimaryContactId);
                    AccountContact primaryContact = context.AccountContacts.FirstOrDefault(i => i.Id == primaryContactId);
                    account.PrimaryContactId = primaryContactId;
                    account.Name = primaryContact.GetFullName();
                    break;

                // Для остальных типов клиентов основной контакт может отсутствовать
                case AccountType.IndividualEntrepreneur:
                    account.PrimaryContactId = string.IsNullOrEmpty(accountViewModel.PrimaryContactId) ? Guid.Empty : Guid.Parse(accountViewModel.PrimaryContactId);
                    break;
            }

            context.Accounts.Update(account);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Метод пытается изменить юридический адрес у клиента
        /// </summary>
        /// <param name="addressViewModel"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool TryChangeLegalAddress(AccountAddressViewModel addressViewModel, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            if (!TryGetItemById(addressViewModel.AccountId, out Account account))
            {
                if (!errors.ContainsKey("RecordNotFound"))
                    errors.Add("RecordNotFound", resManager.GetString("RecordNotFound"));
                return false;
            }

            errors = validator.ChangeLegalAddressCheck(addressViewModel, account);
            if (errors.Any()) return false;

            AccountAddress oldLegalAddress = account.GetAddresses(context).FirstOrDefault(addr => addr.AddressType == AddressType.Legal);
            oldLegalAddress.AddressType = (AddressType)Enum.Parse(typeof(AddressType), addressViewModel.CurrentAddressNewType);
            AccountAddress newLegalAddress = context.AccountAddresses.FirstOrDefault(i => i.Id == Guid.Parse(addressViewModel.NewLegalAddressId));
            newLegalAddress.AddressType = AddressType.Legal;
            context.AccountAddresses.UpdateRange(oldLegalAddress, newLegalAddress);
            context.SaveChanges();
            return true;
        }

        public bool TryChangeSite(string accountId, out Dictionary<string, string> errors, string newSite = null)
        {
            errors = new Dictionary<string, string>();
            if (!TryGetItemById(accountId, out Account account))
                errors.Add("AccountNotFound", resManager.GetString("AccountNotFound"));
            if (context.AccountManagers.FirstOrDefault(i => i.Id == account.PrimaryManagerId) == null)
                errors.Add("AccountWithoutKMIsReadonly", resManager.GetString("AccountWithoutKMIsReadonly"));
            else
            {
                try
                {
                    account.Site = newSite;
                    context.Accounts.Update(account);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    errors.Add(resManager.GetString("UnhandledException"), ex.Message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод пытается добавить в команду по клиенту нового менеджера
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool TryAddAccountManager(AccountViewModel accountViewModel, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            if (!TryGetItemById(accountViewModel.Id, out Account account))
                errors.Add("AccountNotFound", resManager.GetString("AccountNotFound"));
            if (!new EmployeeRepository(context, resManager).TryGetItemById(accountViewModel.NewPrimaryManagerId, out Employee employee))
                errors.Add("EmployeeNotExists", resManager.GetString("EmployeeNotExists"));
            else
            {
                try
                {
                    AccountManager accountManager = new AccountManager()
                    {
                        Id = Guid.NewGuid(),
                        Account = account,
                        AccountId = account.Id,
                        ManagerId = employee.Id
                    };
                    account.AccountManagers.Add(accountManager);
                    account.PrimaryManagerId = accountManager.Id;
                    account.AccountStatus = AccountStatus.Active;
                    context.Entry(account).State = EntityState.Modified;
                    context.Entry(accountManager).State = EntityState.Added;
                    context.Accounts.Update(account);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    errors.Add(resManager.GetString("UnhandledException"), ex.Message);
                }
            }
            return false;
        }
        #endregion
    }
}
