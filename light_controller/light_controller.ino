const int pin1 = 2; // D2
const int pin2 = 3; // D3
const int pin3 = 4; // D4
const int pin4 = 5; // D5
const int pin5 = 6; // D6
const int pin6 = 7; // D7
const int pin7 = 8; // D8
const int pin8 = 9; // D9
const int pin9 = 10; // D10
const int pin10 = 11; // D11
const int pin11 = 12; // D12
const int pin12 = 13; // D13
const int pin13 = 14; // A0
const int pin14 = 15; // A1
const int pin15 = 16; // A2
const int pin16 = 17; // A3

int light[16] = {pin1,pin2,pin3,pin4,pin5,pin6,pin7,pin8,pin9,pin10,pin11,pin12,pin13,pin14,pin15,pin16};


void setup() {
  pinMode(pin1, OUTPUT);
  pinMode(pin2, OUTPUT);
  pinMode(pin3, OUTPUT);
  pinMode(pin4, OUTPUT);
  pinMode(pin5, OUTPUT);
  pinMode(pin6, OUTPUT);
  pinMode(pin7, OUTPUT);
  pinMode(pin8, OUTPUT);
  pinMode(pin9, OUTPUT);
  pinMode(pin10, OUTPUT);
  pinMode(pin11, OUTPUT);
  pinMode(pin12, OUTPUT);
  pinMode(pin13, OUTPUT);
  pinMode(pin14, OUTPUT);
  pinMode(pin15, OUTPUT);
  pinMode(pin16, OUTPUT);
  Serial.begin(9600);
  while(!Serial) {
    ; // Wait for serial to connect, only needed for native USB port
  }
}

void Toggle(int &val)
{
  if (val < 1 || val > 16)
  {
    Serial.println("Invalid input range");
    return;
  }
  val -=1;
  if (digitalRead(light[val]) == LOW)
  {
    digitalWrite(light[val], HIGH);
    //Serial.println("LIGHT ON");
    PrintLight(light[val], "ON");
  }
  else
  {
    digitalWrite(light[val], LOW);
    // Serial.println("LIGHT OFF");
    PrintLight(light[val], "OFF");
  }    
}

void PrintLight(int number, String value)
{
  Serial.print("Light No: ");
  Serial.print(number, DEC);
  Serial.print(" Value ");
  Serial.print(value);
  Serial.println();
}

void loop() {
  // Check and enable correct output  
  if(Serial.available())
  {
    int input = Serial.parseInt();
    Serial.println(input, DEC);
    Toggle(input);
  }  
}
