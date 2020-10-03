using GSCrm.Data;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.Repository.QuoteRepository;

namespace GSCrm.Validators
{
    public class QuoteValidatior : BaseValidator<QuoteViewModel>
    {
        private readonly User currentUser;
        public QuoteValidatior(ApplicationDbContext context, ResManager resManager, HttpContext httpContext = null) : base(context, resManager)
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        public override Dictionary<string, string> CreationCheck(QuoteViewModel quoteViewModel) => CommonChecks(quoteViewModel);

        public override Dictionary<string, string> UpdateCheck(QuoteViewModel quoteViewModel)
        {
            CommonChecks(quoteViewModel);
            return errors;
        }

        /// <summary>
        /// Общие проверки для методов "UpdateCheck" or "CreationCheck"
        /// </summary>
        /// <param name="invoiceViewModel"></param>
        private Dictionary<string, string> CommonChecks(QuoteViewModel quoteViewModel)
        {
            CheckAmount(quoteViewModel);
            if (errors.Any()) return errors;

            CheckAccount(quoteViewModel);
            if (errors.Any()) return errors;

            CheckAccountManager(quoteViewModel);
            return errors;
        }

        /// <summary>
        /// Метод проверяет сумму сделки на коректность
        /// </summary>
        /// <param name="quoteViewModel"></param>
        private void CheckAmount(QuoteViewModel quoteViewModel)
        {
            if (string.IsNullOrEmpty(quoteViewModel.Amount))
            {
                errors.Add("AmountLength", resManager.GetString("AmountLength"));
                return;
            }
            if (!decimal.TryParse(quoteViewModel.Amount, out decimal _))
                errors.Add("AmountWrong", resManager.GetString("AmountWrong"));
        }

        /// <summary>
        /// Метод проверяет выбранного клиента, с которым заключается сделка
        /// </summary>
        /// <param name="quoteViewModel"></param>
        private void CheckAccount(QuoteViewModel quoteViewModel)
        {
            if (string.IsNullOrEmpty(quoteViewModel.AccountName))
            {
                errors.Add("QuoteAccontLength", resManager.GetString("QuoteAccontLength"));
                return;
            }

            Account selectedAccount = context.Accounts
                .Include(acc => acc.AccountManagers)
                    .ThenInclude(man => man.Manager)
                .Where(orgId => orgId.OrganizationId == currentUser.PrimaryOrganizationId)
                .FirstOrDefault(n => n.Name == quoteViewModel.AccountName);
            if (selectedAccount == null)
            {
                errors.Add("AccountNotFound", resManager.GetString("AccountNotFound"));
                return;
            }
            SelectedAccount = selectedAccount;
        }

        /// <summary>
        /// Метод проверяет выбранного клиентского менеджера
        /// </summary>
        /// <param name="quoteViewModel"></param>
        private void CheckAccountManager(QuoteViewModel quoteViewModel)
        {
            if (string.IsNullOrEmpty(quoteViewModel.ManagerInitialName))
            {
                errors.Add("QuoteAccountManagerLength", resManager.GetString("QuoteAccountManagerLength"));
                return;
            }

            List<Employee> accountManagers = SelectedAccount.AccountManagers.Select(man => man.Manager).ToList();
            Employee employee = accountManagers.FirstOrDefault(n => n.GetIntialsFullName().ToLower().Contains(quoteViewModel.ManagerInitialName.ToLower().TrimStartAndEnd()));
            if (employee == null)
            {
                errors.Add("AccountManagerNotFound", resManager.GetString("AccountManagerNotFound"));
                return;
            }
            
            if (employee.EmployeeStatus == EmployeeStatus.Lock)
            {
                errors.Add("AccountManagerIsLocked", resManager.GetString("AccountManagerIsLocked"));
                return;
            }
            QuoteManager = employee;
        }
    }
}
