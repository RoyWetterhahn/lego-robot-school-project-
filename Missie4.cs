/* Missie nummer 4 "M13–Observatory"
 *Geschreven ~Roy */

using System;
using MonoBrickFirmware;
using MonoBrickFirmware.Display.Dialogs;
using MonoBrickFirmware.Display;
using MonoBrickFirmware.Movement;
using System.Threading;
using MonoBrickFirmware.Sensors;

namespace MonoBrickHelloWorld

{ 
	class MainClass
	{

		public static void Main (string[] args)
		{
			InfoDialog dialog = new InfoDialog ("Missie 4");					                        //print iets op het lcd scherm van de brick
			dialog.Show ();																				//wacht todat op ok is gedrukt

			Motor motorR = new Motor (MotorPort.OutB);													//definieeren van de rechtermotor
			Motor motorL = new Motor (MotorPort.OutA);													//definieeren van de linkermotor
			Motor motorU = new Motor (MotorPort.OutC);													//definieeren van de robot arm motor
			EV3ColorSensor sensor = new EV3ColorSensor(SensorPort.In4);									//definieeren van de kleurensensor
			EV3GyroSensor gyroSensor = new EV3GyroSensor(SensorPort.In2, GyroMode.Angle);               //definieeren van de gyrosensor

            //ga er vanuit dat deze functies namen logisch genoeg zijn en geen comments nodig hebben
            DriveStraight(4000, 45, motorR, motorL);
			DriveToColor (30, "White", motorR, motorL, sensor);
			DriveToColor (12, "Black", motorR, motorL, sensor);
			Turning (gyroSensor, -40, motorL, motorR);
			DriveStraight (1500, 30, motorR, motorL);
			DriveToColor (20, "White", motorR, motorL, sensor);
			DriveStraight (1800, 30, motorR, motorL);
			Turning (gyroSensor, 80, motorL, motorR);
			DriveStraight (3500, 30, motorR, motorL);


			DriveToColor (-25, "White", motorR, motorL, sensor);
			DriveStraight (500, -30, motorR, motorL);
			Turning (gyroSensor, -80, motorL, motorR);
			DriveStraight (2500, -30, motorR, motorL);
			DriveToColor (-20, "White", motorR, motorL, sensor);
			DriveToColor (-12, "Black", motorR, motorL, sensor);
			Turning (gyroSensor, 40, motorL, motorR);
			DriveStraight (8000, -60, motorR, motorL);

			LcdConsole.WriteLine("shutdown");
			Shutdown (motorL, motorR, motorU);															//afsluiten van het programma
		}


		static void Turning(EV3GyroSensor gyroSensor, int draaiGraden, Motor motorL, Motor motorR)
		{
			gyroSensor.Reset ();
			sbyte draaiSnelheid = 20;
			sbyte minDraaiSnelheid = -20;

			int graden = 0;
			int counter = 0;
			int updateTime = 8;
			if (draaiGraden > 0) {																		//als hij naar rechts moet draaien dus een positief getal is
				LcdConsole.WriteLine ("GROTER ALS 0 ", draaiGraden);	
				motorL.SetSpeed (draaiSnelheid);
				motorR.SetSpeed (minDraaiSnelheid);
				while (graden <= draaiGraden)															//terwijl het aantal graden dat hij gedraaid is kleiner is dan het aantal graden dat hij moet draaien
				{
					LcdConsole.WriteLine ("Gyro sensor: " + gyroSensor.ReadAsString ());
					graden = gyroSensor.Read ();
					Thread.Sleep (updateTime);
					counter++;
					if (counter > 500) {
						break;
					}
				}
			}
			if (draaiGraden < 0) {																	    //als hij naar links moet draaien dus het aantal graden negatief is
				LcdConsole.WriteLine ("KLEINER ALS 0 ", draaiGraden);	
				motorL.SetSpeed (minDraaiSnelheid);
				motorR.SetSpeed (draaiSnelheid);
				while (graden >= draaiGraden)															//terwijl het aantal graden dat hij gedraaid is groter is dan het aantal graden dat hij moet draaien
				{
					LcdConsole.WriteLine ("Gyro sensor: " + gyroSensor.ReadAsString());	
					graden = gyroSensor.Read ();
					Thread.Sleep (updateTime);
					counter++;
					if (counter > 500){
						break;
					}
				}

			} 
			else
			{
				LcdConsole.WriteLine ("IK KAN GEEN 0 GRADEN DRAAIEN " + gyroSensor.ReadAsString());

			}
			motorR.Brake();
			motorL.Brake();
			motorR.Off();
			motorL.Off();
			Thread.Sleep (1000);
		}

		static void DriveToColor(sbyte speed, string Color, Motor motorR, Motor motorL, EV3ColorSensor sensor)
		{

			int i = 0;
			LcdConsole.WriteLine ("zoeken naar" + sensor.ReadAsString()+ "en "+ Color);
			Thread.Sleep (1000);
			motorL.SetSpeed (speed);
			motorR.SetSpeed (speed);
			while (Color != sensor.ReadAsString())
			{				LcdConsole.WriteLine("Sensor ẁaarde:" + sensor.ReadAsString());

				if (Color == (sensor.ReadAsString()))
				{
					LcdConsole.WriteLine("Sensor ẁaarde:" + sensor.ReadAsString());
					LcdConsole.WriteLine("GEVONDEN  " + sensor.ReadAsString());
					motorR.Off();
					motorL.Off();
					break;
				}
				Thread.Sleep (50);																        // wacht voor zoveel miliseconde voor de volgende loop
				i++;
				if (i > 60)																		        // als 5000 miliseconden voorbij zijn stop
				{
					LcdConsole.WriteLine("heeft de kleur niet kunnen vinden" );
					motorR.Off();
					motorL.Off();
					break;
				}
			}
		}
		static void ArmUp (int time, sbyte speed, Motor motorU)
		{
			sbyte turningSpeed = speed;
			motorU.SetSpeed (turningSpeed);
			Thread.Sleep (time);
			motorU.Off ();
		}

		static void DriveStraight(int time, sbyte speed, Motor motorR, Motor motorL)				    //Om de robot in een rechte lijn te laten rijden voor een bepaalde tijd
		{
			motorL.SetSpeed (speed);
			motorR.SetSpeed (speed);
			Thread.Sleep (time);
			motorR.Brake();
			motorL.Brake();
			motorR.Off();
			motorL.Off();
		}

		static void Shutdown(Motor motorR, Motor motorL, Motor motorU)
		{
			motorR.Off();																				//voor de zekerheid dat alle motoren stoppen met draaien na het afsluiten																			
			motorL.Off();
			motorU.Off ();
			Lcd.Clear ();																				//het lcd scherm vrijmaken van de text en het vrijgemaakte scherm tonen																			
			Lcd.Update ();
		}
	} 
} 

