using System;

namespace GSCrmLibrary.Models.MainEntities
{
    public class BUSEntity : IBUSEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public bool Inactive { get; set; } = false;
        public bool Changed { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
