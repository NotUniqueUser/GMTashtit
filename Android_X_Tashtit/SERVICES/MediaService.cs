using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace Android_X_Tashtit.SERVICES
{
    [Service]
    public class MediaService : Service
    {
        // מנגינה מהאינטרנט
        private const string Mp3 = @"http://www.hochmuth.com/mp3/Vivaldi_Sonata_eminor_.mp3";
        private MediaPlayer player;

        public override IBinder OnBind(Intent intent)
        {
            return null; // חובה להחזיר 
        }

        // Serviceהמטודה המבצעת את פעילות ה-
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags,
            int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            Toast.MakeText(this, "Loading", ToastLength.Short).Show();

            // Thread הפעלת 
            Task.Run(() =>
            {
                // טעינת הקובץ
                player = MediaPlayer.Create(this, Resource.Raw.ambient_music /*Android.Net.Uri.Parse(Mp3)*/);

                // הגדרה שהמנגינה תחזור על עצמה
                player.Looping = true;

                // הפעלת הנגן
                player.Start();
            });

            // ראה הסבר בהמשך
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            player.Stop();
        }
    }
}