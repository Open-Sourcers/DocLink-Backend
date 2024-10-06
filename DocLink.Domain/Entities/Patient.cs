﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class Patient:BaseEntity<string>
    { 
        public AppUser user { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public string EmergencyContact { get; set; }

        public ICollection<Appointment> Appointments { get; set; }=new HashSet<Appointment>();
    }
}
