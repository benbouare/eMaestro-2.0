#include "FastLED.h"
#define NUM_LEDS 49
// Data pin that led data will be written out over
#define DATA_PIN 3
#define MAX_BRIGHTNESS 255     
#define MIN_BRIGHTNESS 16

const int brightnessInPin = A0;
CRGB leds[NUM_LEDS];// This is an array of leds.  One item for each led in your strip.


void setup() {
  // sanity check delay - allows reprogramming if accidently blowing power w/leds
    delay(2000);

      FastLED.addLeds<WS2811, DATA_PIN, RGB>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);
 

}

void loop() {
   
   /*for(int i = 0 ; i < NUM_LEDS ; i++) {
      leds[i] = CRGB::Red; // led i
      // Show the leds 
      FastLED.show();
      // Wait a little bit
      delay(40);
   } ;*/


for(int i = 0 ; i < 5 ; i++) {
  
      leds[i] = CRGB(255,0,0) ; // led i
     
      // Show the leds 
      FastLED.show();
      FastLED.setBrightness(MAX_BRIGHTNESS);
      // Wait a little bit
      delay(40);
  };


for(int i = 5 ; i < 11 ; i++) {

      leds[i] = CRGB(255,69,0); // led i
      FastLED.setBrightness(220);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);

};


for(int i = 11 ; i < 17 ; i++) {

      leds[i] = CRGB(255,255,0); // led i
      FastLED.setBrightness(180);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 17 ; i < 23 ; i++) {

      leds[i] = CRGB(173,255,47); // led i
      FastLED.setBrightness(126);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 23 ; i < 29 ; i++) {

      leds[i] = CRGB(0,128,0); // led i green
      FastLED.setBrightness(80);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 29 ; i < 35 ; i++) {

      leds[i] = CRGB(0,0,255); // led i blue
      FastLED.setBrightness(48);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 35 ; i < 41 ; i++) {

      leds[i] = CRGB(0,255,255); // led i
      FastLED.setBrightness(32);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 41 ; i < NUM_LEDS ; i++) {

      leds[i] = CRGB(255,0,255); // led i
      FastLED.setBrightness(MIN_BRIGHTNESS);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};
delay(1000);
};
