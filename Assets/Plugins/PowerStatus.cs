#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public static class PowerStatus
{
	public static bool IsPowerSaverOn
	{
		get
		{
#if UNITY_EDITOR_OSX
			return false;
#elif UNITY_EDITOR_WIN
			// TODO Do Windows 10 Battery Saver checks
			return false;
#elif UNITY_IOS
			return _isLowPowerModeOn();
#else
			return false;
#endif
		}
	}
#if UNITY_IOS
	[DllImport("__Internal")]
	static extern bool _isLowPowerModeOn();
#endif
}
