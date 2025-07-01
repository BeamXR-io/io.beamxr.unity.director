using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamAudioControlElement : BeamManagerPlusMinusElement
    {
        [SerializeField]
        protected ToggleAnimator _enabledToggle;

        protected override void Awake()
        {
            base.Awake();
            if (_enabledToggle != null)
            {
                _enabledToggle.OnValueChanged += ToggleChange;
            }
        }

        private void ToggleChange(bool value)
        {
            Toggle(value);
            UpdateVisual();
        }

        protected abstract void Toggle(bool value);
    }
}