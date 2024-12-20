using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BeamXR.Streaming;

namespace BeamXR.Director.ControlPanel
{
    public class BeamLoginFlow : BeamComponent
    {        
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Button _button;
        [SerializeField]
        private TextMeshProUGUI _buttonText;

        [SerializeField]
        private Button _closeButton;

        [SerializeField, HideInInspector]
        private BeamControlPanel _controlPanel;

        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(ClickButton);
            if(_controlPanel == null)
            {
                _controlPanel = GetComponentInParent<BeamControlPanel>();
            }
            if(_controlPanel != null)
            {
                _closeButton.onClick.AddListener(CloseButtonAction);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_controlPanel == null)
            {
                _controlPanel = GetComponentInParent<BeamControlPanel>();
            }
        }

        private void OnEnable()
        {
            _unityEvents.OnAuthenticationChanged.AddListener(AuthenticationChanged);
            AuthenticationChanged(_streamingManager.AuthState);

            _closeButton.gameObject.SetActive(_streamingManager.AuthState == AuthenticationState.Authenticated);
        }

        private void OnDisable()
        {
            _unityEvents.OnAuthenticationChanged.RemoveListener(AuthenticationChanged);
        }

        private void AuthenticationChanged(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.NotAuthenticated:
                case AuthenticationState.Error:
                    SetLogin();
                    break;
                case AuthenticationState.Authenticating:
                    SetAuthenticating();
                    break;
                case AuthenticationState.Authenticated:
                    SetAuthenticated();
                    break;
            }
        }

        private void ClickButton()
        {
            switch (_streamingManager.AuthState)
            {
                case AuthenticationState.NotAuthenticated:
                case AuthenticationState.Error:
                    _streamingManager.Authenticate();
                    break;
                case AuthenticationState.Authenticating:
                    Application.OpenURL(_streamingManager.DeviceFlowCode.VerificationUrlComplete);
                    break;
                case AuthenticationState.Authenticated:
                    _streamingManager.SignOut();
                    _closeButton.gameObject.SetActive(false);
                    break;
            }
        }

        private void SetLogin()
        {
            _text.text = "Please login to BeamXR to start streaming and recording";

            _buttonText.text = "Login";
            _button.gameObject.SetActive(true);
        }

        private void SetAuthenticating()
        {
            if(_streamingManager.DeviceFlowCode != null)
            {
                _text.text = $"Please visit\n<color=#4B89C5><b>{_streamingManager.DeviceFlowCode.VerificationUrl}</b></color>\nand enter the code\n<color=#4B89C5><b>{_streamingManager.DeviceFlowCode.UserCode}</b></color>";
                _buttonText.text = "Open Browser";
                _button.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(CheckDeviceFlow());
                _text.text = "Authenticating BeamXR";
                _button.gameObject.SetActive(false);
            }
        }

        private IEnumerator CheckDeviceFlow()
        {
            WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.1f);
            for (; ;)
            {
                if (_streamingManager.DeviceFlowCode == null)
                {
                    yield return waitTime;
                }
                else
                {
                    SetAuthenticating();
                    break;
                }
            }
        }

        private void SetAuthenticated()
        {
            _button.gameObject.SetActive(_streamingManager.StreamingState == Streaming.Core.StreamingState.Disconnected);
            _buttonText.text = "Logout";
            _text.text = $"Logged in to BeamXR as\n<color=#4B89C5><b>{_streamingManager.Me.nickname}</b></color>";
            if(_streamingManager.StreamingState != Streaming.Core.StreamingState.Disconnected)
            {
                _text.text += "\nPlease stop streaming to logout";
            }
        }

        private void CloseButtonAction()
        {
            _controlPanel.SetControlsState(true);
        }
    }
}