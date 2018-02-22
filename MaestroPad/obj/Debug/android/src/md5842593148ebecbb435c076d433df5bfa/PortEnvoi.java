package md5842593148ebecbb435c076d433df5bfa;


public class PortEnvoi
	extends android.media.midi.MidiManager.DeviceCallback
	implements
		mono.android.IGCUserPeer,
		android.media.midi.MidiManager.OnDeviceOpenedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDeviceOpened:(Landroid/media/midi/MidiDevice;)V:GetOnDeviceOpened_Landroid_media_midi_MidiDevice_Handler:Android.Media.Midi.MidiManager/IOnDeviceOpenedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MaestroPad.PortEnvoi, MaestroPad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PortEnvoi.class, __md_methods);
	}


	public PortEnvoi ()
	{
		super ();
		if (getClass () == PortEnvoi.class)
			mono.android.TypeManager.Activate ("MaestroPad.PortEnvoi, MaestroPad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public PortEnvoi (android.media.midi.MidiManager p0, android.app.Activity p1)
	{
		super ();
		if (getClass () == PortEnvoi.class)
			mono.android.TypeManager.Activate ("MaestroPad.PortEnvoi, MaestroPad, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Media.Midi.MidiManager, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onDeviceOpened (android.media.midi.MidiDevice p0)
	{
		n_onDeviceOpened (p0);
	}

	private native void n_onDeviceOpened (android.media.midi.MidiDevice p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
