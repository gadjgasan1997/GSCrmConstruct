using System;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmApplication.Models.MainEntities
{
    public class DataEntity : IDataEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
