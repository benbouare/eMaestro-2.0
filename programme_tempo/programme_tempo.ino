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
int tabRien[25];
int tabBinaire[13];
int tabTernaire[9];
int decalageRien[25];
int decalageBinaire[13];
int decalageTernaire[9];


int nbLed; // nombre de leds à allumer
int mode; // 1-rien, 2-binaire, 3-ternaire
int decalage; // espacement entre les led allumées pour mieux répartir les leds alumées sur les 24

void handleNoteOn(byte inChannel, byte inNumber, byte inVelocity)
{
  //premier note on pour connaitre le mode(binaire, ternaire ou neutre) et le nombre de leds à allumer
  //2e note on pour allumer les leds
    Serial.println("Note On");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");

    int temps = inVelocity;
    switch(temps){
      case 25: //mode rien
        nbLed = tabRien[inNumber];
        mode = 1;
        decalage = decalageRien[inNumber];
        break;

      case 26: //mode binaire
        nbLed = tabBinaire[inNumber];
        mode = 2;
        decalage = decalageBinaire[inNumber];
        break;

      case 27: // mode ternaire
        nbLed = tabTernaire[inNumber];
        mode = 3;
        decalage = decalageTernaire[inNumber];
        break;

      default:
        int  r = 255, g = 255, b = 255;
          for(int i = ((inVelocity-1)*decalage) ; i < (((inVelocity-1)*decalage)+nbLed) ; i++) {
      
              leds[i] = CRGB (r,g,b); // led i
      
              // Show the leds
              FastLED.show();
           ;
          };
        /*leds[inVelocity] = CRGB (0, 0, 255);
        FastLED.show();*/
        break; 
    }
    
}
void handleNoteOff(byte inChannel, byte inNumber, byte inVelocity)
{
  //pareil que pour les messages noteOn
    Serial.println("Note Off");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");
    int  r = 0, g = 0, b = 0;
      for(int i = ((inVelocity-1)*decalage+(nbLed/mode)*(inNumber-1)) ; i < (((inVelocity-1)*decalage)+(nbLed/mode)*inNumber) ; i++) {
      
              leds[i] = CRGB (r,g,b); // led i
      
              // Show the leds
              FastLED.show();
           ;
      };
  /*leds[inVelocity] = CRGB (0, 0, 255);
  FastLED.show();*/
  
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

  tabRien[1] = 24;
  tabRien[2] = 12;
  tabRien[3] = 8;
  tabRien[4] = 6;
  tabRien[5] = 4;
  tabRien[6] = 4;
  tabRien[7] = 3;
  tabRien[8] = 3;
  tabRien[9] = 2;
  tabRien[10] = 2;
  tabRien[11] = 2;
  tabRien[12] = 2;
  tabRien[13] = 1;
  tabRien[14] = 1;
  tabRien[15] = 1;
  tabRien[16] = 1;
  tabRien[17] = 1;
  tabRien[18] = 1;
  tabRien[19] = 1;
  tabRien[20] = 1;
  tabRien[21] = 1;
  tabRien[22] = 1;
  tabRien[23] = 1;
  tabRien[24] = 1;
  
  tabBinaire[1] = 24;
  tabBinaire[2] = 12;
  tabBinaire[3] = 8;
  tabBinaire[4] = 6;
  tabBinaire[5] = 4;
  tabBinaire[6] = 4;
  tabBinaire[7] = 2;
  tabBinaire[8] = 2;
  tabBinaire[9] = 2;
  tabBinaire[10] = 2;
  tabBinaire[11] = 2;
  tabBinaire[12] = 2;
  
  tabTernaire[1] = 24;
  tabTernaire[2] = 12;
  tabTernaire[3] = 6;
  tabTernaire[4] = 6;
  tabTernaire[5] = 3;
  tabTernaire[6] = 3;
  tabTernaire[7] = 3;
  tabTernaire[8] = 3;

  decalageTernaire[1] = 24;
  decalageTernaire[2] = 12;
  decalageTernaire[3] = 8;
  decalageTernaire[4] = 6;
  decalageTernaire[5] = 4;
  decalageTernaire[6] = 4;
  decalageTernaire[7] = 3;
  decalageTernaire[8] = 3;

  for(int j=1; j<25; j++){
    decalageRien[j] = tabRien[j];
  }

  for(int k=1; k<13; k++){
    decalageBinaire[k] = tabBinaire[k];
    if((k == 7) || (k == 8)){
      decalageBinaire[k] = 3;
    }
  }

}

void loop() {
  MIDI.read();
}
