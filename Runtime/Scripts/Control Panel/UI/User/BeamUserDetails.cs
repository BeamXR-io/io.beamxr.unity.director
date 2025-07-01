using BeamXR.Streaming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class BeamUserDetails : BeamComponent
    {
        [SerializeField]
        private ButtonAnimator _button;

        [SerializeField]
        private TextMeshProUGUI _handle;

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponentInChildren<ButtonAnimator>();
            if (_button != null)
            {
                _button.OnClick += SignOut;
                if(ColorUtility.TryParseHtmlString(BeamControlPanel.RecordingColorHex, out var color))
                {
                    _button.SetAlternativeColor(color, color);
                }
            }
        }

        private void OnEnable()
        {
            _unityEvents.OnAuthenticationChanged.AddListener(AuthStateChanged);
            AuthStateChanged(_beamManager.AuthState);
        }

        private void OnDisable()
        {
            _unityEvents.OnAuthenticationChanged.RemoveListener(AuthStateChanged);
        }

        private void AuthStateChanged(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.NotAuthenticated:
                    break;
                case AuthenticationState.Authenticating:
                    break;
                case AuthenticationState.Authenticated:
                    string text = "Logged in as:\n<font-weight=700>";
                    if (_beamManager.Me.handle == "")
                    {
                        text += _beamManager.Me.email;
                    }
                    else
                    {
                        text += "@" + _beamManager.Me.handle;
                    }
                    _handle.text = text + "</font-weight>";
                    break;
                case AuthenticationState.Error:
                    break;
            }
        }

        private void SignOut()
        {
            _beamManager.SignOut();
        }
    }
}