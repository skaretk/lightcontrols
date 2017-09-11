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
const int pin12 = 18; // A4
const int pin13 = 17; // A3
const int pin14 = 16; // A2
const int pin15 = 15; // A1
const int pin16 = 14; // A0

int light[16] = {pin1,pin2,pin3,pin4,pin5,pin6,pin7,pin8,pin9,pin10,pin11,pin12,pin13,pin14,pin15,pin16};
#define NO_OF_LIGHTS  sizeof(light) / sizeof(light[0])
String ver = "lightControls v0.2";

String input;

void setup() {
  for (int i = 0; i < NO_OF_LIGHTS; i++) {
    pinMode(light[i], OUTPUT);
  }
  Serial.begin(9600);
  while(!Serial) {
    ; // Wait for serial to connect, only needed for native USB port
  }
  Serial.println(ver + "\n");
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

void ParseCommand(String command)
{
  //Serial.println("Arduino parsing" + command);
  String cmd;
  String value;

  cmd = command.substring(0, command.indexOf(","));
  if (command == -1)
    cmd = command;
  else
  {
    value = command.substring(command.indexOf(",")+1);
    if (value == -1)
      value = "";
  }
  if (cmd.toInt() == 255) {
    PrintLights();
  }
  else if(cmd.toInt() == 254) {
    Serial.println(ver);
  }
  else
  {
    if (cmd.toInt() < 1 || cmd.toInt() > 16) {
      Serial.print("Invalid input range ");
      Serial.println(cmd);
    }      
    else {
      if (value.toInt() == 1) {
        digitalWrite(light[cmd.toInt()-1], HIGH);
      }
      else if(value.toInt() == 0){
        digitalWrite(light[cmd.toInt()-1], LOW);
      }
      PrintLight(cmd.toInt(), GetLightStatus(cmd.toInt()-1));
    }  
  }  
}

void loop() {
  // Check for incoming data
  if(Serial.available())
  {
    char c = Serial.read();
    if (c == "\r" || c == 0x0d || c == 0x0a)
    {
      ParseCommand(input);
      input= "";
      }
    else
      input += c;
  }  
}
