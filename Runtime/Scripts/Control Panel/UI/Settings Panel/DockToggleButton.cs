using BeamXR.Streaming.Core.Media;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class DockToggleButton : BeamComponent
    {
        [SerializeField]
        private BeamSettingsPanel _settingsPanel;

        [SerializeField]
        private ButtonAnimator _button;

        [SerializeField]
        private Image _dockImage;

        [SerializeField]
        private Sprite _dockedSprite, _undockedSprite;

        protected override void FindParts()
        {
            base.FindParts();
            if(_settingsPanel == null)
            {
                _settingsPanel = GetComponentInParent<BeamSettingsPanel>(true);
            }

            if(_button == null)
            {
                _button = GetComponent<ButtonAnimator>();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _button.OnClick += _settingsPanel.ToggleDock;
            _settingsPanel.OnUndockStateChanged.AddListener(ToggleDock);
            ToggleDock(_settingsPanel.Undocked);
        }

        private void OnEnable()
        {
            _unityEvents.OnAspectRatioChanged.AddListener(OnAspectRatioChanged);
            OnAspectRatioChanged(_beamManager.AspectRatio);
        }

        private void OnDisable()
        {
            _unityEvents.OnAspectRatioChanged.RemoveListener(OnAspectRatioChanged);
        }

        private void ToggleDock(bool undockStatus)
        {
            if (undockStatus)
            {
                _button.ForcePressed(true);
                _dockImage.sprite = _dockedSprite;
            }
            else
            {
                _button.ForcePressed(false);
                _dockImage.sprite = _undockedSprite;
            }
        }

        private void OnAspectRatioChanged(BeamAspectRatio aspectRatio)
        {
            switch (aspectRatio)
            {
                case BeamAspectRatio.Landscape:
                    _dockImage.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case BeamAspectRatio.Portrait:
                    _dockImage.transform.localRotation = Quaternion.identity;
                    break;
            }
        }
    }
}