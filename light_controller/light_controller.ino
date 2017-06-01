int state = 0;
const int pin1 = 1; // TX1
const int pin2 = 2; // D2
const int pin3 = 3; // D3
const int pin4 = 4; // D4
const int pin5 = 5; // D5
const int pin6 = 6; // D6
const int pin7 = 7; // D7
const int pin8 = 8; // D8
const int pin9 = 9; // D9
const int pin10 = 10; // D10
const int pin11 = 11; // D11
const int pin12 = 12; // D12
const int pin13 = 13; // D13
const int pin14 = 14; // A0
const int pin15 = 15; // A1
const int pin16 = 16; // A2

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
}

void loop() {
  // Check and enable correct output
  digitalWrite(pin2, (state) ? HIGH : LOW);
  state = !state;
  delay(5000);
}
