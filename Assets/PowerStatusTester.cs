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

		if (isPowerSaverOn) {
			Application.targetFrameRate = 30;
		} else {
			Application.targetFrameRate = 60;
		}
	}

	#endregion
}
