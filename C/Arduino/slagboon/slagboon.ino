#include <Servo.h> // libarie for the servo

#define trigPin 1  // ultrasonic sensor trig
#define echoPin 2  // ultrasonic sensor echo
#define servo   4  // servo

Servo myservo;

int duration, distance1;
int pos = 170;                // startposition servo
int detectie_distance = 10;   // distance barrier closes
int distance = detectie_distance;
bool up = false;
bool down = false;

void setup() {
  Serial.begin(9600);
  
  //setup for ultrasonic sensor
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  
  //setup for servo
  myservo.attach(servo);
  myservo.write(pos);
  delay(200);
}

void loop(){
  if(!up || !down){
    slagboom();
  }
}

void slagboom(){
  digitalWrite(trigPin, LOW);   // send pulse
  delayMicroseconds(2); 
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10); 
  digitalWrite(trigPin, LOW);
  duration = pulseIn(echoPin, HIGH);  // receive pulse
  distance1 = (duration/2) / 29.1;    // calculate distance in cm
  delay(500);
  if(distance1!=0){       // servo wil stil function correctly get distance 0
    distance = distance1;
  }
  Serial.print("Distance: ");   // display distance on monitor
  Serial.println(distance);

  // check if barrier should be up or down
  if(distance<detectie_distance){       // barrier down
    Serial.println("DOWN");
    for (pos = pos; pos > 85; pos -= 1) { // goes from pos degrees to 85 degrees
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);                       // waits 15ms for the servo to reach the position
    down = true;
    }
  }
  else{                         // barrier up
    Serial.println("UP");
    for (pos = pos; pos < 170; pos += 1) { // goes from pos degrees to 170 degrees
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);                       // waits 15ms for the servo to reach the position
    up = true;
    }
  }
}

