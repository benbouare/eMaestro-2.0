#include <MIDI.h>

//gestion MIDI USB
#if defined(USBCON)
#include <midi_UsbTransport.h>

static const unsigned sUsbTransportBufferSize = 16;
typedef midi::UsbTransport<sUsbTransportBufferSize> UsbTransport;

UsbTransport sUsbTransport;

MIDI_CREATE_INSTANCE(UsbTransport, sUsbTransport, MIDI);

#else // Pas de connexion USB, retour à la connexion Série
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

#define NUM_LEDS 49 // Nombre de leds de la guirlande
#define DATA_PIN 3 // Data pin de la guirlande de leds
#define MAX_BRIGHTNESS 255 // Luminosité minimale
#define MIN_BRIGHTNESS 16 // Luminosité maximale

CRGB leds[NUM_LEDS]; // Tableau dont chaque élément référence chaque led de la guirlande.

// tableaux de nombres de leds à allumer en fonction du nombre de temps par mesure
int tabRien[25]; // pour le mode unaire
int tabBinaire[13]; // pour le mode binaire
int tabTernaire[9]; // pour le mode ternaire

//tableaux définissant le nombre de leds occupées pour chaque temps de la mesure
int decalageRien[25]; // pour le mode unaire
int decalageBinaire[13]; // pour le mode binaire
int decalageTernaire[9]; // pour le mode ternaire


int nbLed; // nombre de leds effectif à allumer
int mode; // 1-rien, 2-binaire, 3-ternaire.
int decalage; // espacement entre les leds allumées pour mieux répartir les leds alumées sur les 24.
int  r, g, b; // codes couleurs : red, green, blue.
int alerteR, alerteG, alerteB; // codes couleurs pour les alertes
int brightness; // variable pour la luminosité
int armature; // nombre de dièses - nombre de bémols

/**
 * Fonction appelée en cas de réception d'un message NoteOn
 * Le premier message informe du mode(unaire, binaire ou ternaire) et du nombre de temps par mesure.
 * Les autres messages servent à allumer les leds avec la couleur et la luminosité définis par les variables bright et r,g,b.
 * Utilisée pour l'affichage du tempo
 */
void handleNoteOn(byte inChannel, byte inNumber, byte inVelocity)
{
    Serial.println("Note On");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");

    int temps = inVelocity;
    switch(temps){
      //gestion du premier message NoteOn(définition du mode et du nombre de leds en fonction du nombre de temps)
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

      //gestion des autres messages Noteon(allumage des leds)
      default:
          //leds du tempo (leds 0-23)
          for(int i = ((inVelocity-1)*decalage) ; i < (((inVelocity-1)*decalage)+nbLed) ; i++) {
      
              leds[i] = CRGB (r,g,b); // led i
           ;
          };
          
          //leds des alertes (leds 24-39 et 48)
          for(int i = 24; i < 40; i++){
            leds[i] = CRGB (alerteR,alerteG,alerteB); // crimson
          }
          leds[48] = CRGB (alerteR,alerteG,alerteB);

          //gestion des armatures 
          if (armature > 0) // dièse > bémol
            {
                for (int i = 41; i < 48; i++)
                    {
                        leds[i] = CRGB (0,0,0);
                    }
                for (int i = 41; i < 41 + armature; i++)
                {
                    leds[i] = CRGB (r,g,b);
                }
            }
            else
            {
                if (armature < 0) // dièse < bémol
                {
                    for (int i = 41; i < 48; i++)
                    {
                        leds[i] = CRGB (0,0,0);
                    }
                    for (int i = 47; i > 47 + armature; i--)
                    {
                        leds[i] = CRGB (r,g,b);
                    }
                }
                else
                {
                    for (int i = 41; i < 48; i++)
                    {
                        leds[i] = CRGB (0,0,0);
                    }
                }
            }
          
          //allumage des Leds
          FastLED.setBrightness(brightness);
          FastLED.show();
        break; 
    }
    
}

/**
 * Fonction appelée en cas de réception d'un message Noteoff
 * Permet d'éteindre les leds spécifiées
 * inNumber représente le sous-temps, inVelocity représente le temps courant de la mesure.
 */
void handleNoteOff(byte inChannel, byte inNumber, byte inVelocity)
{
  //pareil que pour les messages noteOn
    Serial.println("Note Off");
    Serial.print(inNumber);
    Serial.print(" ");
    Serial.println(inVelocity);
    Serial.println("");

    //extinction des leds
      //r = 0; g = 0; b = 0;
      for(int i = ((inVelocity-1)*decalage+(nbLed/mode)*(inNumber-1)) ; i < (((inVelocity-1)*decalage)+(nbLed/mode)*inNumber) ; i++) {
      
              leds[i] = CRGB (0,0,0); // led i
      
              // Show the leds
              FastLED.show();
           ;
      };
  /*leds[inVelocity] = CRGB (0, 0, 255);
  FastLED.show();*/
  
}

/**
 * Fonction appelée en cas de réception de message Control Change
 * Permet de définir des paramètres globaux utilisés par l'ensemble des autres fonctions de la MaestroBox
 * utilisée pour la gestion des nuances(définies par 8 couleurs et luminosités) avec la variable inNumber, représentant la couleur choisie.
 * utilisée pour la gestion des alertes avec le paramètre inChannel(6 couleurs pour représenter les alertes)
 */
void handleControlChange(byte inChannel, byte inNumber, byte inValue){

  //gestion des alertes (leds 24-39 et 48)
  switch(inChannel){
    case 1:
      alerteR = 0;
      alerteG = 0;
      alerteB = 0;
      break;
      
    case 2:
      alerteR = 220;
      alerteG = 20;
      alerteB = 60;
      break;
      
    case 3:
      alerteR = 85;
      alerteG = 107;
      alerteB = 47;
      break;
      
    case 4:
      alerteR = 255;
      alerteG = 20;
      alerteB = 147;
      break;
      
    case 5:
      alerteR = 255;
      alerteG = 250;
      alerteB = 205;
      break;
      
    case 6:
      alerteR = 0;
      alerteG = 0;
      alerteB = 128;
      break;
      
    case 7:
      alerteR = 178;
      alerteG = 34;
      alerteB = 34;
      break;
      
    default :
      alerteR = 0;
      alerteG = 0;
      alerteB = 0;
      break;
  }
  
  //gestion des nuances
  switch(inNumber){
    case 0:
      r = 255;
      g = 255;
      b = 255;
      break;
    
    case 1:
      r = 255;
      g = 0;
      b = 255;
      brightness = MIN_BRIGHTNESS;
      break;
      
    case 2:
      r = 0;
      g = 255;
      b = 255;
      brightness = 32;
      break;
      
    case 3:
      r = 0;
      g = 0;
      b = 255;
      brightness = 48;
      break;

    case 4:
      r = 0;
      g = 128;
      b = 0;
      brightness = 80;
      break;

    case 5:
      r = 173;
      g = 255;
      b = 47;
      brightness = 126;
      break;

    case 6:
      r = 255;
      g = 255;
      b = 0;
      brightness = 180;
      break;

    case 7:
      r = 255;
      g = 69;
      b = 0;
      brightness = 220;
      break;

    case 8:
      r = 255;
      g = 0;
      b = 0;
      brightness = MAX_BRIGHTNESS;
      break; 
  }

  /*switch(inValue){
    
  }*/
}

void handleAfterTouchPoly(byte inChannel, byte inNumber, byte inValue){
  armature = inValue - inNumber; // nombre de dièses - nombre de bémols
}

/**
 * Fonction appelée au démarrage de l'Arduino
 * Permet d'initialiser la guirlande de leds, de paramétrer les callbacks de gestion des messages MIDI
 * permet d'initialiser les tableaux
 */
void setup() {
  Serial.begin(115200);
  FastLED.addLeds<WS2811, DATA_PIN, RGB>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);
  //while (!Serial);
  //pinMode(13, OUTPUT);
  MIDI.setHandleNoteOn(handleNoteOn);
  MIDI.setHandleNoteOff(handleNoteOff);
  MIDI.setHandleControlChange(handleControlChange);
  MIDI.setHandleAfterTouchPoly(handleAfterTouchPoly);
  MIDI.begin(MIDI_CHANNEL_OMNI);
  Serial.println("Arduino ready !");

  //tabRien[nombreDeTemps/mesure] = nombre de leds à allumer
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

  //tabBinaire[nombreDeTemps/mesure] = nombre de leds à allumer
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

  //tabTernaire[nombreDeTemps/mesure] = nombre de leds à allumer
  tabTernaire[1] = 24;
  tabTernaire[2] = 12;
  tabTernaire[3] = 6;
  tabTernaire[4] = 6;
  tabTernaire[5] = 3;
  tabTernaire[6] = 3;
  tabTernaire[7] = 3;
  tabTernaire[8] = 3;

  //decalageTernaire[nombreDeTemps/mesure] - nombre de leds à allumer = Nombre de leds d'espacement
  decalageTernaire[1] = 24;
  decalageTernaire[2] = 12;
  decalageTernaire[3] = 8;
  decalageTernaire[4] = 6;
  decalageTernaire[5] = 4;
  decalageTernaire[6] = 4;
  decalageTernaire[7] = 3;
  decalageTernaire[8] = 3;

  //decalageRien[nombreDeTemps/mesure] - nombre de leds à allumer = Nombre de leds d'espacement
  for(int j=1; j<25; j++){
    decalageRien[j] = tabRien[j];
  }

  //decalageBinaire[nombreDeTemps/mesure] - nombre de leds à allumer = Nombre de leds d'espacement
  for(int k=1; k<13; k++){
    decalageBinaire[k] = tabBinaire[k];
    if((k == 7) || (k == 8)){
      decalageBinaire[k] = 3;
    }
  }
  
}

/**
 * Fonction appelée constamment par l'arduino durant toute l'exécution
 * permet de recevoir des messages MIDI
 * Permet d'appeler la fonction adéquate en fonction du type de message reçu
 */
void loop() {
  MIDI.read();
}
