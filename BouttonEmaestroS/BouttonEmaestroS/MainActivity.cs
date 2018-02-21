using Android.App;
using Android.OS;
using Android.Widget;
using Android.Systems;
using Android.Content;
using Android.Views;
using Android.Graphics;

namespace BouttonEmaestroS
{
  [Activity(Label = "Sasi dees différentes mesures", MainLauncher = true)]
  public class MainActivity : Activity
  {
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      CreateLayoutDynamically(100);
 }
    private void CreateLayoutDynamically(int n)
    {
      int tmp = n + 1;
       ScrollView sv = new ScrollView(this);
      LinearLayout layout = new LinearLayout(this);
      int larglayout = layout.Width;
      int hautlayou = layout.Height;
      //layout.orientation = Orientation.Vertical;
      sv.AddView(layout);
      for (int i = 1; i <= tmp; i++)
      {

        Button myButton = new Button(this);

          myButton.Id = i;

        if (i < tmp)
        {
          myButton.Text = " "+ myButton.Id;
        }
        else
        {
          myButton.Text = "Envoyer" ;
          myButton.SetBackgroundColor(Color.White);
          myButton.SetTextColor(Color.Black);


        }
     
        layout.AddView(myButton);
        
    }
            this.SetContentView(sv);

    }
  }

  }



