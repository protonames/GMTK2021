using DG.Tweening;
using UnityEngine;

namespace GMTK
{
	public class TweenFloaty : MonoBehaviour
	{
		[SerializeField]
		private float duration = .2f;

		[SerializeField]
		private Ease ease;

		[SerializeField]
		private float floatAmount = .25f;

		private void Start()
		{
			transform.DOLocalMoveY(transform.localPosition.y + floatAmount, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
		}
	}
}