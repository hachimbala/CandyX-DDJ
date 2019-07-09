typedef struct{
	uint8_t subcolor[3];
} led_color;

void getColorValuesFromEEPROM(led_color colorStructArray[]);

void getSingleColorValueFromEEPROM(led_color *sing_color);

void writeColorValuesToEEPROM(unsigned char led_number, led_color *color);

void writeSingleColorValueToEEPROM(unsigned char red, unsigned char green, unsigned char blue);

void setLedMode(uint8_t n);

void setSingleLed(unsigned char led_number, led_color colorStructArray[]);

void setAllLeds(unsigned char mode);

void pushbuttonsRead();
