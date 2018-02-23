using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Media.Midi;

namespace MaestroPad
{

    [Activity(Label = "MaestroPad", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public MidiManager manager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
          //  manager = (MidiManager)GetSystemService(MidiService);
            Button nouvllpartition = FindViewById<Button>(Resource.Id.NouvellePartition);
            nouvllpartition.Click += (sender, e) =>
             {
                 //new activity 
                 //setup a new intent
                 var intent = new Intent(this, typeof(NouvellePartition));
                 StartActivity(intent);
             };

        }
        public MidiManager getmanager()
        {
            return this.manager;
        }
    }
}

