using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	public Text Text;
	public float UpdateInterval = 1;

	int m_FrameCount;
	float m_LastUpdate;

	#region Overrides

	protected virtual void Update()
	{
		var currentTimestamp = Time.realtimeSinceStartup;
		var expectedUpdate = m_LastUpdate + UpdateInterval;

		if (expectedUpdate < currentTimestamp) {
			Text.text = "FPS: " + m_FrameCount / UpdateInterval;
			m_FrameCount = 0;
			m_LastUpdate = expectedUpdate;
		}

		++m_FrameCount;
	}

	#endregion
}
