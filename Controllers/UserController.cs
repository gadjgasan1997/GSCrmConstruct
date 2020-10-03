using GSCrm.Data;
using GSCrm.DataTransformers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static GSCrm.CommonConsts;

namespace GSCrm.Controllers
{
    [Authorize]
    [Route(USER)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ResManager resManager;
        public UserController(ApplicationDbContext context, ResManager resManager)
        {
            this.context = context;
            this.resManager = resManager;
        }

        [HttpGet("{id}")]
        public new ViewResult User(string id)
        {
            if (string.IsNullOrEmpty(id)) return View("Error");
            User user = context.Users.FirstOrDefault(i => i.Id == id);
            if (user == null) return View("Error");
            UserTransformer userTransformer = new UserTransformer(context, resManager);
            UserViewModel userViewModel = userTransformer.DataToViewModel(user);
            return View(USER, userViewModel);
        }
    }
}
