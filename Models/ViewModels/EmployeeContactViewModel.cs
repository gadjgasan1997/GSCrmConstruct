﻿using System;

namespace GSCrm.Models.ViewModels
{
    public class EmployeeContactViewModel : BaseViewModel
    {
        public Guid EmployeeId { get; set; }
        public string ContactType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
