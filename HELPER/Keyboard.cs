using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace HELPER
{
    public class Keyboard
    {
        //public static void HideKeyboardOnCreate(Android.App.Activity activity)
        //{
        //    activity.Window.SetSoftInputMode(SoftInput.StateHidden); // getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
        //}

        public static void HideKeyboard(Activity activity, bool onCreate = false)
        {
            if (!onCreate)
            {
                // Check if no view has focus:
                var view = activity.CurrentFocus; // GetCurrentFocus();
                if (view != null)
                {
                    var imm = (InputMethodManager) activity.GetSystemService(
                        Context.InputMethodService /*INPUT_METHOD_SERVICE*/);
                    imm.HideSoftInputFromWindow(view.WindowToken /*GetWindowToken()*/, 0);
                }
            }
            else
            {
                activity.Window.SetSoftInputMode(SoftInput
                    .StateHidden); // getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
            }
        }

        //public static void HideKeyboard(Activity activity)
        //{
        //    //Find the currently focused view, so we can grab the correct window token from it.
        //    View view = activity.CurrentFocus;

        //    //If no view currently has focus, create a new one, just so we can grab a window token from it
        //    if (view == null)
        //    {
        //        view = new View(activity);
        //    }

        //    InputMethodManager imm = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
        //    imm.HideSoftInputFromWindow(view.WindowToken, 0);
        //}

        public static void HideKeyboardFrom(Context context, View view)
        {
            var imm = (InputMethodManager) context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}