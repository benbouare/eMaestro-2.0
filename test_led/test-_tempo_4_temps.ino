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

CRGB leds[NUM_LEDS];// This is an array of leds.  One item for each led in your strip.

// int bank_1_size = 10;  //Number of LEDs in bank 1
// int bank_2_size = 10;  //Number of LEDs in bank 2
// int c; //bank1 display
// int d; //bank2 display
// int offset = 25; //Number of LEDs between banks
// int bank2; //bank 2 start location


void setup() {
  // sanity check delay - allows reprogramming if accidently blowing power w/leds
    delay(2000);

      FastLED.addLeds<WS2811, DATA_PIN, RGB>(leds, NUM_LEDS);

}

// Main endless Loop
void loop() {
   
   /*for(int i = 0 ; i < NUM_LEDS ; i++) {

      leds[i] = CRGB::Red; // led i

      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   } ;*/


{ int  r = 0, g = 0, b = 255; 

for(int i = 0 ; i < 13 ; i++) {

      leds[i] = CRGB (r,g,b); // led i
     
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   ;
  
 };

delay(10000);
};   

{ int  r = 225, g = 0, b = 0; 

for(int i = 13 ; i < 25 ; i++) {

      leds[i] = CRGB (r,g,b); // led i
     
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   ;
   
 };

delay(10000);
};   

{ int  r = 0, g = 255, b = 0; 

for(int i = 25 ; i < 37 ; i++) {

      leds[i] = CRGB (r,g,b); // led i
     
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   ;
   
 };

delay(10000);
};   

{ int  r = 255, g = 255, b = 255; 

for(int i = 37 ; i < NUM_LEDS ; i++) {

      leds[i] = CRGB (r,g,b); // led i
     
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   ;
   
 };

delay(10000);
};
};   
  /* {int r = 255, g = 0, b = 0;
    for(int i = 0 ; i < NUM_LEDS ; i++) {
   

      leds[i] = CRGB(255,100,0); // led i
 b = (b+50)%255;
 g = (g+50)%255;
 r = (r+50)%255;
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
   }};
   delay(10000);
   }*/
   
