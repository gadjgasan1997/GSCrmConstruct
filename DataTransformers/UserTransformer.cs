using GSCrm.Data;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;

namespace GSCrm.DataTransformers
{
    public class UserTransformer
    {
        private readonly ApplicationDbContext context;
        private readonly ResManager resManager;
        public UserTransformer(ApplicationDbContext context, ResManager resManager)
        {
            this.context = context;
            this.resManager = resManager;
        }

        public UserViewModel DataToViewModel(User user)
        {
            return new UserViewModel()
            {
                AvatarPath = user.AvatarPath
            };
        }
    }
}
