using System;
using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

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
			return _isLowPowerModeOn();
#else
			return false;
#endif
		}
	}

	public static event Action<bool> PowerSaverChanged
	{
		add
		{
			Instance.s_PowerSaverChanged += value;
		}
		remove
		{
			Instance.s_PowerSaverChanged -= value;
			if (Instance.s_PowerSaverChanged.GetInvocationList().Length == 0) {
				Destroy(Instance.gameObject);
			}
		}
	}

	Action<bool> s_PowerSaverChanged;

#if UNITY_EDITOR
#elif UNITY_IOS
	IntPtr m_NativeObject;
#endif

	#region Overrides

	protected virtual void Awake()
	{
		if (!s_Instance) {
			s_Instance = this;
		} else if (this != s_Instance) {
			Destroy(gameObject);
			return;
		}

		hideFlags = HideFlags.HideAndDontSave;

#if UNITY_EDITOR
#elif UNITY_IOS
		m_NativeObject = _createPowerStatus(name);
#endif
	}

	protected virtual void OnDestroy()
	{
#if UNITY_EDITOR
#elif UNITY_IOS
		_destroyPowerStatus(m_NativeObject);
#endif
	}

	#endregion

	#region Native Messages

	// ReSharper disable once UnusedMember.Local
	private void HandleSaverChange(string message)
	{
		bool state;
		if (!bool.TryParse(message, out state)) return;
		if (s_PowerSaverChanged != null) {
			s_PowerSaverChanged(state);
		}
	}

	#endregion

#if UNITY_IOS
	[DllImport("__Internal")]
	static extern bool _isLowPowerModeOn();

	[DllImport("__Internal")]
	static extern IntPtr _createPowerStatus(string targetName);

	[DllImport("__Internal")]
	static extern void _destroyPowerStatus(IntPtr powerStatus);
#endif
}
