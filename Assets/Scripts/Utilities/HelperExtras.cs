using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PNLib.Time;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.Utilities
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
		
		public static bool GetClosestObjectInCircleRadius<T>(Vector3 origin, float radius, out T current, LayerMask layerMask)
		{
			Collider2D[] targets = Physics2D.OverlapCircleAll(origin, radius, layerMask);
			current = default(T);
			float maxDistance = float.MaxValue;

			for (int index = 0; index < targets.Length; index++)
			{
				Collider2D hitTarget = targets[index];
				T target = hitTarget.GetComponent<T>();

				if (Equals(target, default(T)))
				{
					continue;
				}

				float currentDistance = Vector3.Distance(origin, hitTarget.transform.position);

				if (currentDistance < maxDistance)
				{
					current = target;
					maxDistance = currentDistance;
				}
			}

			return !Equals(current, default(T));
		}
		
		public static bool GetAllObjectsInCircleRadius<T>(Vector3 origin, float radius, out List<T> currentList, LayerMask layerMask)
		{
			Collider2D[] targets = Physics2D.OverlapCircleAll(origin, radius, layerMask);
			currentList = new List<T>();

			for (int index = 0; index < targets.Length; index++)
			{
				Collider2D hitTarget = targets[index];
				T target = hitTarget.GetComponent<T>();

				if (Equals(target, default(T)))
				{
					continue;
				}

				currentList.Add(target);
			}

			return currentList.Count > 0;
		}
		
		
		public static bool GetClosestObjectInList(Vector3 origin, List<Transform> list, out Transform current)
		{
			current = default(Transform);
			float maxDistance = float.MaxValue;

			for (int index = 0; index < list.Count; index++)
			{
				var target = list[index];
				
				if (Equals(target, default(Transform)))
				{
					continue;
				}

				float currentDistance = Vector3.Distance(origin, target.position);

				if (currentDistance < maxDistance)
				{
					current = target;
					maxDistance = currentDistance;
				}
			}

			return !Equals(current, default(Transform));
		}
	}
}