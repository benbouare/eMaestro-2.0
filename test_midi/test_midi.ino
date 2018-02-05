#include <MIDI.h>

#if defined(USBCON)
#include <midi_UsbTransport.h>

static const unsigned sUsbTransportBufferSize = 16;
typedef midi::UsbTransport<sUsbTransportBufferSize> UsbTransport;

UsbTransport sUsbTransport;

MIDI_CREATE_INSTANCE(UsbTransport, sUsbTransport, MIDI);

#else // No USB available, fallback to Serial
MIDI_CREATE_DEFAULT_INSTANCE();
#endif

// --

void handleNoteOn(byte inChannel, byte inNumber, byte inVelocity)
{
    digitalWrite(13, HIGH);
}
void handleNoteOff(byte inChannel, byte inNumber, byte inVelocity)
{
    digitalWrite(13, LOW);
}

void setup() {
    //Serial.begin(115200);
    //while (!Serial);
    pinMode(13, OUTPUT);
    MIDI.setHandleNoteOn(handleNoteOn);
    MIDI.setHandleNoteOff(handleNoteOff);
    MIDI.begin(MIDI_CHANNEL_OMNI);
    //Serial.println("Arduino ready.");
}

void loop() {
    MIDI.read();
}
