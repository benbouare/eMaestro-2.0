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
  
      leds[i] = CRGB::Red ; // led i
     
      // Show the leds 
      FastLED.show();
      FastLED.setBrightness(MAX_BRIGHTNESS);
      // Wait a little bit
      delay(40);
  };


for(int i = 5 ; i < 11 ; i++) {

      leds[i] = CRGB::OrangeRed; // led i
      FastLED.setBrightness(220);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);

};


for(int i = 11 ; i < 17 ; i++) {

      leds[i] = CRGB::Yellow; // led i
      FastLED.setBrightness(180);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 17 ; i < 23 ; i++) {

      leds[i] = CRGB::GreenYellow; // led i
      FastLED.setBrightness(126);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 23 ; i < 29 ; i++) {

      leds[i] = CRGB::Green; // led i green
      FastLED.setBrightness(80);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 29 ; i < 35 ; i++) {

      leds[i] = CRGB::Blue; // led i blue
      FastLED.setBrightness(48);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 35 ; i < 41 ; i++) {

      leds[i] = CRGB::Cyan; // led i
      FastLED.setBrightness(32);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};

for(int i = 41 ; i < NUM_LEDS ; i++) {

      leds[i] = CRGB::Fuchsia; // led i
      FastLED.setBrightness(MIN_BRIGHTNESS);
      // Show the leds 
      FastLED.show();

      // Wait a little bit
      delay(40);
};
delay(1000);
};
  

/*{ int  r = 255, g = 255, b = 255; 

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
   
