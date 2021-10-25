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

namespace MODEL
{
    class Vaccines : BaseList_FS<Vaccine>
    {
        public override bool Exist(Vaccine entity, out Vaccine existEntity)
        {
            existEntity = FindAll(item => item.UserNo.Equals(entity.UserNo) && item.UserNo.Equals(entity.Date))[0];
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((Vaccine v1, Vaccine v2) => v1.Date.Ticks.CompareTo(v2.Date.Ticks));
        }
    }
}