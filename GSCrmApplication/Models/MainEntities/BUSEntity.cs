using System;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmApplication.Models.MainEntities
{
    public class BUSEntity : IBUSEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
