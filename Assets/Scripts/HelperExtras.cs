using System.Collections;
using Cinemachine;
using DG.Tweening;
using PNLib.Time;
using PNLib.Utility;
using UnityEngine;

namespace PNTemplate
{
	public static class HelperExtras
	{
		private static CinemachineImpulseSource shakeSource;

		public static bool IsInsideCameraViewport(Vector3 position)
		{
			Vector3 viewportPoint = Helper.Camera.WorldToViewportPoint(position);
			return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
		}

		public static void Slow(float scale, float duration)
		{
			GameObject holder = new GameObject("holder");
			DestroyAfterSeconds destroy = holder.AddComponent<DestroyAfterSeconds>();
			destroy.Seconds = duration + .1f;
			destroy.StartCoroutine(FreezeTimeRoutine(scale, duration));
		}

		private static IEnumerator FreezeTimeRoutine(float scale, float duration)
		{
			TimeManager.SetTimeScale(scale);
			yield return new WaitForSecondsRealtime(duration);

			TimeManager.ResetTimeScale();
		}

		public static void Shake(float amplitude, float frequency, float duration)
		{
			if (shakeSource == null)
			{
				shakeSource = Helper.Camera.GetComponent<CinemachineImpulseSource>();
				CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
			}

			shakeSource.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
			shakeSource.m_ImpulseDefinition.m_FrequencyGain = frequency;
			shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration;
			shakeSource.GenerateImpulse();
		}

		public static void Flash(float seconds)
		{
			Color startColor = Helper.Camera.backgroundColor;
			Helper.Camera.DOColor(startColor, seconds).From(Color.white).SetUpdate(true);
		}

		public static Vector2 GetNormalizedCircularPosition(float angle)
		{
			float radians = angle * Mathf.Deg2Rad;
			float x = Mathf.Cos(radians);
			float y = Mathf.Sin(radians);
			return new Vector2(x, y);
		}
	}
}