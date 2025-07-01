using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BeamXR.Director.ControlPanel
{
    public class BeamSettingsPanel : BeamPanelGroup
    {
        private bool _undocked = false;
        public bool Undocked => _undocked;

        [SerializeField]
        private Transform _dockPosition;
        [Tooltip("We use undocked rather than docked as we want things to enable when the panel is undocked.")]
        public UnityEvent<bool> OnUndockStateChanged;

        [SerializeField, Tooltip("Amount of time for the panel to return to it's docked position")]
        private float _dockAnimationTime = 0.1f;
        private float _dockCurrentAnimation = 0;
        private IEnumerator _dockAnimation = null;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;

        protected override void OnEnable()
        {
            base.OnEnable();
            transform.position = _dockPosition.position;
            transform.rotation = _dockPosition.rotation;
            OnUndockStateChanged?.Invoke(_undocked);
        }

        public void ToggleDock()
        {
            _undocked = !_undocked;
            OnUndockStateChanged?.Invoke(_undocked);
            if (!_undocked)
            {
                DockAnimation();
            }
        }

        private void LateUpdate()
        {
            if (_dockPosition != null && !_undocked)
            {
                if(_dockCurrentAnimation > 0)
                {
                    transform.position = Vector3.Lerp(_dockPosition.position, _lastPosition, _dockCurrentAnimation);
                    transform.rotation = Quaternion.Lerp(_dockPosition.rotation, _lastRotation, _dockCurrentAnimation);
                }
                else
                {
                    transform.position = _dockPosition.position;
                    transform.rotation = _dockPosition.rotation;
                }
            }
        }

        private void DockAnimation()
        {
            if(_dockAnimation != null)
            {
                StopCoroutine(_dockAnimation);
            }
            _dockAnimation = DockAnimationRoutine();
            StartCoroutine(_dockAnimation);
        }

        private IEnumerator DockAnimationRoutine()
        {
            _dockCurrentAnimation = 1f;
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
            while (_dockCurrentAnimation > 0)
            {
                _dockCurrentAnimation -= Time.deltaTime * (1.0f / _dockAnimationTime);
                yield return null;
            }
            _dockCurrentAnimation = 0;
        }
    }
}