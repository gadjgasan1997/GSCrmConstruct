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
using static GSCrm.Repository.QuoteRepository;

namespace GSCrm.DataTransformers
{
    public class QuoteTransformer : BaseTransformer<Quote, QuoteViewModel>
    {
        private readonly User currentUser;
        public QuoteTransformer(ApplicationDbContext context, ResManager resManager, HttpContext httpContext = null) : base(context, resManager)
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        public override QuoteViewModel DataToViewModel(Quote quote)
        {
            Account account = context.Accounts.FirstOrDefault(i => i.Id == quote.AccountId);
            Employee employee = context.Employees.FirstOrDefault(i => i.Id == quote.ManagerId);
            return new QuoteViewModel()
            {
                Id = quote.Id,
                Number = quote.Number,
                AccountName = account.Name,
                OrganizationId = quote.OrganizationId.ToString(),
                QuoteStatus = quote.QuoteStatus,
                Amount = quote.Amount.ToString(),
                ManagerInitialName = employee.GetIntialsFullName()
            };
        }

        public override Quote OnModelCreate(QuoteViewModel quoteViewModel)
        {
            Guid quoteId = Guid.NewGuid();
            return new Quote()
            {
                Id = quoteId,
                AccountId = SelectedAccount.Id,
                Amount = decimal.Parse(quoteViewModel.Amount),
                QuoteStatus = QuoteStatus.Potential,
                OrganizationId = currentUser.PrimaryOrganizationId,
                ManagerId = QuoteManager.Id,
                Number = quoteViewModel.GetNewNumber(context),
                AccountQuotes = new List<AccountQuote>()
                {
                    new AccountQuote()
                    {
                        AccountId = SelectedAccount.Id,
                        QuoteId = quoteId
                    }
                }
            };
        }

        public override Quote OnModelUpdate(QuoteViewModel viewModel)
        {
            return base.OnModelUpdate(viewModel);
        }

        /// <summary>
        /// Метод инициализирует поля модели списка сделок
        /// </summary>
        /// <param name="quotesViewModel"></param>
        public void InitializeQuotesViewModel(QuotesViewModel quotesViewModel, HttpContext httpContext)
        {
            // Проставление названия организации
            User user = httpContext.GetCurrentUser(context);
            if (user.PrimaryOrganizationId != null)
            {
                Organization organization = context.Organizations.FirstOrDefault(i => i.Id == user.PrimaryOrganizationId);
                if (organization != null)
                    quotesViewModel.PrimaryOrganizationName = organization.Name;
            }

            // Проставление значений в поля фильтров
            QuotesViewModel allAccsViewModel = ModelCash<QuotesViewModel>.GetViewModel(ALL_QUOTES);
            QuotesViewModel currentAccsViewModel = ModelCash<QuotesViewModel>.GetViewModel(CURRENT_QUOTES);
            quotesViewModel.AllQuotesSearchName = allAccsViewModel.AllQuotesSearchName;
            quotesViewModel.CurrentQuotesSearchName = currentAccsViewModel.CurrentQuotesSearchName;
        }
    }
}
