using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    [Serializable]
    public struct VisaRecord
    {
        private string country;
        private DateTime startDate;
        private DateTime endDate;

        public VisaRecord(string ctr, DateTime sDate, DateTime eDate)
        {
            if (ctr == null) throw new ArgumentException();
            country = ctr;
            startDate = sDate;
            endDate = eDate;
        }

        public string Country => country;
        public DateTime StartDate => startDate;
        public DateTime EndDate => endDate;
    }
}
