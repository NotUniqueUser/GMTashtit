using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HELPER;

namespace MODEL
{
    [Serializable]
    public enum UserType { USER, VACCINE, MANAGER };

    [Serializable]
    public class User : BaseEntity
    {
        private string family;
        private string name;
        private string tz;
        private string image;
        private DateTime birthDate;
        private string email;
        private string phone;
        private string password;
        private string clinicNo;
        private UserType type;

        public User() { }

        public User(string family, string name, string tz, string image, DateTime birthDate, string email, string phone, string password, string clinicNo, UserType type)
        {
            this.family = family;
            this.name = name;
            this.tz = tz;
            this.image = image;
            this.birthDate = birthDate;
            this.email = email;
            this.phone = phone;
            this.password = password;
            this.clinicNo = clinicNo;
            this.type = type;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(family) &&
                   !string.IsNullOrEmpty(name) &&

                   ValidateEntry.CheckID(tz, true) == ErrorStatus.NONE &&
                   ValidateEntry.CheckEMail(email, true) == ErrorStatus.NONE &&
                   ValidateEntry.CheckPhone(phone, true) == ErrorStatus.NONE &&
                   DateTimeUtil.IsValidDate(birthDate.ToShortDateString()) &&
                   Convert.ToInt32(DateTimeUtil.Age(birthDate).Substring(0, 2)) >= 12 &&
                   !string.IsNullOrEmpty(clinicNo) &&
                   !string.IsNullOrEmpty(password) && password.Length >= 5;
        }

        public string FullName
        {
            get { return family + " " + name; }
            set { return; }
        }

        public string Family { get => family; set => family = value; }
        public string Name { get => name; set => name = value; }
        public string Tz { get => tz; set => tz = value; }
        public string Image { get => image; set => image = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Password { get => password; set => password = value; }
        public string ClinicNo { get => clinicNo; set => clinicNo = value; }
        public UserType Type { get => type; set => type = value; }

        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   base.Equals(obj) &&
                   family == user.family &&
                   name == user.name &&
                   tz == user.tz &&
                   image == user.image &&
                   birthDate == user.birthDate &&
                   email == user.email &&
                   phone == user.phone &&
                   clinicNo == user.clinicNo &&
                   password == user.password &&
                   type == user.type;

        }

        public static bool operator ==(User left, User right)
        {
            return EqualityComparer<User>.Default.Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !(left == right);
        }
    }
}

