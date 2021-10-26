using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HELPER;

namespace MODEL
{
    public class Vaccine : BaseEntity
    {
        private string userNo;
        private DateTime date;

        public Vaccine()
        {
        }

        public Vaccine(string userNo, DateTime date)
        {
            this.userNo = userNo;
            this.date = date;
        }

        public string UserNo { get => userNo; set => userNo = value; }
        public DateTime Date { get => date; set => date = value; }

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(userNo)
                && DateTimeUtil.IsValidDate(date.ToLongDateString())
                && ValidateEntry.CheckID(userNo, true) == ErrorStatus.NONE
                && (DateTime.Today - date).TotalDays >= 0;

        }
    }
}