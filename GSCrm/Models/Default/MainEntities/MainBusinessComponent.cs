﻿using System;

namespace GSCrm.Models.Default.MainEntities
{
    public class MainBusinessComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public Guid? ParRowId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public int Sequence { get; set; } = 0;
        public bool Inactive { get; set; } = false;
        public bool Display { get; set; } = true;
        public bool Required { get; set; } = false;
        public string Type { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SearchSpecification { get; set; }
        public MainBusinessComponent() { }
    }
}
