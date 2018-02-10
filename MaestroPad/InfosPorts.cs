using Android.Media.Midi;

namespace MaestroPad
{
    class InfosPorts
    {
        private MidiDeviceInfo info;
        private int index;

        public InfosPorts(MidiDeviceInfo info, int i)
        {
            this.info = info;
            this.index = i;
        }

        public MidiDeviceInfo getDeviceInfo()
        {
            return info;
        }

        public int getPortIndex()
        {
            return index;
        }
    }
}
