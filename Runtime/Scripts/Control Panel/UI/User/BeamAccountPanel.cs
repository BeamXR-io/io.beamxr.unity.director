using System.Collections;
using UnityEngine;
using BeamXR.Streaming;

namespace BeamXR.Director.ControlPanel
{
    public class BeamAccountPanel : BeamComponent
    {
        [SerializeField]
        private GameObject _loadingPanel;

        [SerializeField]
        private DeviceCodeUI _deviceCode;

        [SerializeField]
        private GameObject _loginPanel;

        private IEnumerator _deviceFlowCode;

        [SerializeField]
        private GameObject _userPanel;

        private void OnEnable()
        {
            OnAuthChanged(_beamManager.AuthState);
            _unityEvents.OnAuthenticationChanged.AddListener(OnAuthChanged);
        }

        private void OnDisable()
        {
            _unityEvents.OnAuthenticationChanged.RemoveListener(OnAuthChanged);
        }

        private void OnAuthChanged(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.Error:
                case AuthenticationState.NotAuthenticated:
                    _beamManager.Authenticate();
                    SetPanelState(0);
                    break;
                case AuthenticationState.Loading:
                    SetPanelState(0);
                    break;
                case AuthenticationState.Authenticating:
                    _deviceCode.SetCode(_beamManager.DeviceFlowCode.UserCode);
                    SetPanelState(1);
                    break;
                case AuthenticationState.Authenticated:
                    SetPanelState(2);
                    break;
            }
        }

        private void SetPanelState(int state)
        {
            switch (state)
            {
                case 0: // idle
                    _loadingPanel.SetActive(true);
                    _loginPanel.SetActive(false);
                    _userPanel.SetActive(false);
                    break;
                case 1: // authenticating
                    _loadingPanel.SetActive(false);
                    _loginPanel.SetActive(true);
                    _userPanel.SetActive(false);
                    break;
                case 2: // logged in
                    _loadingPanel.SetActive(false);
                    _loginPanel.SetActive(false);
                    _userPanel.SetActive(true);
                    break;
            }
        }

        public void OpenDeviceURL()
        {
            if (_beamManager.DeviceFlowCode != null)
            {
                Application.OpenURL(_beamManager.DeviceFlowCode.VerificationUrlComplete);
            }
        }

        public void OpenPortal()
        {
            Application.OpenURL(_beamManager.GetEnvironment().GetViewingBaseUrl());
        }
    }
}