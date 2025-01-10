using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamAudioControlElement : BeamManagerPlusMinusElement
    {
        [SerializeField]
        protected Toggle _enabledToggle;

        protected override void Awake()
        {
            base.Awake();
            _enabledToggle.onValueChanged.AddListener(ToggleChange);
        }

        private void ToggleChange(bool value)
        {
            Toggle(value);
            UpdateVisual();
        }

        protected abstract void Toggle(bool value);
    }
}