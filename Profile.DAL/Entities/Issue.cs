﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.DAL.Entities
{
    public class Issue
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public int OriginalEstimate { get; set; }
        public int TimeSpent { get; set; }
        public string IssueType { get; set; }
        public string IconUrl { get; set; }
        public string Status { get; set; }
        public string ColorName { get; set; }

        public int? TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}
