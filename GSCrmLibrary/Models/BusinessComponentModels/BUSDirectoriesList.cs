using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSDirectoriesList : BUSEntity
    {
        public string Language { get; set; }
        public string LIC { get; set; }
        public string DisplayValue { get; set; }
        public string DirectoryType { get; set; }
    }
}
