using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Validators;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static GSCrm.CommonConsts;

namespace GSCrm.Repository
{
    public class QuoteRepository : GenericRepository<Quote, QuoteViewModel, QuoteValidatior, QuoteTransformer>
    {
        public static QuoteViewModel CurrentQuote { get; set; }
        public static Account SelectedAccount { get; set; }
        public static Employee QuoteManager { get; set; }

        private readonly User currentUser;
        public QuoteRepository(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager, HttpContext httpContext = null)
            : base(context, viewsInfo, resManager, new QuoteValidatior(context, resManager, httpContext), new QuoteTransformer(context, resManager, httpContext))
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        #region Ataching Quotes
        public void AttachQuotes(ref QuotesViewModel quotesViewModel)
        {
            quotesViewModel.AllQuotes = context.GetAllQuotes(currentUser).TransformToViewModels
                <Quote, QuoteViewModel, QuoteTransformer>(
                transformer: new QuoteTransformer(context, resManager),
                limitingFunc: GetLimitedAllQuotesList);

            quotesViewModel.CurrentQuotes = context.GetCurrentQuotes(currentUser).TransformToViewModels
                <Quote, QuoteViewModel, QuoteTransformer>(
                transformer: new QuoteTransformer(context, resManager),
                limitingFunc: GetLimitedCurrentQuotesList);
        }

        /// <summary>
        /// Метод ограничивает список всех сделок
        /// </summary>
        /// <param name="quotes"></param>
        /// <returns></returns>
        private List<Quote> GetLimitedAllQuotesList(List<Quote> quotes)
        {
            List<Quote> limitedQuotes = quotes;
            LimitListByPageNumber(ALL_QUOTES, ref limitedQuotes);
            return limitedQuotes;
        }

        /// <summary>
        /// Метод ограничивает список сделок основной организации пользователя
        /// </summary>
        /// <param name="quotes"></param>
        /// <returns></returns>
        private List<Quote> GetLimitedCurrentQuotesList(List<Quote> quotes)
        {
            List<Quote> limitedQuotes = quotes;
            LimitListByPageNumber(CURRENT_QUOTES, ref limitedQuotes);
            return limitedQuotes;
        }
        #endregion
    }
}
