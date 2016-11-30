using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    [Serializable]
    public class User : IEquatable<User>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Id { get;}
        public Gender Sex { get; private set; }
        public List<VisaRecord> visaCards { get; private set; }

        #region Equals
        public bool Equals(User other)
        {
            if (other == null)
                return false;
            if (this.Id == other.Id) return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            User userObj = obj as User;
            if (userObj == null)
                return false;
            else return Equals(userObj);
        }

        public override int GetHashCode()
        {
            if (Id != null) return Id.GetHashCode();
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString()+" "
                                    + FirstName.ToString() + " " 
                                    +LastName.ToString()+" "
                                    +Sex.ToString()+" "
                                    +visaCards.Count.ToString();            
        }
        #endregion

        #region Constructors
        private User() { }

        public User(string fName, string lName, string Id, Gender sex)
        {
            if (fName == null || lName == null || Id == null) throw new ArgumentException();
            visaCards = new List<VisaRecord>();
        }
        #endregion

        public void ChangeGender(Gender newSex)
        {
            this.Sex = newSex;
        }

        public void AddVisaCard(VisaRecord newVisa)
        {
            if (visaCards.IndexOf(newVisa) == -1) visaCards.Add(newVisa);
        }
    }

    public struct VisaRecord
    {
        private string country;
        private DateTime startDate;
        private DateTime endDate;

        public VisaRecord (string ctr, DateTime sDate, DateTime eDate)
        {
            if (ctr == null || eDate < DateTime.Now) throw new ArgumentException();            
            country = ctr;
            startDate = sDate;
            endDate = eDate;
        }

        public string Country => country;
        public DateTime StartDate => startDate;
        public DateTime EndDate => endDate;
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
