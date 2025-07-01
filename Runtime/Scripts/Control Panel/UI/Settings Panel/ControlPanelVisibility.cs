using BeamXR.Streaming.Core.Gameplay;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class ControlPanelVisibility : MonoBehaviour
    {
        [SerializeField]
        private ToggleAnimator _toggle;

        private BeamControlPanel _controlPanel;
        private BeamObjectHider _objectHider;

        private void Awake()
        {
            FindParts();
            Setup();
        }

        private void OnValidate()
        {
            FindParts();
        }

        private void FindParts()
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<ToggleAnimator>();
            }
            if (Application.isPlaying)
            {
                _controlPanel = FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include);
                _objectHider = _controlPanel.GetComponent<BeamObjectHider>();
            }
        }

        private void Setup()
        {
            if(_objectHider != null)
            {
                _toggle.OnValueChanged += TogglePanel;
            }
        }

        private void TogglePanel(bool value)
        {
            if (value)
            {
                _objectHider.ChangeHideFromCamera(BeamObjectHider.HideType.Selfie);
            }
            else
            {
                _objectHider.ChangeHideFromCamera(BeamObjectHider.Everything);
            }
        }

        private void OnEnable()
        {
            FindParts();
            if (_objectHider.CurrentHideType == BeamObjectHider.HideType.None || _objectHider.CurrentHideType == BeamObjectHider.HideType.Selfie)
            {
                _toggle.SetToggle(true, true);
            }
            else
            {
                _toggle.SetToggle(false, true);
            }

        }
    }
}