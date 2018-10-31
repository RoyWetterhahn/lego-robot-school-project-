/* Missie nummer 1 "M01–Space Travel"
 *Volledig geschreven ~Roy */
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

			InfoDialog dialog = new InfoDialog ("verbind een motor aan port A B en C");					//print iets op het lcd scherm van de brick
			dialog.Show ();																				//wacht todat op ok is gedrukt
			sbyte turningSpeed = 30;																	//hoe hoger deze waarden hoe sneller de robot draaid

			Motor motorR = new Motor (MotorPort.OutB);													//definieeren van de rechtermotor
			Motor motorL = new Motor (MotorPort.OutA);													//definieeren van de linkermotor
			Motor motorU = new Motor (MotorPort.OutC);                                                  //definieeren van de robot arm motor

            //ga er vanuit dat deze functies namen logisch genoeg zijn en geen comments nodig hebben
            DriveStraight(2350 , 80, motorL, motorR);													
			TurnLeft (turningSpeed, 45, motorR, motorL);
			DriveStraight (150 , 50, motorL, motorR);
			ArmUp (800, 18, motorU);
			Thread.Sleep (1500);
			ArmUp (800, -18, motorU);

			Shutdown (motorL, motorR);																	//afsluiten van het programma
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
			motorR.Off();
			motorL.Off();
		}

		static void TurnLeft(sbyte turningSpeed, double angle, Motor motorR, Motor motorL)		        //om de robot een bocht naar links te laten maken
		{
			double time = angle * 10.8;																	//kijken hoelang de robot moet draaien voordat hij het juiste aantal graden heeft gedraaid.
			int roundedTime = Convert.ToInt32 (time);													//de double tijd wordt hier naar een int geconvert zodat hij bij Thread.sleep gebruikt kan worden
			int reverseTurningSpeedOld = 0 - (Convert.ToInt32 (turningSpeed));							//een waarschijnlijk slechte manier om van een sbyte een negatieve sbyte te maken zonder een error te krijgen
			sbyte reverseTurningSpeed = Convert.ToSByte (reverseTurningSpeedOld);
			motorL.SetSpeed (turningSpeed);
			motorR.SetSpeed (reverseTurningSpeed);
			Thread.Sleep (roundedTime);
			motorR.Off();
			motorL.Off(); 
		}

		static void TurnRight(sbyte turningSpeed, double angle, Motor motorR, Motor motorL)		        //om de robot een bocht naar rechts te laten maken
		{
			double time = angle * 10.8;																	//kijken hoelang de robot moet draaien voordat hij het juiste aantal graden heeft gedraaid.
			int roundedTime = Convert.ToInt32 (time);													//de double tijd wordt hier naar een int geconvert zodat hij bij Thread.sleep gebruikt kan worden
			int reverseTurningSpeedOld = 0 - (Convert.ToInt32 (turningSpeed));							//een waarschijnlijk slechte manier om van een sbyte een negatieve sbyte te maken zonder een error te krijgen
			sbyte reverseTurningSpeed = Convert.ToSByte (reverseTurningSpeedOld);
			Convert.ToSByte (reverseTurningSpeed);
			motorL.SetSpeed (reverseTurningSpeed);																										
			motorR.SetSpeed (turningSpeed);
			Thread.Sleep (roundedTime);
			motorR.Off();																				
			motorL.Off(); 
		}

		static void Shutdown(Motor motorR, Motor motorL)
		{
			motorR.Off();																				//voor de zekerheid dat alle motoren stoppen met draaien na het afsluiten																			
			motorL.Off();
			Lcd.Clear ();																				//het lcd scherm vrijmaken van de text en het vrijgemaakte scherm tonen																			
			Lcd.Update ();
		}
	} 
} 

