using UnityEngine;
using UnityEngine.Events;
using BeamXR.Streaming.Core;

namespace BeamXR.Director.ControlPanel
{
    public class BeamControlPanel : BeamComponent
    {
        [SerializeField]
        private BeamCorePanel _corePanel;

        [SerializeField]
        private BeamSettingsPanel _settingsPanel;

        [SerializeField]
        private BeamAccountPanel _accountPanel;

        [SerializeField]
        private BeamToaster _toasts;
        public BeamToaster Toasts => _toasts;

        [Header("Positioning"), SerializeField]
        private float _distanceFromPlayer = 0.5f;
        [SerializeField]
        private float _heightFromPlayer = -0.5f;

        public const string LiveColorHex = "#58D779";
        public const string RecordingColorHex = "#E55153";

        [SerializeField]
        private ButtonAnimator _settingsButton;
        private bool _settingsOpen = false;

        public UnityEvent<bool> OnControlPanelVisible;

        private Transform _cameraTransform;

        private StreamingState _oldStreamState;
        private RecordingState _oldRecordState;

        protected override void FindParts()
        {
            base.FindParts();
            if (_corePanel == null)
            {
                _corePanel = GetComponentInChildren<BeamCorePanel>(true);
            }
            if (_settingsPanel == null)
            {
                _settingsPanel = GetComponentInChildren<BeamSettingsPanel>(true);
            }
            if (_accountPanel == null)
            {
                _accountPanel = GetComponentInChildren<BeamAccountPanel>(true);
            }
            if (_toasts == null)
            {
                _toasts = GetComponent<BeamToaster>();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            FindParts();
            _cameraTransform = UnityEngine.Camera.main.transform;
            _settingsPanel.gameObject.SetActive(false);
            if (_settingsButton != null)
            {
                _settingsButton.OnClick += () =>
                {
                    _settingsOpen = !_settingsOpen;
                    _settingsButton.ForcePressed(_settingsOpen);
                    _settingsPanel.gameObject.SetActive(_settingsOpen);
                };
            }
        }

        private void OnEnable()
        {
            _corePanel.gameObject.SetActive(true);

            OpenPanelLogic(true);

            OnControlPanelVisible?.Invoke(true);
        }

        private void OnDisable()
        {
            if (_corePanel.transform.parent != transform)
            {
                _corePanel.transform.SetParent(transform, true);
            }
            if (_settingsPanel.transform.parent != transform)
            {
                _settingsPanel.transform.SetParent(transform, true);
            }

            OnControlPanelVisible?.Invoke(false);
        }

        public void ToggleControlPanel()
        {
            OpenPanelLogic(!_corePanel.gameObject.activeInHierarchy);
            _corePanel.gameObject.SetActive(!_corePanel.gameObject.activeInHierarchy);
            OnControlPanelVisible?.Invoke(_corePanel.gameObject.activeInHierarchy);
        }

        private void OpenPanelLogic(bool open)
        {
            
            if (open)
            {
                if (_cameraTransform == null)
                {
                    _cameraTransform = UnityEngine.Camera.main.transform;
                }

                if (_cameraTransform != null)
                {
                    Vector3 pos = _cameraTransform.position + (_cameraTransform.forward * _distanceFromPlayer);
                    pos.y = (_cameraTransform.position.y + _heightFromPlayer);
                    _corePanel.transform.position = pos;
                    _corePanel.transform.rotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
                }

                if (_corePanel.transform.parent != transform)
                {
                    _corePanel.transform.SetParent(transform, true);
                }
                if (_settingsPanel.transform.parent != transform)
                {
                    _settingsPanel.transform.SetParent(transform, true);
                }

                if (_settingsOpen)
                {
                    _settingsPanel.gameObject.SetActive(true);
                    _settingsButton.ForcePressed(_settingsOpen);
                }
            }
            else
            {
                _settingsPanel.gameObject.SetActive(false);
            }
        }

        public void OpenUserAccount()
        {
            if (!_settingsOpen)
            {
                _settingsPanel.gameObject.SetActive(true);
                _settingsButton.ForcePressed(true);
                _settingsOpen = true;
            }
            _settingsPanel.OpenPanel(_accountPanel.gameObject);
        }
    }
}