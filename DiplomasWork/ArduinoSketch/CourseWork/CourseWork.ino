#include <hcsr04.h>
#include <Servo.h>
#define LIGHTS 2
#define MOTORFWD 3
#define ULTRASONIC_FWD 4
#define ULTRASONIC_LFT 5
#define ULTRASONIC_RGHT 6
#define TRIG 7
#define SERVOPIN 9
#define MOTORBACK 11
#define STBY 12

Servo myservo;
HCSR04 hc(TRIG, ULTRASONIC_FWD, 20, 4000);
HCSR04 hc_left(TRIG, ULTRASONIC_LFT, 20, 4000);
HCSR04 hc_right(TRIG, ULTRASONIC_RGHT, 20, 4000);

char data[2];
char Command;
int recievedNumber = 0;
bool automatic = false;
bool stopMove = false;
int maxLeftAngle = 40;
int maxRightAngle = 90;
int directAngle = 110;

void setup()
{
    Serial.begin(9600);
    pinMode(MOTORFWD, OUTPUT);
    pinMode(MOTORBACK, OUTPUT);
    pinMode(STBY, OUTPUT);
    pinMode(LIGHTS, OUTPUT);
    digitalWrite(LIGHTS, LOW);
    myservo.attach(SERVOPIN);
    myservo.write(90);
    delay(1000);
}

void loop()
{
    if (Serial.available() > 0)
    {
        Command = Serial.readBytes(data, 2);
        recievedNumber = convertNumberFromChar(data[1]);
        EncodeData();
    }
    if(stopMove == true){ 
      HardStop();
      myservo.write(directAngle);
      stopMove = false;
    }
    else if (automatic == true)
    {
        AutoMove();
    }
}

void AutoMove()
{
  //calculate average distantion
  delay(250);
  
    int forwarddist = hc.distanceInMillimeters() / 10;
    delay(2);
    int leftdist = hc_left.distanceInMillimeters() / 10;
    delay(2);
    int rightdist = hc_right.distanceInMillimeters() / 10;
    delay(2);
    int forwarddist2 = hc.distanceInMillimeters() / 10;
    delay(2);
    int leftdist2 = hc_left.distanceInMillimeters() / 10;
    delay(2);
    int rightdist2 = hc_right.distanceInMillimeters() / 10;
    
    int average_dist = (forwarddist + forwarddist2) / 2;
    int average_dist_left = (leftdist + leftdist2) / 2;
    int average_dist_right = (rightdist + rightdist2) / 2;

    
    if(average_dist < 10) 
    {
      HardStop();
      myservo.write(directAngle);
    }
    else if(average_dist < 40)
    {
      HardStop();
      delay(500);
      if(average_dist_left > average_dist_right)
      { 
      myservo.write(maxLeftAngle);
      }
      else 
      {
      myservo.write(maxRightAngle);
      }
      delay(500);
      digitalWrite(STBY, HIGH);
      digitalWrite(MOTORBACK, LOW);
      analogWrite(MOTORFWD, 180);
     // Serial.println("TURN");
      delay(1000);
      HardStop();
    }
    else 
    {
      myservo.write(directAngle);
      digitalWrite(STBY, HIGH);
      digitalWrite(MOTORBACK, LOW);
      analogWrite(MOTORFWD, 180);
      //Serial.println("MOVE");
      delay(1000);
      HardStop();
}
delay(250);
}

void EncodeData()
{
    switch (data[0])
    {
        case 'G'://Max Right Position
            automatic = false;
            maxRightAngle = (recievedNumber + 1) * 18;
            myservo.write(maxRightAngle);
            break;
        case 'T': // Max Left Position
            automatic = false;
            maxLeftAngle = (recievedNumber + 1) * 18;
            myservo.write(maxLeftAngle);
            break;
        case 'D': //Direct Position Angle
            automatic = false;
            directAngle = (recievedNumber + 1) * 18;
            myservo.write(directAngle);
            break;
        case 'F': // Move Forward
            automatic = false;
            digitalWrite(STBY, HIGH);
            digitalWrite(MOTORBACK, LOW);
            analogWrite(MOTORFWD, recievedNumber * 25);
            break;
        case 'B': //Move Back
            automatic = false;
            digitalWrite(STBY, HIGH);
            digitalWrite(MOTORFWD, LOW);
            analogWrite(MOTORBACK, recievedNumber * 25);
            break;
        case 'R'://Turn Right
            automatic = false;
            myservo.write((recievedNumber + 1) * 18);
            break;
        case 'L': // Turn Left
            automatic = false;
            myservo.write((recievedNumber + 1) * 18);
            break;
        case 'M': // Set Direct Position
            automatic = false;
            myservo.write(directAngle);
            break;
        case 'S': // Stop Moving
            automatic = false;
            HardStop();
            break;
        case 'I': // Turn on/off the light
            automatic = false;
            digitalWrite(LIGHTS, recievedNumber);
            break;
        case 'A': //Automatic mode
            automatic = recievedNumber == 1 ? true : false;
            if(automatic == true) stopMove = false;
            else stopMove = true;
            break;
    }

}

void HardStop()
{
   digitalWrite(STBY, LOW);
   digitalWrite(MOTORFWD, LOW);
   digitalWrite(MOTORBACK, LOW);
}

int convertNumberFromChar(char symbol)
{
    return (int)(symbol - '0');
}
