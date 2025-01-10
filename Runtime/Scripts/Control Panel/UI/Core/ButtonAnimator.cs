using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Transform _objectToAnimate;

        [SerializeField]
        private float _animationTime = 0.08f;

        private Selectable _selectable;

        private float _initialZValue = 0f;

        private Vector3 _currentPosition;
        private float _lerpValue = 0, _targetValue = 0;
        private bool _forcedState = false;

        private bool _animating = false;

        private void Awake()
        {
            _initialZValue = _objectToAnimate.localPosition.z;

            if (_selectable == null)
            {
                _selectable = GetComponent<Selectable>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_selectable.interactable)
                return;

            ChangeState(1);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_selectable.interactable)
                return;

            ChangeState(0);
        }

        private void ChangeState(float value)
        {
            if (_forcedState)
            {
                _targetValue = 1;
            }
            else
            {
                _targetValue = value;
            }
            if (!_animating)
            {
                StartCoroutine(AnimationRoutine());
            }
        }

        public void ForcePressed(bool force)
        {
            _forcedState = force;
            _targetValue = _forcedState ? 1 : 0;
            if (!_animating)
            {
                StartCoroutine(AnimationRoutine());
            }
        }

        private void OnEnable()
        {
            SetPosition(_forcedState ? 1 : 0);
            _animating = false;
        }

        private void OnDisable()
        {
            SetPosition(_forcedState ? 1 : 0);
            _animating = false;
        }

        private IEnumerator AnimationRoutine()
        {
            _animating = true;
            while (_lerpValue != _targetValue)
            {
                _lerpValue = Mathf.Clamp01(Mathf.Lerp(_lerpValue, _targetValue, Time.deltaTime * (1f / _animationTime)));
                SetPosition(_lerpValue);
                yield return null;
            }
            _animating = false;
        }

        private void SetPosition(float amount)
        {
            _currentPosition = _objectToAnimate.localPosition;
            _currentPosition.z = Mathf.Lerp(_initialZValue, 0, amount);
            _objectToAnimate.localPosition = _currentPosition;
        }
    }
}