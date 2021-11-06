using System;
using HELPER;

namespace MODEL
{
    public class Vaccine : BaseEntity
    {
        private DateTime date;

        public Vaccine()
        {
        }

        public Vaccine(string userNo, DateTime date)
        {
            this.UserNo = userNo;
            this.date = date;
        }

        public string UserNo { get; set; }

        public DateTime Date
        {
            get => date;
            set => date = value;
        }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(UserNo)
                   && DateTimeUtil.IsValidDate(date.ToLongDateString())
                   && ValidateEntry.CheckID(UserNo, true) == ErrorStatus.NONE
                   && (DateTime.Today - date).TotalDays >= 0;
        }
    }
}