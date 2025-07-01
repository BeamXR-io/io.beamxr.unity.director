using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class ToggleAnimator : MonoBehaviour
    {
        [SerializeField]
        private ButtonAnimator _buttonAnimator;
        public Selectable Selectable => _buttonAnimator.Selectable;

        [SerializeField]
        private Image _objectToAnimate;

        [SerializeField]
        private float _animationTime = 0.08f;

        [SerializeField, HideInInspector]
        private RectTransform _rect, _parent;
        private float _toggleStart, _toggleEnd;
        private float _lerp = 0;

        private bool _awaitingRefresh = false;

        private IEnumerator _animation;

        private Color _onColor;

        private bool _isOn = false;
        public Action<bool> OnValueChanged;

        private void Awake()
        {
            FindParts();
            _onColor = _buttonAnimator.Selectable.colors.selectedColor;
            _buttonAnimator.OnClick += ToggleClick;
        }

        private void OnValidate()
        {
            FindParts();
        }

        private void FindParts()
        {
            if (_buttonAnimator == null)
            {
                _buttonAnimator = GetComponent<ButtonAnimator>();
            }

            if (_objectToAnimate != null)
            {
                _rect = _objectToAnimate.GetComponent<RectTransform>();
                _parent = GetComponent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            FindParts();
            GetSizing();
            ToggleAction();
            _awaitingRefresh = true;
        }

        private void Update()
        {
            if (_awaitingRefresh)
            {
                GetSizing();
                UpdateVisuals();
                _awaitingRefresh = false;
            }
        }

        private void GetSizing()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_parent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);

            float val = Mathf.Abs(_rect.sizeDelta.y) / 2f;
            _toggleStart = val;

            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_parent);
            
            _toggleEnd = bounds.size.x - _rect.sizeDelta.x - _toggleStart;
        }

        public void SetToggle(bool value, bool silent = true)
        {
            _isOn = value;
            ToggleAction();
            if (!silent)
            {
                OnValueChanged?.Invoke(_isOn);
            }
        }

        private void ToggleClick()
        {
            _isOn = !_isOn;
            ToggleAction();
            OnValueChanged?.Invoke(_isOn);
        }

        private void ToggleAction()
        {
            if (_isOn)
            {
                _buttonAnimator.SetAlternativeColor(_onColor, _onColor);
            }
            else
            {
                _buttonAnimator.ResetAlternativeColor();
            }

            if (gameObject.activeInHierarchy)
            {
                if (_animation != null)
                {
                    StopCoroutine(_animation);
                    _animation = null;
                }
                _animation = AnimateToggle();
                StartCoroutine(_animation);
            }
            else
            {
                _lerp = _isOn ? 1 : 0;
                UpdateVisuals();
            }
        }

        public void UpdateVisuals()
        {
            _rect.anchoredPosition = new Vector2(Mathf.Lerp(_toggleStart, _toggleEnd, _lerp), 0);
        }

        private void OnRectTransformDimensionsChange()
        {
            GetSizing();
            UpdateVisuals();
        }

        private IEnumerator AnimateToggle()
        {
            while (_lerp != (_isOn ? 1 : 0))
            {
                _lerp += Time.unscaledDeltaTime * ((_isOn ? 1 : -1) / _animationTime);
                if (_lerp > 1)
                {
                    _lerp = 1;
                }
                if (_lerp < 0)
                {
                    _lerp = 0;
                }
                UpdateVisuals();
                yield return null;
            }
            _animation = null;
        }

    }
}