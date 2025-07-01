using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamPlusMinusElement : BeamCameraControlElement
    {
        [SerializeField]
        protected Button _minus, _plus;

        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        protected override void Awake()
        {
            base.Awake();
            _minus.onClick.AddListener(Minus);
            _plus.onClick.AddListener(Plus);
        }

        protected abstract void Minus();

        protected abstract void Plus();

        protected override void UpdateSettings()
        {
            if(_text != null)
            {
                _text.text = UpdateText();
            }
        }

        protected abstract string UpdateText();
    }
}