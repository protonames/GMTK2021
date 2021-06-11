using DG.Tweening;
using UnityEngine;

namespace PNTemplate._Temp
{
	public class Muzzle : MonoBehaviour
	{
		private void Start()
		{
			transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InOutCubic).OnComplete(() => Destroy(gameObject));
		}
	}
}