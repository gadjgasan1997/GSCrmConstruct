﻿using System;
using System.Collections.Generic;

namespace GSCrm.Models.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Site { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string OKPO { get; set; }
        public string OGRN { get; set; }
        public string Country { get; set; }
        public string LegalAddress { get; set; }
        public string NewPrimaryManagerId { get; set; } // Id сотрудника, который выбирается в качестве нового основного менеджера
        public string PrimaryManagerInitialName { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountType { get; set; }
        public string PrimaryContactId { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string SearchAllManagersName { get; set; }
        public string SearchAllManagersDivision { get; set; }
        public string SearchAllManagersPosition { get; set; }
        public string SearchSelectedManagersName { get; set; }
        public string SearchSelectedManagersPosition { get; set; }
        public string SearchSelectedManagersPhone { get; set; }
        public string SearchContactFullName { get; set; }
        public string SearchContactType { get; set; }
        public string SearchContactEmail { get; set; }
        public string SearchContactPhoneNumber { get; set; }
        public bool SearchContactPrimary { get; set; }
        public string SearchAddressCountry { get; set; }
        public string SearchAddressRegion { get; set; }
        public string SearchAddressCity { get; set; }
        public string SearchAddressStreet { get; set; }
        public string SearchAddressHouse { get; set; }
        public string SearchAddressType { get; set; }
        public string SearchInvoiceBankName { get; set; }
        public string SearchInvoiceCity { get; set; }
        public string SearchInvoiceCheckingAccount { get; set; }
        public string SearchInvoiceCorrespondentAccount { get; set; }
        public string SearchInvoiceBIC { get; set; }
        public string SearchInvoiceSWIFT { get; set; }
        public List<AccountContactViewModel> AccountContacts { get; set; }
        public List<AccountAddressViewModel> AccountAddresses { get; set; }
        public List<AccountAddressViewModel> AllAccountAddresses { get; set; }
        public List<AccountInvoiceViewModel> AccountInvoices { get; set; }
        public List<AccountQuoteViewModel> AccountQuotes { get; set; }
        public List<AccountManagerViewModel> AccountManagers { get; set; }
        public List<Employee> AllAccountOwnerOrgEmployees { get; set; }
    }
}
