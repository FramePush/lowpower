using UnityEngine;
using UnityEngine.UI;

public class PowerStatusTester : MonoBehaviour
{
	public Toggle PowerSaverToggle;

	#region Overrides

	protected virtual void Start()
	{
		var isPowerSaverOn = PowerStatus.IsPowerSaverOn;
		PowerSaverToggle.isOn = isPowerSaverOn;
		Application.targetFrameRate = isPowerSaverOn ? 30 : 60;
	}

	protected virtual void Awake()
	{
		PowerStatus.PowerSaverChanged += PowerStatus_PowerSaverChanged;
	}

	protected virtual void OnDestroy()
	{
		PowerStatus.PowerSaverChanged -= PowerStatus_PowerSaverChanged;
	}

	#endregion

	void PowerStatus_PowerSaverChanged(bool obj)
	{
		PowerSaverToggle.isOn = obj;
		Application.targetFrameRate = obj ? 30 : 60;
	}
}
