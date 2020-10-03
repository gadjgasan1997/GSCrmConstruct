﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GSCrm.Models
{
    public class Position : BaseDataModel
    {
        public string Name { get; set; }
        public Guid? ParentPositionId { get; set; }
        public Guid? PrimaryEmployeeId { get; set; }

        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        public Division Division { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }

        public Position()
        {
            EmployeePositions = new List<EmployeePosition>();
        }
    }

    public class PositionEqualityComparer : IEqualityComparer<Position>
    {
        public bool Equals([AllowNull] Position x, [AllowNull] Position y) => x.Name == y.Name;

        public int GetHashCode([DisallowNull] Position obj) => obj.Name.GetHashCode();
    }
}
