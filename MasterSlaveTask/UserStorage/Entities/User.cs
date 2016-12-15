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
    public class User : IEquatable<User>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Id { get; }
        public Gender Sex { get; private set; }
        
        public VisaRecord[] visaCards { get; private set; }

        #region Equals
        public bool Equals(User other)
        {
            if (other == null)
                return false;
            if (Id == other.Id) return true;
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
                                    +visaCards.Length.ToString();            
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Public default constructor for XML serialisation
        /// </summary>
        public User() { }

        public User(string fName, string lName, string id, Gender sex)
        {
            if (fName == null || lName == null || id == null) throw new ArgumentException();
            FirstName = fName;
            LastName = lName;
            Id = id;
            Sex = sex;
            visaCards = new VisaRecord[5]; // need to discuss the number of cards with the manager 
        }
        #endregion

        // need to discuss the method for adding cards with the manager 
        public void AddVisaCard(VisaRecord newVisa)
        {
            if (!visaCards.Contains(newVisa))
            {
                for(int i=0;i<visaCards.Length;i++)
                {
                    if (visaCards[i].EndDate < DateTime.Now)
                        visaCards[i] = newVisa;
                }
            }
        }
    }
    
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
