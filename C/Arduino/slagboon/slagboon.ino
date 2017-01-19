#include <Servo.h>

#define trigPin 1
#define echoPin 2
#define servo   4  // servo

Servo myservo;

long duration, distance;
int pos = 180;

void setup() {
  Serial.begin(9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  myservo.attach(servo);
  myservo.write(pos);
  delay(200);
}

void loop(){
  digitalWrite(trigPin, LOW);  //PULSE ___|---|___
  delayMicroseconds(2); 
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10); 
  digitalWrite(trigPin, LOW);
  duration = pulseIn(echoPin, HIGH);
  distance = (duration/2) / 29.1;
  delay(500);
  Serial.println(distance);
  if(distance<5 && distance!=0){
    for (pos = pos; pos >= 90; pos -= 1) { // goes from 180 degrees to 90 degrees
    // in steps of 1 degree
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);                       // waits 15ms for the servo to reach the position
    }
  }
  else if(distance>5 && distance!=0){
    for (pos = pos; pos <= 180; pos += 1) { // goes from 90 degrees to 180 degrees
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);                       // waits 15ms for the servo to reach the position
    }
  }
}
