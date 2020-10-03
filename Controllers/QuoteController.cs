using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Repository;
using GSCrm.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using static GSCrm.CommonConsts;
using static GSCrm.Repository.QuoteRepository;

namespace GSCrm.Controllers
{
    [Authorize]
    [Route(QUOTE)]
    public class QuoteController
        : MainController<Quote, QuoteViewModel, QuoteValidatior, QuoteTransformer, QuoteRepository>
    {
        public QuoteController(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager)
            : base(context, viewsInfo, resManager, new QuoteTransformer(context, resManager), new QuoteRepository(context, viewsInfo, resManager))
        { }

        [HttpGet("ListOfAllQuotes/{pageNumber}")]
        public ViewResult AllQuotes(int pageNumber)
        {
            QuoteRepository quoteRepository = new QuoteRepository(context, viewsInfo, resManager, HttpContext);
            QuotesViewModel quotesViewModel = new QuotesViewModel();
            transformer.InitializeQuotesViewModel(quotesViewModel, HttpContext);
            quoteRepository.SetViewInfo(ALL_QUOTES, pageNumber);
            quoteRepository.AttachQuotes(ref quotesViewModel);
            return View(QUOTES, quotesViewModel);
        }

        [HttpGet("ListOfCurrentQuotes/{pageNumber}")]
        public ViewResult CurrentQuotes(int pageNumber)
        {
            QuoteRepository quoteRepository = new QuoteRepository(context, viewsInfo, resManager, HttpContext);
            QuotesViewModel quotesViewModel = new QuotesViewModel();
            transformer.InitializeQuotesViewModel(quotesViewModel, HttpContext);
            repository.SetViewInfo(CURRENT_QUOTES, pageNumber);
            quoteRepository.AttachQuotes(ref quotesViewModel);
            return View(QUOTES, quotesViewModel);
        }

        [HttpGet("BackToQuotes")]
        public IActionResult BackToQuotes()
        {
            // Возврат назад в зависимости от того, на какой вкладке находился пользователь перед проваливанием в карточку
            return CurrentQuote.SelectedQuotesTab switch
            {
                // Проваливание со списка всех сделок
                ALL_QUOTES => RedirectToAction(ALL_QUOTES, QUOTE, new { pageNumber = viewsInfo.Get(ALL_QUOTES)?.CurrentPageNumber }),

                // Проваливание со списка сделок основной организации текущего пользователя
                _ => RedirectToAction(CURRENT_QUOTES, QUOTE, new { pageNumber = viewsInfo.Get(CURRENT_QUOTES)?.CurrentPageNumber }),
            };
        }

        [HttpPost("Create")]
        public override IActionResult Create(QuoteViewModel quoteViewModel)
        {
            ModelStateDictionary modelState = ModelState;
            QuoteRepository quoteRepository = new QuoteRepository(context, viewsInfo, resManager, HttpContext);
            if (quoteRepository.TryCreate(ref quoteViewModel, modelState))
                return Json("");
            return BadRequest(modelState);
        }
    }
}
