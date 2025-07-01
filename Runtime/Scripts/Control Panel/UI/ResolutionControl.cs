using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BeamXR.Streaming.Core;
using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class ResolutionControl : BeamComponent
    {
        [SerializeField]
        private MenuGroup _menuGroup;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private GameObject _buttonPrefab;

        [SerializeField]
        private CaptureType _resolutionType = CaptureType.Cloud;

        private Dictionary<BeamResolution, ButtonAnimator> _buttons = new Dictionary<BeamResolution, ButtonAnimator>();

        protected override void FindParts()
        {
            base.FindParts();
            if (_menuGroup == null)
            {
                _menuGroup = GetComponentInChildren<MenuGroup>(true);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            for (int i = -2; i < 5; i++)
            {
                if (Enum.IsDefined(typeof(BeamResolution), i))
                {
                    BeamResolution res = (BeamResolution)i;
                    GameObject go = Instantiate(_buttonPrefab, _parent);
                    var animator = go.GetComponent<ButtonAnimator>();
                    var label = res.ToString();
                    var match = Regex.Match(label, @"\d");
                    if (match.Success)
                    {
                        label = label.Substring(match.Index);
                    }
                    animator.Text.text = label;
                    animator.Image.gameObject.SetActive(false);

                    switch (_resolutionType)
                    {
                        case CaptureType.Local:
                            animator.OnClick += () => _beamManager.SetLocalResolution(res);
                            break;
                        case CaptureType.Cloud:
                            animator.OnClick += () => _beamManager.SetCloudResolution(res);
                            break;
                        case CaptureType.Photo:
                            animator.OnClick += () => _beamManager.SetPhotoResolution(res);
                            break;
                    }
                    _buttons.Add(res, animator);
                }
            }
        }

        protected void OnEnable()
        {
            _unityEvents.OnResolutionChanged.AddListener(UpdateSettings);

            _unityEvents.OnCapturingStarted.AddListener(CapturingStarted);
            _unityEvents.OnCapturingEnded.AddListener(CapturingEnded);

            UpdateSettings(_resolutionType, _beamManager.GetResolution(_resolutionType));

            if (_beamManager.IsCapturing)
            {
                CapturingStarted();
            }
            else
            {
                CapturingEnded();
            }
        }

        private void OnDisable()
        {
            _unityEvents.OnResolutionChanged.RemoveListener(UpdateSettings);
            _unityEvents.OnCapturingStarted.RemoveListener(CapturingStarted);
            _unityEvents.OnCapturingEnded.RemoveListener(CapturingEnded);
        }

        private void UpdateSettings(CaptureType resType, BeamResolution resolution)
        {
            if (resType == _resolutionType)
            {
                _menuGroup.ChangeSelected(_buttons[resolution]);
            }
        }

        private void CapResolution()
        {
            if (_beamManager.StreamAvailability == null)
            {
                foreach (var button in _buttons)
                {
                    if((int)button.Key > 0)
                    {
                        button.Value.gameObject.SetActive(false);
                    }
                    button.Value.Selectable.interactable = false;
                }
                StartCoroutine(AwaitAvailability());
            }
            else
            {
                foreach (var button in _buttons)
                {
                    if ((int)button.Key > (int)_beamManager.StreamAvailability.CloudMaxResolution)
                    {
                        button.Value.gameObject.SetActive(false);
                        button.Value.Selectable.interactable = false;
                    }
                    else
                    {
                        button.Value.gameObject.SetActive(true);
                        button.Value.Selectable.interactable = true;
                    }
                }
            }
        }

        private IEnumerator AwaitAvailability()
        {
            var wfs = new WaitForSecondsRealtime(0.5f);
            while (_beamManager.StreamAvailability == null)
            {
                yield return wfs;
            }
            CapResolution();
        }

        private void CapturingStarted()
        {
            foreach (var button in _buttons)
            {
                button.Value.Selectable.interactable = false;
            }
        }

        private void CapturingEnded()
        {
            foreach (var button in _buttons)
            {
                button.Value.Selectable.interactable = true;
            }

            if (_resolutionType == CaptureType.Cloud)
            {
                CapResolution();
            }
        }
    }
}