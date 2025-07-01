using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private Transform _objectToAnimate;

        [SerializeField]
        private float _animationTime = 0.08f;

        private Selectable _selectable;
        public Selectable Selectable
        {
            get
            {
                if (_selectable == null)
                {
                    _selectable = GetComponent<Selectable>();
                }
                return _selectable;
            }
        }

        [SerializeField]
        private Image _image;
        public Image Image => _image;

        [SerializeField]
        private TextMeshProUGUI _text;
        public TextMeshProUGUI Text => _text;

        internal float _initialZValue = 0f;

        private Vector3 _currentPosition;
        private float _lerpValue = 0, _targetValue = 0;
        private bool _forcedState = false;

        private IEnumerator _animation = null;

        private bool _setupColors = false;
        private ColorBlock _normalBlock, _forcedBlock;

        private bool _alternateColors = false;
        private ColorBlock _alternativeBlock, _alternativeForcedBlock;

        public Action OnClick;
        private int _frame = -1;

        private void Awake()
        {
            _initialZValue = _objectToAnimate.localPosition.z;

            SetupColors();
        }

        private void SetupColors()
        {
            if(_initialZValue == 0)
            {
                _initialZValue = _objectToAnimate.localPosition.z;
            }

            if (_setupColors)
                return;

            if (_selectable == null)
            {
                _selectable = GetComponent<Selectable>();
            }
            if (_selectable != null)
            {
                _normalBlock = _selectable.colors;
                _alternativeBlock = _selectable.colors;
                _alternativeForcedBlock = _selectable.colors;
                _forcedBlock = _selectable.colors;
                _forcedBlock.normalColor = _forcedBlock.selectedColor;
                _setupColors = true;
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
            ChangeState(0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable)
                return;

            if(Time.frameCount > _frame)
            {
                OnClick?.Invoke();
                _frame = Time.frameCount;
            }
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
            CoroutineLogic();
        }

        private void CoroutineLogic()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
                _animation = null;
            }
            if (gameObject.activeInHierarchy)
            {
                _animation = AnimationRoutine();
                StartCoroutine(_animation);
            }
            else
            {
                SetPosition(_forcedState ? 1 : 0);
            }
        }

        public void ForcePressed(bool force)
        {
            _forcedState = force;
            _targetValue = _forcedState ? 1 : 0;
            SetupColors();
            UpdateColors();
            CoroutineLogic();
        }

        private void OnEnable()
        {
            SetPosition(_forcedState ? 1 : 0);
        }

        private void OnDisable()
        {
            SetPosition(_forcedState ? 1 : 0);
        }

        private IEnumerator AnimationRoutine()
        {
            while (_lerpValue != _targetValue)
            {
                _lerpValue = Mathf.Clamp01(Mathf.Lerp(_lerpValue, _targetValue, Time.deltaTime * (1f / _animationTime)));
                SetPosition(_lerpValue);
                yield return null;
            }
            _animation = null;
        }

        private void SetPosition(float amount)
        {
            _currentPosition = _objectToAnimate.localPosition;
            _currentPosition.z = Mathf.Lerp(_initialZValue, 0, amount);
            _objectToAnimate.localPosition = _currentPosition;
        }

        public void SetAlternativeColor(Color normalColor, Color forcedColor)
        {
            SetupColors();
            _alternativeBlock.normalColor = normalColor;
            _alternativeForcedBlock.normalColor = forcedColor;
            _alternateColors = true;
            UpdateColors();
        }

        public void ResetAlternativeColor()
        {
            SetupColors();
            _alternateColors = false;
            UpdateColors();
        }

        private void UpdateColors()
        {
            if (_forcedState)
            {
                _selectable.colors = _alternateColors ? _alternativeForcedBlock : _forcedBlock;
            }
            else
            {
                _selectable.colors = _alternateColors ? _alternativeBlock : _normalBlock;
            }
        }
    }
}