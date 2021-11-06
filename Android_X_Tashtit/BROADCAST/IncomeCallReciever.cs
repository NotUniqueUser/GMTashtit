using Android.App;
using Android.Content;
using Android.Telephony;

namespace Android_X_Tashtit.BROADCAST
{
    [BroadcastReceiver]
    [IntentFilter(new[] {TelephonyManager.ActionPhoneStateChanged})]
    public class IncomeCallReciever : BroadcastReceiver
    {
        private static bool messageSent;

        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();

            // בדיקת אם יש מידע ב-intent
            if (intent.Extras != null)
            {
                // קריאת מצב השיחה
                var state = intent.GetStringExtra(TelephonyManager.ExtraState);

                if (state == TelephonyManager.ExtraStateOffhook)
                    messageSent = true;

                // בדיקת מצב השיחה
                if (state == TelephonyManager.ExtraStateIdle)
                {
                    // קריאת מס' הטלפון המתקשר
                    var telephone = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);

                    if (string.IsNullOrEmpty(telephone))
                        telephone = string.Empty;

                    //Toast.MakeText(context, "Calling number: " + telephone, ToastLength.Short).Show();

                    if (!string.IsNullOrEmpty(telephone) && !messageSent)
                    {
                        SmsManager.Default.SendTextMessage(telephone, null,
                            "Sorry, I'm bussy now.\nI'll call you later.", null, null);
                        messageSent = true;
                    }
                }
            }
        }
    }
}