#include "ddj.h"
#include <EEPROM.h>
#include <RotaryEncoder.h>
#include <MIDI.h>
#include <Adafruit_NeoPixel.h>

#define DATA_PIN 2  //DATA PIN OF THE RGB LEDS

#define runEvery(t) for (static uint16_t _lasttime;\
                         (uint16_t)((uint16_t)millis() - _lasttime) >= (t);\
                         _lasttime += (t))

RotaryEncoder encoder(18, 19); //Encoders are connected to pins 18-19 and 2-3 (A and B of each encoder)
RotaryEncoder encoder1(20, 21);
Adafruit_NeoPixel strip = Adafruit_NeoPixel(16, DATA_PIN, NEO_RGB + NEO_KHZ800);

MIDI_CREATE_DEFAULT_INSTANCE();

bool default_init = HIGH;
led_color color[16];
led_color single_color;
led_color test = {24, 255, 0};
unsigned char ledMode = 3;
bool ledState = LOW;
unsigned int interval = 500; //Interval in milliseconds of the Led Blinking mode
int potenciometro[6] = {A0, A1, A2, A3, A4, A5};  //The potentiometers are connected to A0, A1, A2 and A3
int valpot[6] = {0, 0, 0, 0, 0, 0};
int lastval[6] = {0, 0, 0, 0, 0, 0};
int diff[6] = {0, 0, 0, 0, 0, 0};
bool state[44] = {0};
bool laststate[44] = {0};
int tolerance = 2;

int teststate = LOW;
int lastteststate = LOW;

void getColorValuesFromEEPROM(led_color *colorStructArray) {
  int eedirection = 0;
  int h = 0;
  for (int i = 0; i < 16; i++) {
    for (int j = 0; j < 3; j++) {
      colorStructArray[i].subcolor[j] = EEPROM[h + j];
    }
    h += 3;
  }
}

void getSingleColorValueFromEEPROM(led_color *sing_color) {
  for (int j = 0; j < 3; j++) {
    sing_color->subcolor[j] = EEPROM[48 + j];
  }
}

void writeColorValuesToEEPROM(unsigned char led_number, led_color *color) {
  int eedirection;
  eedirection = led_number * 3;
  EEPROM.put(eedirection, color->subcolor[0]);
  EEPROM.put(eedirection + 1, color->subcolor[1]);
  EEPROM.put(eedirection + 2, color->subcolor[2]);
}

void writeSingleColorValueToEEPROM(led_color *single) {
  EEPROM.put(48, single->subcolor[0]);
  EEPROM.put(49, single->subcolor[1]);
  EEPROM.put(50, single->subcolor[2]);
}

void setLedMode(unsigned char n) { // 0 - All On; 1 - All Off; 2 - Blink With Delay; 3 - On When Pressed
  ledMode = n;
}

void setSingleLed(unsigned char led_number, led_color colorStructArray) {
  strip.setPixelColor(led_number, colorStructArray.subcolor[0], colorStructArray.subcolor[1], colorStructArray.subcolor[2]);
  strip.show();
}

void clearSingleLed(unsigned char led_number) {
  strip.setPixelColor(led_number, 0, 0, 0);
  strip.show();
}

void setAllLeds(uint8_t mode) { // 0 - All On; 1 - All Off;
  if (mode == 0) {
    digitalWrite(13, HIGH);
    for (int i = 0; i < 16; i++) {
      strip.setPixelColor(i, single_color.subcolor[0], single_color.subcolor[1], single_color.subcolor[2]);
      strip.show();
    }
  } else if (mode == 1) {
    for (int i = 0; i < 16; i++) {
      strip.setPixelColor(i, 0, 0, 0);
      strip.show();
    }
  }
}

void pushbuttonsRead() {
  for (int i = 4; i < 16; i++) {
    state[i + 30] = digitalRead(i); //This statement read the state of the digital pin
    if (state[i + 30] != laststate[i + 30]) { //This two if conditionals make the interrupt and "debounce" the button so it only will send data when the state changes, and that state is LOW
      if (state[i + 30] == 0) {
        MIDI.sendNoteOn(i + 30, 127, 1); //This send the MIDI data---  NOTE 56, VELOCITY 127, CHANNEL 1
      }
    }
    laststate[i + 30] = state[i + 30];
  }
  for (int i = 22; i < 54; i++) { //32 BUTTONS
    state[i - 22] = digitalRead(i); //This statement read the state of the digital pin
    if (state[i - 22] != laststate[i - 22]) { //This two if conditionals make the interrupt and "debounce" the button so it only will send data when the state changes, and that state is LOW
      if (state[i - 22] == 0) {
        MIDI.sendNoteOn(i - 21, 127, 1); //This send the MIDI data---  NOTE 56, VELOCITY 127, CHANNEL 1
        if ((ledMode == 3) && (i >= 22 && i <= 38)) {
          setSingleLed(i - 22, color[i - 22]);
        }
      } else {
        clearSingleLed(i - 22);
      }
      laststate[i - 22] = state[i - 22];
    }
  }
}

void potentiometersRead() {
  if (default_init == HIGH) {
    for (int i = 0; i < 6; i++) {   //Here you read the potentiometers data, convert them from 10 bit(Arduino's ADC read resolution) to 7 bit (MIDI protocol resolution) and send them
      valpot[i] = analogRead(potenciometro[i]);
      diff[i] = abs(valpot[i] - lastval[i]);
      if (diff[i] > tolerance) {
        MIDI.sendControlChange(i + i, valpot[i] >> 3, 3);
      }
      lastval[i] = valpot[i];
    }
  } else if (default_init == LOW) {
    for (int i = 0; i < 6; i++) {   //Here you read the potentiometers data, convert them from 10 bit(Arduino's ADC read resolution) to 7 bit (MIDI protocol resolution) and send them
      valpot[i] = analogRead(potenciometro[i]);
      diff[i] = abs(valpot[i] - lastval[i]);
      if (diff[0] > tolerance) {
        Serial.println(valpot[0] >> 3);
      }
      lastval[i] = valpot[i];
    }
    digitalWrite(13, HIGH);
  }
}

void setup()
{

  strip.begin();
  pinMode(13, OUTPUT);
  pinMode(3, INPUT_PULLUP);
  Serial.begin(9600);
  for (int i = 30; i < 46; i++) {
    pinMode(i, INPUT_PULLUP);  //Set the pushbuttons pins as input with a PULLUP resistor
  }
  pinMode(50, INPUT_PULLUP);
  getSingleColorValueFromEEPROM(&single_color);
  getColorValuesFromEEPROM(color);

  default_init = digitalRead(3);
  if (default_init == LOW) {
    Serial.begin(9600);
  } else {
    MIDI.begin(MIDI_CHANNEL_OMNI);  //Here the MIDI communication starts
    default_init = HIGH;
  }
}

void loop()
{

  if (Serial.available()) {
    String str_data = Serial.readString();
    int firstCommaIndex = str_data.indexOf(',');
    int secondCommaIndex = str_data.indexOf(',', firstCommaIndex + 1);
    int thirdCommaIndex = str_data.indexOf(',', secondCommaIndex + 1);

    String input_led_number = str_data.substring(0, firstCommaIndex);
    String red_input = str_data.substring(firstCommaIndex + 1, secondCommaIndex);
    String green_input = str_data.substring(secondCommaIndex + 1, thirdCommaIndex);
    String blue_input = str_data.substring(thirdCommaIndex + 1);

    color[input_led_number.toInt()].subcolor[0] = red_input.toInt();
    color[input_led_number.toInt()].subcolor[1] = green_input.toInt();
    color[input_led_number.toInt()].subcolor[2] = blue_input.toInt();

    writeColorValuesToEEPROM((unsigned char)input_led_number.toInt(), &color[input_led_number.toInt()]);
    getColorValuesFromEEPROM(color);

  }
  if (ledMode == 0 || ledMode == 1) { //ALL ON AND ALL OFF MODES
    setAllLeds(ledMode);
  } else if (ledMode == 2) { //BLINK WITH DELAY MODE
    runEvery(interval) {
      if (ledState == LOW) {
        ledState = HIGH;
      }
      else {
        ledState = LOW;
      }
      setAllLeds(ledState);
    }
  }

  static int pos = 60;
  encoder.tick();

  int newPos = encoder.getPosition(); //Here you read the encoder position and if it has changed from the last time, you send it
  if (pos != newPos) {
    if (pos < newPos) {
      MIDI.sendControlChange(15, 1, 2);
    } else if (pos > newPos) {
      MIDI.sendControlChange(15, 127, 2);
    }
    pos = newPos;
  }
  static int pos1 = 60;
  encoder1.tick();

  int newPos1 = encoder1.getPosition();
  if (pos1 != newPos1) {
    if (pos1 < newPos1) {
      MIDI.sendControlChange(16, 1, 2);
    } else if (pos1 > newPos1) {
      MIDI.sendControlChange(16, 127, 2);
    }
    pos1 = newPos1;
  }


  pushbuttonsRead();

  potentiometersRead();

}
