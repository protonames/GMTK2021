using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK
{
    public class HyperlinkButton : MonoBehaviour
    {
        [SerializeField]
        private string url = "";

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
        }
    }
}