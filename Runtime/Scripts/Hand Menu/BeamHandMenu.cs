using System.Collections.Generic;
using BeamXR.Streaming.Core;
using UnityEngine;
using UnityEngine.Events;

namespace BeamXR.Director.HandMenu
{
    public class BeamHandMenu : MonoBehaviour
    {
        [System.Serializable]
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Backward
        }

        [System.Serializable]
        public class TransformDirection
        {
            public Transform transform;
            public Direction direction;
            
            public Vector3 GetDirection()
            {
                switch (direction)
                {
                    case Direction.Up:
                        return transform.up;
                    case Direction.Down:
                        return -transform.up;
                    case Direction.Left:
                        return -transform.right;
                    case Direction.Right:
                        return transform.right;
                    case Direction.Forward:
                        return transform.forward;
                    case Direction.Backward:
                        return -transform.forward;
                    default:
                        return Vector3.zero;
                }
            }
        }

        [SerializeField]
        private List<TransformDirection> _wristTransforms = new List<TransformDirection>();
        private TransformDirection _currentTransform = null;

        [SerializeField]
        private float _wristToCameraAngle = 45.0f, _cameraToWristAngle = 45.0f;
        [SerializeField]
        private bool _followActiveTransform = true;

        private Vector3 _wristToCamDir, _camToWristDir;
        private Vector3 _wristNormal, _camNormal;
        private float _wristAngle, _camAngle;

        private bool _currentState = false;
        public bool CurrentState => _currentState;
        public UnityEvent<bool> OnMenuStateChanged;

        [SerializeField]

        private void Start()
        {
            Transform child = transform.GetChild(0);
            if (child == null)
            {
                BeamLogger.LogError("Hand Menu is missing the menu object and will not be activated.");
                enabled = false;
                return;
            }
            OnMenuStateChanged?.Invoke(false);
        }

        private void Update()
        {
            if (_followActiveTransform && _currentState)
            {
                FollowTransform();
            }
        }

        void FixedUpdate()
        {
            if (_wristTransforms.Count == 0)
                return;

            bool visible = false;
            
            if(_currentTransform != null && _currentTransform.transform != null)
            {
                visible = MenuVisible(_currentTransform);
                if (!visible)
                {
                    _currentTransform = null;
                }
            }

            if (_currentTransform == null || _currentTransform.transform == null)
            {
                for (int i = 0; i < _wristTransforms.Count; i++)
                {
                    visible = MenuVisible(_wristTransforms[i]);
                    if(visible)
                    {
                        _currentTransform = _wristTransforms[i];
                        break;
                    }    
                }
            }

            ChangeMenuState(visible);
        }

        private bool MenuVisible(TransformDirection transform)
        {
            _wristToCamDir = (UnityEngine.Camera.main.transform.position - transform.transform.position).normalized;
            _camToWristDir = (transform.transform.position - UnityEngine.Camera.main.transform.position).normalized;

            _wristNormal = transform.GetDirection();
            _camNormal = UnityEngine.Camera.main.transform.forward;

            _wristAngle = Vector3.Angle(_wristNormal, _wristToCamDir);
            _camAngle = Vector3.Angle(_camNormal, _camToWristDir);

            return _wristAngle <= _wristToCameraAngle && _camAngle <= _cameraToWristAngle;
        }

        private void ChangeMenuState(bool active)
        {
            if(active != _currentState)
            {
                _currentState = active;
                OnMenuStateChanged?.Invoke(active);
            }
        }

        private void FollowTransform()
        {
            if (_currentTransform != null && _currentTransform.transform != null)
            {
                transform.position = _currentTransform.transform.position;
                transform.rotation = _currentTransform.transform.rotation;
            }
        }
    }
}