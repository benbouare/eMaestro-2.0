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

#include <bitswap.h>
#include <chipsets.h>
#include <color.h>
#include <colorpalettes.h>
#include <colorutils.h>
#include <controller.h>
#include <cpp_compat.h>
#include <dmx.h>
#include <FastLED.h>
#include <fastled_config.h>
#include <fastled_delay.h>
#include <fastled_progmem.h>
#include <fastpin.h>
#include <fastspi.h>
#include <fastspi_bitbang.h>
#include <fastspi_dma.h>
#include <fastspi_nop.h>
#include <fastspi_ref.h>
#include <fastspi_types.h>
#include <hsv2rgb.h>
#include <led_sysdefs.h>
#include <lib8tion.h>
#include <noise.h>
#include <pixelset.h>
#include <pixeltypes.h>
#include <platforms.h>
#include <power_mgt.h>

#include <Boards.h>
#include <Firmata.h>
#include <FirmataConstants.h>
#include <FirmataDefines.h>
#include <FirmataMarshaller.h>
#include <FirmataParser.h>

#include "FastLED.h"
#define NUM_LEDS 49
// Data pin that led data will be written out over
#define DATA_PIN 3

CRGB leds[NUM_LEDS];
// --
bool test = false;

void handleNoteOn(byte inChannel, byte inNumber, byte inVelocity)
{
  if(test == false){
    Serial.println("Note On");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");
    { int  r = 255, g = 255, b = 255;
  
    for(int i = ((inVelocity-1)*6) ; i < (inVelocity*6) ; i++) {
  
          leds[i] = CRGB (r,g,b); // led i
  
          // Show the leds
          FastLED.show();
       ;
      };
    };
    /*leds[inVelocity] = CRGB (0, 0, 255);
    FastLED.show();*/
    test = true;
  }
  
}
void handleNoteOff(byte inChannel, byte inNumber, byte inVelocity)
{
  if(test == true){
    Serial.println("Note Off");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");
    { int  r = 0, g = 0, b = 0;
  
    for(int i = ((inVelocity-1)*6) ; i < (inVelocity*6) ; i++) {
  
          leds[i] = CRGB (r,g,b); // led i
  
          // Show the leds
          FastLED.show();
       ;
      };
    };
  /*leds[inVelocity] = CRGB (0, 0, 255);
  FastLED.show();*/
  test = false;
  }
  
}

void setup() {
  Serial.begin(115200);
  FastLED.addLeds<WS2811, DATA_PIN, RGB>(leds, NUM_LEDS);
  //while (!Serial);
  //pinMode(13, OUTPUT);
  MIDI.setHandleNoteOn(handleNoteOn);
  MIDI.setHandleNoteOff(handleNoteOff);
  MIDI.begin(MIDI_CHANNEL_OMNI);
  Serial.println("Arduino ready !");
}

void loop() {
  MIDI.read();
}
