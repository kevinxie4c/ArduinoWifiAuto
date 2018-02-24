#include <SoftwareSerial.h>

#define IN1 A0
#define IN2 A1
#define IN3 A2
#define IN4 A3
#define PWM1 5
#define PWM2 6
#define SPEED_PIN1 2
#define SPEED_PIN2 3

SoftwareSerial mySerial(A4, A5);

bool is_cmd() {
  byte state=0;
  byte ch;
  if(!mySerial.available()) return false;
  switch(state) {
    case 0:
      ch=mySerial.read();
      if(ch=='+') state=1;
      else return false;
    case 1:
      while(!mySerial.available()) {;}
      ch=mySerial.read();
      if(ch=='+') state=1;
      else if(ch=='I') state=2;
      else return false;
    case 2:
      while(!mySerial.available()) {;}
      ch=mySerial.read();
      if(ch=='+') state=1;
      else if(ch=='P') state=3;
      else return false;
    case 3:
      while(!mySerial.available()) {;}
      ch=mySerial.read();
      if(ch=='+') state=1;
      else if(ch=='D') return true;
      else return false;
  }
  return false;
}

byte data[256];
int id,len;

void parse_cmd() {
  int i;
  id=mySerial.parseInt();
  len=mySerial.parseInt();
  Serial.println(mySerial.read());
  for(i=0;i<len;++i)
    data[i]=mySerial.read();
}
void setup() {
  pinMode(7,OUTPUT);
  digitalWrite(7,HIGH);
  Serial.begin(9600);
  while(!Serial) {;}

  pinMode(IN1,OUTPUT);
  pinMode(IN2,OUTPUT);
  pinMode(IN3,OUTPUT);
  pinMode(IN4,OUTPUT);
  pinMode(PWM1,OUTPUT);
  pinMode(PWM2,OUTPUT);
  
  mySerial.begin(9600);
  delay(1000);
  mySerial.write("AT+CIPMUX=1\r\n");
  delay(1000);
  mySerial.write("AT+CIPSERVER=1,333\r\n");
  delay(1000);
  mySerial.write("AT+CIPSTO=10000\r\n");
  delay(1000);
  Serial.println("You may start now");
}

void loop() {
  if(is_cmd()) {
    Serial.println("Got one");
    parse_cmd();
    Serial.println(id);
    Serial.println(len);
    Serial.println(data[0]);
    Serial.println(data[1]);
    switch(data[0]) {
      case 1:
        digitalWrite(IN1,HIGH);
        digitalWrite(IN2,HIGH);
        break;
      case 2:
        digitalWrite(IN3,HIGH);
        digitalWrite(IN4,HIGH);
        break;
      case 3:
        digitalWrite(IN1,HIGH);
        digitalWrite(IN2,HIGH);
        digitalWrite(IN3,HIGH);
        digitalWrite(IN4,HIGH);
        break;
      case 4:
        digitalWrite(IN1,HIGH);
        digitalWrite(IN2,LOW);
        analogWrite(PWM1,data[1]);
        break;
      case 5:
        digitalWrite(IN3,HIGH);
        digitalWrite(IN4,LOW);
        analogWrite(PWM2,data[1]);
        break;
      case 6:
        digitalWrite(IN1,HIGH);
        digitalWrite(IN2,LOW);
        digitalWrite(IN3,HIGH);
        digitalWrite(IN4,LOW);
        analogWrite(PWM1,data[1]);
        analogWrite(PWM2,data[1]);
        break;
      case 7:
        digitalWrite(IN1,LOW);
        digitalWrite(IN2,HIGH);
        analogWrite(PWM1,data[1]);
        break;
      case 8:
        digitalWrite(IN3,LOW);
        digitalWrite(IN4,HIGH);
        analogWrite(PWM2,data[1]);
        break;
      case 9:
        digitalWrite(IN1,LOW);
        digitalWrite(IN2,HIGH);
        digitalWrite(IN3,LOW);
        digitalWrite(IN4,HIGH);
        analogWrite(PWM1,data[1]);
        analogWrite(PWM2,data[1]);
        break;
    }
  }
}
