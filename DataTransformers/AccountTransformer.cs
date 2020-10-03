using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.CommonConsts;

namespace GSCrm.DataTransformers
{
    public class AccountTransformer : BaseTransformer<Account, AccountViewModel>
    {
        private readonly User currentUser;
        public AccountTransformer(ApplicationDbContext context, ResManager resManager, HttpContext httpContext = null)
            : base(context, resManager)
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        public override Account OnModelCreate(AccountViewModel accountViewModel)
        {
            // Поля клиента и список контактов
            string name = string.Empty;
            string kpp = string.Empty;
            string okpo = string.Empty;
            string ogrn = string.Empty;
            Guid newAccountId = Guid.NewGuid();
            Guid primaryContactId = Guid.Empty;
            List<AccountContact> accountContacts = new List<AccountContact>();

            // В зависимости от типа клиента инициализация переменных разными значениями
            AccountType accountType = (AccountType)Enum.Parse(typeof(AccountType), accountViewModel.AccountType);
            switch (accountType)
            {
                // Физическое лицо
                case AccountType.Individual:
                    name = accountViewModel.GetIndividualFullName();
                    AccountContact accountContact = GetNewAccountContact(accountViewModel, newAccountId);
                    primaryContactId = accountContact.Id;
                    accountContacts.Add(accountContact);
                    context.AccountContacts.Add(accountContact);
                    break;

                // ИП
                case AccountType.IndividualEntrepreneur:
                    name = accountViewModel.Name;
                    kpp = accountViewModel.KPP;
                    break;

                // Юридическое лицо
                case AccountType.LegalEntity:
                    name = accountViewModel.Name;
                    kpp = accountViewModel.KPP;
                    okpo = accountViewModel.OKPO;
                    ogrn = accountViewModel.OGRN;
                    break;
            }

            // Создание адреса - при любом типе клиента
            List<AccountAddress> accountAddresses = new List<AccountAddress>();
            AccountAddress accountAddress = GetNewAccountAddress(accountViewModel, newAccountId);
            Guid legalAddressId = accountAddress.Id;
            accountAddresses.Add(accountAddress);
            context.AccountAddresses.Add(accountAddress);

            // Привязка существующего сотрудника организации как основного менеджера по клиенту и добавление его в список "accountManagers"
            List<AccountManager> accountManagers = new List<AccountManager>();
            AccountManager accountManager = GetNewAccountManager(accountViewModel, newAccountId);
            Guid primaryManagerId = accountManager.Id;
            accountManagers.Add(accountManager);
            context.AccountManagers.Add(accountManager);

            return new Account()
            {
                Id = newAccountId,
                Name = name,
                INN = accountViewModel.INN,
                KPP = kpp,
                OKPO = okpo,
                OGRN = ogrn,
                AccountStatus = AccountStatus.Active,
                AccountType = accountType,
                OrganizationId = currentUser.PrimaryOrganizationId,
                ParentAccountId = Guid.Empty,
                PrimaryContactId = primaryContactId,
                LegalAddressId = legalAddressId,
                PrimaryManagerId = primaryManagerId,
                AccountContacts = accountContacts,
                AccountAddresses = accountAddresses,
                AccountManagers = accountManagers
            };
        }

        public override Account OnModelUpdate(AccountViewModel accountViewModel)
        {
            // Клиент, которого необходимо обновить
            Account account = context.Accounts.FirstOrDefault(i => i.Id == accountViewModel.Id);

            // В зависимости от типа клиента менять разные поля
            switch (account.AccountType)
            {
                case AccountType.Individual:
                    account.INN = accountViewModel.INN;
                    break;

                case AccountType.IndividualEntrepreneur:
                    account.Name = accountViewModel.Name;
                    account.INN = accountViewModel.INN;
                    break;

                case AccountType.LegalEntity:
                    account.Name = accountViewModel.Name;
                    account.INN = accountViewModel.INN;
                    account.KPP = accountViewModel.KPP;
                    account.OKPO = accountViewModel.OKPO;
                    account.OGRN = accountViewModel.OGRN;
                    break;
            }

            return account;
        }

        /// <summary>
        /// Метод инициализирует поля модели списка клиентов
        /// </summary>
        /// <param name="accountsViewModel"></param>
        public void InitializeAccountsViewModel(AccountsViewModel accountsViewModel, HttpContext httpContext)
        {
            // Проставление названия организации
            User user = httpContext.GetCurrentUser(context);
            Organization organization = context.Organizations.FirstOrDefault(i => i.Id == user.PrimaryOrganizationId);
            if (organization != null)
                accountsViewModel.PrimaryOrganizationName = organization.Name;

            // Проставление значений в поля фильтров
            AccountsViewModel allAccsViewModel = ModelCash<AccountsViewModel>.GetViewModel(ALL_ACCS);
            AccountsViewModel currentAccsViewModel = ModelCash<AccountsViewModel>.GetViewModel(CURRENT_ACCS);
            accountsViewModel.AllAccountsSearchName = allAccsViewModel.AllAccountsSearchName;
            accountsViewModel.AllAccountsSearchType = allAccsViewModel.AllAccountsSearchType;
            accountsViewModel.CurrentAccountsSearchName = currentAccsViewModel.CurrentAccountsSearchName;
            accountsViewModel.CurrentAccountsSearchType = currentAccsViewModel.CurrentAccountsSearchType;
        }

        public override AccountViewModel DataToViewModel(Account account)
        {
            Organization organization = context.Organizations.FirstOrDefault(i => i.Id == account.OrganizationId);
            AccountAddress legalAddress = account.GetAddresses(context).FirstOrDefault(addr => addr.AddressType == AddressType.Legal);
            return new AccountViewModel()
            {
                Id = account.Id,
                AccountStatus = account.AccountStatus,
                AccountType = account.AccountType.ToString(),
                PrimaryContactId = account.PrimaryContactId == Guid.Empty ? string.Empty : account.PrimaryContactId.ToString(),
                OrganizationId = organization.Id,
                OrganizationName = organization.Name,
                LegalAddress = legalAddress?.GetFullAddress(currentUser),
                Name = account.Name,
                INN = account.INN,
                KPP = account.KPP,
                OKPO = account.OKPO,
                OGRN = account.OGRN,
                Site = account.Site
            };
        }

        public override AccountViewModel UpdateViewModelFromCash(AccountViewModel accountViewModel)
        {
            AccountViewModel accContactsViewModel = ModelCash<AccountViewModel>.GetViewModel(ACC_CONTACTS);
            AccountViewModel accAddressesViewModel = ModelCash<AccountViewModel>.GetViewModel(ACC_ADDRESSES);
            AccountViewModel accInvoicesViewModel = ModelCash<AccountViewModel>.GetViewModel(ACC_INVOICES);
            AccountViewModel accQuotesViewModel = ModelCash<AccountViewModel>.GetViewModel(ACC_QUOTES);
            AccountViewModel accDocumentsViewModel = ModelCash<AccountViewModel>.GetViewModel(ACC_DOCS);
            accountViewModel.SearchContactFullName = accContactsViewModel.SearchContactFullName;
            accountViewModel.SearchContactType = accContactsViewModel.SearchContactType;
            accountViewModel.SearchContactPhoneNumber = accContactsViewModel.SearchContactPhoneNumber;
            accountViewModel.SearchContactEmail = accContactsViewModel.SearchContactEmail;
            accountViewModel.SearchContactPrimary = accContactsViewModel.SearchContactPrimary;
            accountViewModel.SearchAddressCountry = accAddressesViewModel.SearchAddressCountry;
            accountViewModel.SearchAddressRegion = accAddressesViewModel.SearchAddressRegion;
            accountViewModel.SearchAddressCity = accAddressesViewModel.SearchAddressCity;
            accountViewModel.SearchAddressStreet = accAddressesViewModel.SearchAddressStreet;
            accountViewModel.SearchAddressHouse = accAddressesViewModel.SearchAddressHouse;
            accountViewModel.SearchAddressType = accAddressesViewModel.SearchAddressType;
            accountViewModel.SearchInvoiceBankName = accInvoicesViewModel.SearchInvoiceBankName;
            accountViewModel.SearchInvoiceCity = accInvoicesViewModel.SearchInvoiceCity;
            accountViewModel.SearchInvoiceCheckingAccount = accInvoicesViewModel.SearchInvoiceCheckingAccount;
            accountViewModel.SearchInvoiceCorrespondentAccount = accInvoicesViewModel.SearchInvoiceCorrespondentAccount;
            accountViewModel.SearchInvoiceBIC = accInvoicesViewModel.SearchInvoiceBIC;
            accountViewModel.SearchInvoiceSWIFT = accInvoicesViewModel.SearchInvoiceSWIFT;
            return accountViewModel;
        }

        /// <summary>
        /// Метод создает новый контакт при создании клиента с типом "Физическое лицо"
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <param name="newAccountId">Id нового созданного клиента</param>
        /// <returns></returns>
        private AccountContact GetNewAccountContact(AccountViewModel accountViewModel, Guid newAccountId)
        {
            return new AccountContact()
            {
                Id = Guid.NewGuid(),
                AccountId = newAccountId,
                ContactType = ContactType.Work,
                FirstName = accountViewModel.FirstName,
                LastName = accountViewModel.LastName,
                MiddleName = accountViewModel.MiddleName
            };
        }

        /// <summary>
        /// Метод создает новый адрес при создании клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <param name="newAccountId">Id нового созданного клиента</param>
        /// <returns></returns>
        private AccountAddress GetNewAccountAddress(AccountViewModel accountViewModel, Guid newAccountId)
        {
            return new AccountAddress()
            {
                Id = Guid.NewGuid(),
                AccountId = newAccountId,
                AddressType = AddressType.Legal,
                Country = accountViewModel.Country
            };
        }

        /// <summary>
        /// Метод создает нового менеджера у клиента
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <param name="newAccountId">Id нового созданного клиента</param>
        /// <returns></returns>
        private AccountManager GetNewAccountManager(AccountViewModel accountViewModel, Guid newAccountId)
        {
            Func<Employee, bool> predicate = n => n.GetIntialsFullName().ToLower() == accountViewModel.PrimaryManagerInitialName.ToLower().TrimStartAndEnd();
            Employee employee = context.GetOrgEmployees(currentUser.PrimaryOrganizationId).FirstOrDefault(predicate);
            return new AccountManager()
            {
                Id = Guid.NewGuid(),
                AccountId = newAccountId,
                Manager = employee,
                ManagerId = employee.Id
            };
        }
    }
}
