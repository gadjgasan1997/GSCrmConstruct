using GSCrmLibrary.Models.MainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GSCrmLibrary.Models.TableModels
{
    public class DirectoriesList : DataEntity
    {
        [NotMapped]
        new public string Name { get; set; }
        public string Language { get; set; }
        public string LIC { get; set; }
        public string DisplayValue { get; set; }
        public string DirectoryType { get; set; }
    }
}
