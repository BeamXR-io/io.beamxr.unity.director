using BeamXR.Director.ControlPanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class PresetButton : BeamButton
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private bool _savePreset = false;

        [SerializeField]
        private int _preset = 0;

        private BeamControlPanel _controlPanel;

        protected override void FindParts()
        {
            base.FindParts();
            if(_controlPanel == null)
            {
                _controlPanel = FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (_preset == -1)
            {
                _text.text = "";
            }
            else
            {
                _text.text = (_preset + 1).ToString();
            }
            Bind(ApplyPreset);
        }

        private void OnEnable()
        {
            if (_preset == -1)
            {
                ChangeDefaultVisible();
            }
            else
            {
                ChangeVisible(_beamCamera.PresetAvailable(_preset));
            }
        }

        private void ChangeDefaultVisible()
        {
            _button.Selectable.interactable = !_savePreset;
        }

        private void ChangeVisible(bool visible)
        {
            if (_savePreset)
            {
                _button.Selectable.interactable = true;
                _image.gameObject.SetActive(true);
            }
            else
            {
                _button.Selectable.interactable = visible;
                _image.gameObject.SetActive(visible);
            }   
            _text.gameObject.SetActive(visible);
        }

        private void ApplyPreset()
        {
            if (_savePreset)
            {
                if (_preset != -1)
                {
                    _beamCamera.SaveCurrentCamera(_preset);
                    _controlPanel.Toasts.SendToast($"Preset {_preset + 1} saved");
                }
            }
            else
            {
                if (_preset == -1)
                {
                    _beamCamera.SetToDefaultCamera();
                }
                else
                {
                    _beamCamera.LoadPreset(_preset);
                }
            }
        }
    }
}