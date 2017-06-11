const int pin1 = 2;   // D2
const int pin2 = 3;   // D3
const int pin3 = 4;   // D4
const int pin4 = 5;   // D5
const int pin5 = 6;   // D6
const int pin6 = 7;   // D7
const int pin7 = 8;   // D8
const int pin8 = 9;   // D9
const int pin9 = 10;  // D10
const int pin10 = 11; // D11
const int pin11 = 12; // D12
const int pin12 = 13; // D13
const int pin13 = 14; // A0
const int pin14 = 15; // A1
const int pin15 = 16; // A2
const int pin16 = 17; // A3

int light[16] = {pin1,pin2,pin3,pin4,pin5,pin6,pin7,pin8,pin9,pin10,pin11,pin12,pin13,pin14,pin15,pin16};
#define NO_OF_LIGHTS  sizeof(light) / sizeof(light[0])
String ver = "lightControls v0.1";

void setup() {
  for (int i = 0; i < NO_OF_LIGHTS; i++) {
    pinMode(light[i], OUTPUT);
  }
  Serial.begin(9600);
  while(!Serial) {
    ; // Wait for serial to connect, only needed for native USB port
  }
}

void PrintLight(int number, int value)
{
  Serial.print("Light: ");
  Serial.print(number, DEC);
  Serial.print(" State: ");
  Serial.print(value, DEC);
  Serial.println();
}

void PrintLights()
{
  for (int i = 0; i< NO_OF_LIGHTS; i++) {
    PrintLight(i+1, GetLightStatus(i));
  }
}

int GetLightStatus(int index)
{
  if (digitalRead(light[index]) == LOW) {
    return 0;
  }
  else {
    return 1;
  }
}

void ParseSerialData()
{
  int cmd = Serial.parseInt();   
  if (cmd == 255) {
    PrintLights();
  }
  else if(cmd == 254) {
    Serial.println(ver);
  }
  else
  {
    if (cmd < 1 || cmd > 16) {
      Serial.println("Invalid input range");
      Serial.println(cmd, DEC);
    }      
    else {
      int value = Serial.parseInt();
      if (value == 1) {
        digitalWrite(light[cmd-1], HIGH);
      }
      else if(value == 0){
        digitalWrite(light[cmd-1], LOW);
      }
      PrintLight(cmd, GetLightStatus(cmd-1));
    }  
  }  
}

void loop() {
  // Check for incoming data
  if(Serial.available() > 0)
  {
    ParseSerialData();
  }  
}
