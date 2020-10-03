using System.Collections.Generic;

namespace GSCrm.Models.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        public string CurrentAccountsSearchName { get; set; }
        public string CurrentAccountsSearchType { get; set; }
        public string AllAccountsSearchName { get; set; }
        public string AllAccountsSearchType { get; set; }
        public string PrimaryOrganizationName { get; set; }
        public IEnumerable<AccountViewModel> CurrentAccounts { get; set; }
        public IEnumerable<AccountViewModel> AllAccounts { get; set; }
    }
}
