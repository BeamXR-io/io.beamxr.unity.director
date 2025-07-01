using System.Collections;
using System.Collections.Generic;
using BeamXR.Streaming.Core.Models;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PlatformControl : BeamComponent
    {
        [SerializeField]
        private GameObject _platformTogglePrefab;

        private Dictionary<StreamPlatform, PlatformToggle> _toggles = null;

        private bool _awaitingResult = false;

        private void OnEnable()
        {
            if (_beamManager.StreamPlatforms == null)
            {
                StartCoroutine(AwaitPlatforms());
            }
            else if (_toggles == null)
            {
                GeneratePlatforms();
            }
            else
            {
                UpdatePlatforms();
            }
        }

        private IEnumerator AwaitPlatforms()
        {
            WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(0.5f);
            while (_beamManager.StreamPlatforms == null)
            {
                yield return wfs;
            }
            GeneratePlatforms();
        }

        private void UpdatePlatforms()
        {
            if (!_awaitingResult)
            {
                ChangeInteractable(true);
            }

            foreach (var item in _beamManager.StreamPlatforms)
            {
                if (_toggles.TryGetValue(item, out var toggle))
                {
                    toggle.Toggle.SetToggle(item.AutoStream, true);
                }
            }
        }

        private void GeneratePlatforms()
        {
            _toggles = new Dictionary<StreamPlatform, PlatformToggle>();
            foreach (var item in _beamManager.StreamPlatforms)
            {
                GameObject go = Instantiate(_platformTogglePrefab, transform);
                PlatformToggle toggle = go.GetComponent<PlatformToggle>();
                toggle.Text.text = item.GetVisualPlatformName();
                toggle.Toggle.SetToggle(item.AutoStream, true);
                toggle.Toggle.OnValueChanged += (val) => { Toggle(val, item); };
                _toggles.Add(item, toggle);
            }
        }

        private void Toggle(bool value, StreamPlatform platform)
        {
            if (_beamManager.IsStreaming)
            {
                _beamManager.ToggleSocialStream(platform);
            }
            else
            {
                _beamManager.ChangePlatformAutoStream(platform, value, ToggleResult);
                ChangeInteractable(false);
                _awaitingResult = true;
            }
        }

        private void ChangeInteractable(bool interactable)
        {
            foreach (var item in _toggles)
            {
                item.Value.Toggle.Selectable.interactable = interactable;
            }
        }

        private void ToggleResult(bool value, StreamPlatform platform)
        {
            _awaitingResult = false;
            ChangeInteractable(true);
            if (_toggles.TryGetValue(platform, out PlatformToggle toggle))
            {
                toggle.Toggle.SetToggle(platform.AutoStream, true);
            }
        }
    }
}