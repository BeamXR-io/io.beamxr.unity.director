using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class PresetSelection : BeamButton
    {
        [SerializeField]
        private bool _closePanelOnSelect = true;

        [SerializeField]
        private GameObject _selectionPanel = null;

        [SerializeField]
        private List<ButtonAnimator> _presetButtons = new List<ButtonAnimator>();

        [SerializeField]
        private TextMeshProUGUI _presetText = null;

        [SerializeField]
        private Image _defaultPresetImage = null;

        protected override void Awake()
        {
            base.Awake();
            Bind(TogglePanel);
            if (_closePanelOnSelect)
            {
                for (int i = 0; i < _presetButtons.Count; i++)
                {
                    int j = i;
                    _presetButtons[i].OnClick += () => { ClosePanel(j); } ;
                }
            }
        }

        private void TogglePanel()
        {
            _button.ForcePressed(!_selectionPanel.activeInHierarchy);
            _selectionPanel.SetActive(!_selectionPanel.activeInHierarchy);
        }

        private void ClosePanel(int preset)
        {
            _selectionPanel.SetActive(false);
            _button.ForcePressed(false);
            if(preset == 0)
            {
                _defaultPresetImage.gameObject.SetActive(true);
                _presetText.text = "";
            }
            else
            {
                _defaultPresetImage.gameObject.SetActive(false);
                _presetText.text = preset.ToString();
            }
        }
    }
}