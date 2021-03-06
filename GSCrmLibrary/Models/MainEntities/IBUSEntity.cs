﻿using System;

namespace GSCrmLibrary.Models.MainEntities
{
    public interface IBUSEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        DateTime LastUpdated { get; set; }
        string UpdatedBy { get; set; }
        string ErrorMessage { get; set; }
    }
}
