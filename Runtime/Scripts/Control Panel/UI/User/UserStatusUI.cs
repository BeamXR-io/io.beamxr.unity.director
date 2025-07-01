using BeamXR.Streaming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class UserStatusUI : BeamComponent
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _text;

        private Color _good, _bad;

        protected override void Awake()
        {
            base.Awake();
            ColorUtility.TryParseHtmlString(BeamControlPanel.LiveColorHex, out _good);
            ColorUtility.TryParseHtmlString(BeamControlPanel.RecordingColorHex, out _bad);
        }

        private void OnEnable()
        {
            UpdateState(_beamManager.AuthState);
            _unityEvents.OnAuthenticationChanged.AddListener(UpdateState);
        }

        private void OnDisable()
        {
            _unityEvents.OnAuthenticationChanged.RemoveListener(UpdateState);
        }

        private void UpdateState(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.NotAuthenticated:
                case AuthenticationState.Error:
                case AuthenticationState.Authenticating:
                    _image.color = _bad;
                    _text.text = "!";
                    break;
                case AuthenticationState.Authenticated:
                    _image.color = _good;
                    _text.text = "";
                    break;
            }
        }

        protected override void FindParts()
        {
            base.FindParts();
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            if (_text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }
}