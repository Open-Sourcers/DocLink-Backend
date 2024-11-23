using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
    public class AppointmentsSpec : BaseSpecification<Appointment , int>
    {
        public AppointmentsSpec(string ID , AppointmentStatus status) : base(app => app.PatientId == ID && app.Status == status){
            Includes.Add(app => app.Doctor);
            Includes.Add(app => app.Doctor.user);
            Includes.Add(app => app.Doctor.Specialty);
        }
    }
}
