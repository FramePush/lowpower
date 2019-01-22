using System;
using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace LowPower
{
	public class PowerStatus : MonoBehaviour
	{
		static PowerStatus s_Instance;

		private static PowerStatus Instance
		{
			get
			{
				if (s_Instance)
					return s_Instance;

				s_Instance = FindObjectOfType<PowerStatus>();

				if (s_Instance)
					return s_Instance;

				s_Instance = new GameObject("PowerStatus").AddComponent<PowerStatus>();

				return s_Instance;
			}
		}

		public static bool IsPowerSaverOn
		{
			get
			{
#if UNITY_EDITOR
				return false;
#elif UNITY_IOS
				return _CFPisLowPowerModeOn();
#elif UNITY_ANDROID
				using (AndroidJavaClass ps = new AndroidJavaClass("com.framepush.lowpower.PowerStatus")) {
					return ps.CallStatic<bool>("isPowerSaveModeOn");
				}
#else
				return false;
#endif
			}
		}

		public static event Action<bool> PowerSaverChanged
		{
			add { Instance.s_PowerSaverChanged += value; }
			remove
			{
				Instance.s_PowerSaverChanged -= value;
				if (Instance.s_PowerSaverChanged.GetInvocationList().Length == 0)
				{
					Destroy(Instance.gameObject);
				}
			}
		}

		Action<bool> s_PowerSaverChanged;

#if UNITY_EDITOR
#elif UNITY_IOS
		IntPtr m_NativeObject;
#elif UNITY_ANDROID
		AndroidJavaObject m_JavaObject;
#endif

		#region Overrides

		private void Awake()
		{
			if (!s_Instance)
			{
				s_Instance = this;
			}
			else if (this != s_Instance)
			{
				Destroy(gameObject);
				return;
			}

			hideFlags = HideFlags.HideAndDontSave;

#if UNITY_EDITOR
#elif UNITY_IOS
			m_NativeObject = _CFPcreatePowerStatus(name);
#elif UNITY_ANDROID
			m_JavaObject = new AndroidJavaObject("com.framepush.lowpower.PowerStatus", name);
#endif
		}

		private void OnDestroy()
		{
#if UNITY_EDITOR
#elif UNITY_IOS
			_CFPdestroyPowerStatus(m_NativeObject);
#elif UNITY_ANDROID
			m_JavaObject.Call("close");
			m_JavaObject.Dispose();
#endif
		}

		#endregion

		#region Native Messages

		// ReSharper disable once UnusedMember.Local
		private void HandleSaverChange(string message)
		{
			bool state;
			if (!bool.TryParse(message, out state)) return;
			if (s_PowerSaverChanged != null)
			{
				s_PowerSaverChanged(state);
			}
		}

		#endregion

#if UNITY_IOS
		[DllImport("__Internal")]
		static extern bool _CFPisLowPowerModeOn();

		[DllImport("__Internal")]
		static extern IntPtr _CFPcreatePowerStatus(string targetName);

		[DllImport("__Internal")]
		static extern void _CFPdestroyPowerStatus(IntPtr powerStatus);
#endif
	}
}