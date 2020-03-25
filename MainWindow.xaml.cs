﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace DoomTrainer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		globalKeyboardHook hook = new globalKeyboardHook();
		SigScanTarget consoleCmdScanTarget = new SigScanTarget("4C8B0EBA01000000488BCE448BF041FF51??8D");
		SigScanTarget consoleBindScanTarget = new SigScanTarget("4C8B0FBA01000000488BCF448BF041FF51??4C6BC507");


		IntPtr xLocPtr;
		IntPtr yLocPtr;
		IntPtr zLocPtr;
		IntPtr xVelPtr;
		IntPtr yVelPtr;
		IntPtr zVelPtr;
		IntPtr xRotPtr;
		IntPtr yRotPtr;
		DeepPointer raxPointer = new DeepPointer("DOOMEternalx64vk.exe", 0x06121BB8, 0x38, 0x28, 0x0);
		DeepPointer eaxPointer = new DeepPointer("DOOMEternalx64vk.exe", 0x04C7CA08, 0xCB0, 0xDF8, 0x1D0, 0x88);
		DeepPointer xVelDP = new DeepPointer("DOOMEternalx64vk.exe", 0x04C7CA08, 0x1510, 0x598, 0x1D0,  0x3F40);
		DeepPointer rotDP = new DeepPointer("DOOMEternalx64vk.exe", 0x4C83F38);
		float[] storedPos = new float[3] { 0f, 0f, 0f };
		float[] storedVel = new float[3] { 0f, 0f, 0f };
		float[] storedRot = new float[2] { 0f, 0f };
		Process process;

		DeepPointer restrictedCommandsDPTR = new DeepPointer("DOOMEternalx64vk.exe", 0x442741);
		DeepPointer restrictedKeyPressDPTR = new DeepPointer("DOOMEternalx64vk.exe", 0x44FCE7);
		IntPtr restrictedCommandsPtr;
		IntPtr restrictedKeyPressPtr;

		bool retainVel;


		public MainWindow()
		{
			InitializeComponent();
			hook.KeyDown += Hook_KeyDown;
			hook.KeyUp += Hook_KeyUp;
			hook.HookedKeys.Add(System.Windows.Forms.Keys.F5);
			hook.HookedKeys.Add(System.Windows.Forms.Keys.F6);

			Timer timer = new Timer();
			timer.Interval = (16); // 60 Hz
			timer.Tick += new EventHandler(updateTick);
			timer.Start();

			retainVel = false;

		}

		public void updateTick(object snder, EventArgs e){

			
			if (!GetPointers())
				return;
			if (process.HasExited)
			{
				process = null;
				return;
			}
			

			float curX;
			process.ReadValue<float>(xLocPtr, out curX);
			float curY;
			process.ReadValue<float>(yLocPtr, out curY);
			float curZ;
			process.ReadValue<float>(zLocPtr, out curZ);

			float curXv;
			process.ReadValue<float>(xVelPtr, out curXv);
			float curYv;
			process.ReadValue<float>(yVelPtr, out curYv);
			float curZv;
			process.ReadValue<float>(zVelPtr, out curZv);

			double hVel = Math.Sqrt(curXv * curXv + curYv * curYv);

			unlockButton.IsEnabled = restrictedCommandsDPTR.DerefBytes(process, 1)[0] > 0;

			PosBlock.Text = "Current Position\nX: " + curX.ToString("0.00") + "\nY: " + curY.ToString("0.00") + "\nZ: " + curZ.ToString("0.00") + "\n\n\nStored Position\nX: " + storedPos[0].ToString("0.00") + "\nY: " + storedPos[1].ToString("0.00") + "\nZ: " + storedPos[2].ToString("0.00");
			VelBlock.Text = "Current Velocity\nX: " + curXv.ToString("0.00") + "\nY: " + curYv.ToString("0.00") + "\nZ: " + curZv.ToString("0.00") + "\n"+hVel.ToString("0.00")+"m/s\n\nStored Velocity\nX: " + storedVel[0].ToString("0.00") + "\nY: " + storedVel[1].ToString("0.00") + "\nZ: " + storedVel[2].ToString("0.00");
		}

		public void Update(){
			Debug.WriteLine("test");
		}

		private void Hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void Hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (GetPointers())
			{
				if (e.KeyCode == System.Windows.Forms.Keys.F5)
				{
					process.ReadValue<float>(xLocPtr, out storedPos[0]);
					process.ReadValue<float>(yLocPtr, out storedPos[1]);
					process.ReadValue<float>(zLocPtr, out storedPos[2]);
					process.ReadValue<float>(xVelPtr, out storedVel[0]);
					process.ReadValue<float>(yVelPtr, out storedVel[1]);
					process.ReadValue<float>(zVelPtr, out storedVel[2]);
					process.ReadValue<float>(xRotPtr, out storedRot[0]);
					process.ReadValue<float>(yRotPtr, out storedRot[1]);
				}

				if (e.KeyCode == System.Windows.Forms.Keys.F6)
				{
					
					process.WriteBytes(xLocPtr, BitConverter.GetBytes(storedPos[0]));
					process.WriteBytes(yLocPtr, BitConverter.GetBytes(storedPos[1]));
					process.WriteBytes(zLocPtr, BitConverter.GetBytes(storedPos[2]+ 1f));
					process.WriteBytes(xRotPtr, BitConverter.GetBytes(storedRot[0]));
					process.WriteBytes(yRotPtr, BitConverter.GetBytes(storedRot[1]));

					if (retainVel)
					{
						process.WriteBytes(xVelPtr, BitConverter.GetBytes(storedVel[0]));
						process.WriteBytes(yVelPtr, BitConverter.GetBytes(storedVel[1]));
						if (Math.Abs(storedVel[2]) < 1f)
							storedVel[2] = 1f;
						process.WriteBytes(zVelPtr, BitConverter.GetBytes(storedVel[2]));
					}
					else
					{
						process.WriteBytes(xVelPtr, BitConverter.GetBytes(0f));
						process.WriteBytes(yVelPtr, BitConverter.GetBytes(0f));
						process.WriteBytes(zVelPtr, BitConverter.GetBytes(1f));

					}
				}
			}
			e.Handled = true;
		}



		private bool GetPointers()
		{
			List<Process> processList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
			if (processList.Count == 0)
				return false;
			process = processList[0];
			if (process.HasExited)
			{
				process = null;
				return false;
			}

			IntPtr raxIntPtr;
			raxPointer.DerefOffsets(process, out raxIntPtr);
			IntPtr eaxIntPtr;
			eaxPointer.DerefOffsets(process, out eaxIntPtr);
			int eaxValue = process.ReadValue<int>(eaxIntPtr);
			eaxValue &= 0xFFFFFF;
			eaxValue *= 0xB0;
			IntPtr posPointer = new IntPtr(raxIntPtr.ToInt64() + eaxValue + 0x30);

			

			float xFloat;
			process.ReadValue<float>(posPointer, out xFloat);

			xLocPtr = posPointer;
			yLocPtr = posPointer + 4;
			zLocPtr = posPointer + 8;
			
			xVelDP.DerefOffsets(process, out xVelPtr);
			yVelPtr = xVelPtr + 4;
			zVelPtr = xVelPtr + 8;

			rotDP.DerefOffsets(process, out xRotPtr);
			yRotPtr = xRotPtr + 4;




			return true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			unlockButton.IsEnabled = false;
			if (!GetPointers())
			{
				unlockButton.IsEnabled = true;
				return;
			}
			

			SignatureScanner sigScanner = new SignatureScanner(process, process.MainModuleWow64Safe().BaseAddress, process.MainModuleWow64Safe().ModuleMemorySize);
			restrictedCommandsPtr = sigScanner.Scan(consoleCmdScanTarget);
			restrictedKeyPressPtr = sigScanner.Scan(consoleBindScanTarget);
			if (restrictedCommandsPtr.ToInt64() != 0 && restrictedKeyPressPtr.ToInt64() != 0)
			{
				restrictedKeyPressPtr += 4;
				restrictedCommandsPtr += 4;
				Console.WriteLine(restrictedKeyPressPtr.ToInt64().ToString("X16"));
				Console.WriteLine(restrictedCommandsPtr.ToInt64().ToString("X16"));
				process.WriteBytes(restrictedCommandsPtr, BitConverter.GetBytes(0));
				process.WriteBytes(restrictedKeyPressPtr, BitConverter.GetBytes(0));
				System.Windows.Forms.MessageBox.Show("Console commands unlocked", "Success");
			}else{
				System.Windows.Forms.MessageBox.Show("Memory Address not found.\nEither console commands are already unlocked, or your game version is not supported.", "Error");
			}

			unlockButton.IsEnabled = true;
		}

		private void zeroRB_Checked(object sender, RoutedEventArgs e)
		{
			if (zeroRB == null || retainRB == null)
				return;
			zeroRB.IsChecked = true;
			retainRB.IsChecked = false;
			retainVel = false;
		}

		private void retainRB_Checked(object sender, RoutedEventArgs e)
		{
			if (zeroRB == null || retainRB == null)
				return;
			zeroRB.IsChecked = false;
			retainRB.IsChecked = true;
			retainVel = true;
		}
	}
}


