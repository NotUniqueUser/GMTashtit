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
    class Vaccine : BaseEntity
    {
        private string userNo;
        private DateTime date;

        public string UserNo { get => userNo; set => userNo = value; }
        public DateTime Date { get => date; set => date = value; }

        public override bool Validate()
        {
            return ValidateEntry.CheckID(userNo, true) == ErrorStatus.NONE
                && (DateTime.Today - date).TotalDays >= 0;

        }
    }
}