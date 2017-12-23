using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KaboomAdrioFxPlusTwoFeedersDllWrapper
{
    /// <summary>
    /// Wrapper class to access to the KaboomComDll functionality
    /// </summary>
    public class KaboomAdrioFxPlusTwoFeedersDll
    {
        /// <summary>
        /// Path to the KaboomComDll
        /// </summary>
		#if UNITY_EDITOR_64
        public const string DLL_PATH = "KaboomAdrioFxPlusTwoFeeders64";
		#else
        public const string DLL_PATH = "KaboomAdrioFxPlusTwoFeeders";
		#endif

        //private const string DLL_PATH = @"D:\data\SVN\CTAQ\Kaboom\Sources\KaboomComDll\Debug\KaboomComDLL.dll";
        //private const string DLL_PATH = @"D:\data\SVN\CTAQ\Kaboom\Sources\KaboomComDll\Release\KaboomComDLL.dll";
        //private const string DLL_PATH = @"D:\Data\SVN\CTAQ\ADRENALINE-AMUSEMENTS\KABOOM\KaboomComDll\Debug\LaneSplitterArcade_Data\Plugins\KaboomComDLL.dll";

        /// <summary>
        /// List of KaboomCom error/state
        /// </summary>
        public enum KABOOM_DLL_ERROR : byte
        {
            NO_ERROR = 0,
            SERIAL_COMMUNICATION_INITIALIZATION_ERROR,
            SERIAL_COMMUNICATION_NOT_INITIALIZED,
            SERIAL_COMMUNICATION_ERROR,
            DLL_IO_CARD_INCOMPATIBILITY
        }

        /// <summary>
        /// Keypad information
        /// </summary>
        public struct KaboomKeypad
        {
            public ushort un8UpButtonPressCount;
            public ushort un8DownButtonPressCount;
            public ushort un8MenuButtonPressCount;
            public ushort un8SelectButtonPressCount;
        }

		public struct KaboomWheelSpinnerParams
		{
			public byte un8Target;
			public byte un8Phase;
		}

        /// <summary>
        /// Flag to extract KaboomSteeringInfoFlags
        /// </summary>
        private enum KaboomSteeringInfoFlagsValues : uint
        {
            SteeringInitSucceed = 0x01,
            InitCompleted = 0x04,
            CrossOrigine = 0x04,
        }

        /// <summary>
        /// KaboomSteeringInfoFlags structure
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = sizeof(Int16), CharSet = CharSet.Unicode)]
        public struct KaboomSteeringInfoFlags
        {
            [FieldOffset(0)]
            private UInt16 bSteeringInitSucceed;	// TRUE: Steering init succeed, FALSE: Steering init FAILED 
            public bool SteeringInitSucceed
            {
                get
                {
                    return (bSteeringInitSucceed & (UInt16)KaboomSteeringInfoFlagsValues.SteeringInitSucceed) != 0;
                }
            }

            [FieldOffset(0)]
            private UInt16 bInitCompleted;			// TRUE: Init completed
            public bool InitCompleted
            {
                get
                {
                    return (bInitCompleted & (UInt16)KaboomSteeringInfoFlagsValues.InitCompleted) != 0;
                }
            }

            [FieldOffset(0)]
            private UInt16 bCrossOrigine;			// TRUE: if the steering has crossed the origine
            public bool CrossOrigine
            {
                get
                {
                    return (bCrossOrigine & (UInt16)KaboomSteeringInfoFlagsValues.CrossOrigine) != 0;
                }
            }
        }

        /// <summary>
        /// Steering information
        /// </summary>
        public struct KaboomSteeringInfo
        {
            public byte un8MaxRight;					// Pot value min
            public byte un8MaxLeft;						// Pot value max
            public byte un8Center;
            public byte bSteeringInitSucceed;			// TRUE: Steering init succeed, FALSE: Steering init FAILED 
            public byte bInitCompleted;
            public byte un8SteeringPotValue;
            public UInt16 un16Power;
        }

        /// <summary>
        /// Event mask for automatic callback from target
        /// </summary>
        public struct KaboomEventMaskInfo
        {
            public UInt16 un16EventMask;	                // ????? 

            public bool KeyboardEventMask
            {
                get
                {
                    return (un16EventMask & 0x0001) != 0;
                }
            }

            public bool CoinEventMask
            {
                get
                {
                    return (un16EventMask & 0x0002) != 0;
                }
            }

            public bool DropObjectEventMask
            {
                get
                {
                    return (un16EventMask & 0x0004) != 0;
                }
            }
        }

        /// <summary>
        /// Flag to extract KaboomReadInputFlagsValues
        /// </summary>
        public enum KaboomReadInputFlagsValues : uint
        {
            In1 = 0x0001,
            In2 = 0x0002,
            Opto1 = 0x0004,
            Opto2 = 0x0008,
            Opto3 = 0x0010,
            Opto4 = 0x0020,

            Trig = 0x0040,
            Boom = 0x0080,

            KeyEnter = 0x0100,
            KeyDown = 0x0200,
            KeySel = 0x0400,
            KeyUp = 0x0800
        }

        /// <summary>
        /// KaboomSteeringInfoFlags structure
        /// </summary>
        public struct KaboomReadInputFlags
        {
            #pragma warning disable 649
            private UInt16 allBits;	// TRUE: Steering init succeed, FALSE: Steering init FAILED 
            #pragma warning restore 649

            /// <summary>
            /// Test for a read input
            /// </summary>
            /// <param name="readInputFlag"></param>
            /// <returns></returns>
            public bool IsReadInputSet(KaboomReadInputFlagsValues readInputFlag)
            {
                return (allBits & ((UInt16)readInputFlag)) != 0;
            }

            /// <summary>
            /// Return the 16 bits input
            /// </summary>
            public UInt16 AllBits
            {
                get
                {
                    return allBits;
                }
            }
        }

        /// <summary>
        /// Delegate signature for a Kaboom Keyboard KeyPress event
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncKaboomKeyboardEvent(UInt16 un16KeyPressed);

        /// <summary>
        /// Delegate signature for a Kaboom Coin count change event
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncKaboomCoinEvent(UInt16 un16CoinCount1, UInt16 un16CoinCount2);

        /// <summary>
        /// Delegate signature for a Kaboom Object Detected event
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncKaboomObjectDetectedEvent(UInt16 un16Output, UInt16 un16PulseCnt);

        /// <summary>
        /// Delegate signature for a Kaboom Object Not Detected event
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncKaboomObjectNotDetectedEvent(UInt16 un16Output);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void FuncKaboomWheelSpinnerDoneEvent(byte un8RequestedTarget, byte un8RequestedPhase, byte un8Target, byte un8Phase);

        /// <summary>
        /// InitLib extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "InitLib", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR InitLib(
            int nSerialPort,
            [In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomKeyboardEvent pFctKeyboardCallBack,
            [In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomCoinEvent pFctCoinCountCallBack,
            [In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomObjectDetectedEvent pFctObjectDetectedCallBack,
            [In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomObjectNotDetectedEvent pFctObjectNotDetectedCallBack,
            [In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomWheelSpinnerDoneEvent pFctWheelSpinnerDoneCallBack);

		/// <summary>
		/// InitLib extern dll declaration
		/// For Linux only
		/// </summary>
		[DllImport(DLL_PATH, EntryPoint = "InitLib2Feeder", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern KABOOM_DLL_ERROR InitLib2Feeder(
			int nSerialPort,
			[In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomKeyboardEvent pFctKeyboardCallBack,
			[In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomCoinEvent pFctCoinCountCallBack,
			[In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomObjectDetectedEvent pFctObjectDetectedCallBack,
			[In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomObjectNotDetectedEvent pFctObjectNotDetectedCallBack,
			[In, MarshalAs(UnmanagedType.FunctionPtr)] FuncKaboomWheelSpinnerDoneEvent pFctWheelSpinnerDoneCallBack);

		/// <summary>
        /// CloseLib extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CloseLib", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseLib();

        /// <summary>
        /// GetKeypadCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetKeypadCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetKeypadCount(ref KaboomKeypad pKeypadCount);

        /// <summary>
        /// GetTicketFeederTicketToFeedCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetTicketFeederTicketToFeedCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetTicketFeederTicketToFeedCount(ref UInt16 pun16TicketToFeedCount1, ref UInt16 pun16TicketToFeedCount2);

        /// <summary>
        /// SetTicketFeederTicketToFeedCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetTicketFeederTicketToFeedCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetTicketFeederTicketToFeedCount(UInt16 un16TicketToFeedCount1, UInt16 un16TicketToFeedCount2);

        /// <summary>
        /// GetTicketFeederCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetTicketFeederCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetTicketFeederCount(ref UInt16 pun16FeederCount1, ref UInt16 pun16IsEmpty1, ref UInt16 pun16FeederCount2, ref UInt16 pun16IsEmpty2);

        /// <summary>
        /// ResetTicketFeeder extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "ResetTicketFeeder", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR ResetTicketFeeder();

        /// <summary>
        /// GetCurrentCoinCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetCurrentCoinCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetCurrentCoinCount(ref UInt16 pun16CoinCounter1, ref UInt16 pun16CoinCounter2);

        /// <summary>
        /// ResetCoinCount extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "ResetCoinCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR ResetCoinCount(ref UInt16 pun16CoinCounter1, ref UInt16 pun16CoinCounter2);

        /// <summary>
        /// InitSteeringKickStart extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "InitSteeringKickStart", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR InitSteeringKickStart();

        /// <summary>
        /// GetSteeringStatus extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetSteeringStatus", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetSteeringStatus(ref KaboomSteeringInfo pSteeringInfo);

        /// <summary>
        /// GetEventMask extern dll declaration 
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetEventMask", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetEventMask(ref KaboomEventMaskInfo pEventMask);

        /// <summary>
        /// SetEventMask extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetEventMask", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetEventMask(KaboomEventMaskInfo EventMask);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetSteeringParameters", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetSteeringParameters(byte un8Position, byte un8Power);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetSteeringUpdateRate", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetSteeringUpdateRate(UInt16 un16UpdateRateInMs);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetSteeringPosition", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte GetSteeringPosition();

        /// <summary>
        /// SetOutput extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetOutput", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetOutput(UInt16 un16Msk);

        /// <summary>
        /// ClearOutput extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "ClearOutput", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR ClearOutput(UInt16 un16Msk);

        /// <summary>
        /// SetExtendedOutput extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetExtendedOutput", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetExtendedOutput(UInt16 un16Msk);

        /// <summary>
        /// ClearExtendedOutput extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "ClearExtendedOutput", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR ClearExtendedOutput(UInt16 un16Msk);


        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "ReadInput", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR ReadInput(ref KaboomReadInputFlags pReadInput);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetDiagnosticState", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetDiagnosticState(byte bDiagnosticMode);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetIRDetectorCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR GetIRDetectorCount(UInt16 un16Output, ref UInt16 pun16PulseCnt);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "SetIRDetectorCount", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR SetIRDetectorCount(UInt16 un16Output, UInt16 un16PulseCnt);

        /// <summary>
        /// SetSteeringParameters extern dll declaration
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "StartIRDetector", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern KABOOM_DLL_ERROR StartIRDetector(UInt16 un16Output);

		/// <summary>
		/// StartWheelSpinner extern dll declaration
		/// </summary>
		[DllImport(DLL_PATH, EntryPoint = "StartWheelSpinner", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern KABOOM_DLL_ERROR StartWheelSpinner(KaboomWheelSpinnerParams oParams);

		/// <summary>
		/// SendLedData extern dll declaration
		/// </summary>
		//[DllImport(DLL_PATH, EntryPoint = "SendLedData", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		//public static extern KABOOM_DLL_ERROR SendLedData(UInt16 pun16Length,  byte* pbData);

		/// <summary>
		/// SetLedDisplay extern dll declaration
		/// </summary>
		[DllImport(DLL_PATH, EntryPoint = "SetLedDisplay", ExactSpelling = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern KABOOM_DLL_ERROR SetLedDisplay(UInt16 un16Value);
    }
}
