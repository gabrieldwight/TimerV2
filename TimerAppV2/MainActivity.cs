using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Timers;

namespace TimerAppV2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ProgressBar round_progress_bar;
        Button start_button, stop_button;
        TextView Timer_txt;
        Timer timer;

        int hour = 0, minute = 0, second = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            round_progress_bar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            start_button = FindViewById<Button>(Resource.Id.Startbtn);
            stop_button = FindViewById<Button>(Resource.Id.Stopbtn);
            Timer_txt = FindViewById<TextView>(Resource.Id.TImertxt);

            start_button.Click += Start_button_Click;

            stop_button.Click += Stop_button_Click;
        }

        private void Start_button_Click(object sender, System.EventArgs e)
        {
            RunOnUiThread(() =>
            {
                start_button.Enabled = false;
                stop_button.Enabled = true;
            });
            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            });
        }

        private void Stop_button_Click(object sender, System.EventArgs e)
        {
            timer.Dispose();
            timer = null;
            RunOnUiThread(() =>
            {
                start_button.Enabled = true;
                stop_button.Enabled = false;
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            second++;
            if (second == 60)
            {
                minute++;
                second = 0;
            }

            if (minute == 60)
            {
                hour++;
                minute = 0;
            }

            RunOnUiThread(() =>
            {
                Timer_txt.Text = string.Format("{0}:{1}:{2}", hour, minute, second);
                round_progress_bar.Progress = second;
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}