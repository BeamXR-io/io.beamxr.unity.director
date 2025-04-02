using UnityEngine;
using BeamXR.Streaming.Core.Media;
using BeamXR.Streaming;
using UnityEngine.Events;

namespace BeamXR.Director.ControlPanel
{
    public class BeamControlPanel : BeamComponent
    {
        [SerializeField]
        private GameObject _controlsParent;

        [SerializeField]
        private BeamLoginFlow _loginFlow;


        [Header("Camera Control"), SerializeField]
        private float _distanceFromPlayer = 0.5f, _heightFromPlayer = -0.5f;

        public UnityEvent<bool> OnControlPanelVisible;

        private Transform _cameraTransform;

        protected override void OnValidate()
        {
            base.OnValidate();
            FindParts();
        }

        protected override void Awake()
        {
            base.Awake();
            FindParts();
            _unityEvents.OnAuthenticationChanged.AddListener(AuthenticationChanged);
            AuthenticationChanged(_streamingManager.AuthState);
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        private void FindParts()
        {
            if(_loginFlow == null)
            {
                _loginFlow = GetComponentInChildren<BeamLoginFlow>(true);
            }
        }

        private void OnEnable()
        {
            SetControlsState(_streamingManager.AuthState == AuthenticationState.Authenticated);
            
            if(_cameraTransform == null)
            {
                _cameraTransform = UnityEngine.Camera.main.transform;
            }
            
            if(_cameraTransform != null)
            {
                Vector3 pos = _cameraTransform.position + (_cameraTransform.forward * _distanceFromPlayer);
                pos.y = (_cameraTransform.position.y + _heightFromPlayer);
                transform.position = pos;
                transform.rotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);
            }

            OnControlPanelVisible?.Invoke(true);
        }

        private void OnDisable()
        {
            OnControlPanelVisible?.Invoke(false);
        }

        public void SetControlsState(bool controls)
        {
            _loginFlow.gameObject.SetActive(!controls);
            _controlsParent.gameObject.SetActive(controls);
        }

        private void AuthenticationChanged(AuthenticationState state)
        {
            switch (state)
            {
                case AuthenticationState.Error:
                case AuthenticationState.NotAuthenticated:
                case AuthenticationState.Authenticating:
                    SetControlsState(false);
                    break;
                case AuthenticationState.Authenticated:
                    SetControlsState(true);
                    break;
            }
        }

        public void ToggleControlPanel()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}