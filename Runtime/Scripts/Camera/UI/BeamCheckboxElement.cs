using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamCheckboxElement : BeamCameraControlElement
    {
        [SerializeField]
        protected Toggle _checkbox;

        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        protected override void Awake()
        {
            base.Awake();
            _checkbox.onValueChanged.AddListener(Checkbox);
        }

        protected override void UpdateSettings()
        {
            UpdateTextMesh();
        }

        private void UpdateTextMesh()
        {
            if (_text != null)
            {
                _text.text = UpdateText();
            }
        }

        protected abstract string UpdateText();

        protected abstract void Checkbox(bool value);

    }
}