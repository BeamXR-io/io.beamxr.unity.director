using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamManagerPlusMinusElement : MonoBehaviour
    {
        [SerializeField]
        protected Button _minus, _plus;

        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        protected virtual void Awake()
        {
            _minus.onClick.AddListener(MinusWrapper);
            _plus.onClick.AddListener(PlusWrapper);
        }

        private void OnEnable()
        {
            UpdateVisual();
        }

        private void MinusWrapper()
        {
            Minus();
            UpdateVisual();
        }

        private void PlusWrapper()
        {
            Plus();
            UpdateVisual();
        }

        protected abstract void Minus();

        protected abstract void Plus();

        protected abstract void UpdateVisual();

        protected void UpdateText(string text)
        {
            _text.text = text;
        }
    }
}