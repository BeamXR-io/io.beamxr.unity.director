using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BeamXR.Streaming.Core;
using System.Collections;
using System.Linq;

public class StreamingLocationText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    
    [SerializeField]
    private Image _streamIcon;

    [SerializeField]
    private Sprite _cloudSprite, _appSprite;

    [SerializeField, HideInInspector]
    private BeamUnityEvents _unityEvents;

    private bool _checkingForStream = false;

    private void Awake()
    {
        FindParts();
    }

    private void OnValidate()
    {
        FindParts();
    }

    private void FindParts()
    {
        if(_unityEvents == null)
        {
            _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
        }
    }

    private void OnEnable()
    {
        if(BeamStreamingManager.Instance != null)
        {
            UpdateHostInfo();
            if (BeamStreamingManager.Instance.StreamingState == StreamingState.Disconnected)
            {
                StartCoroutine(CheckHosts());
            }
            if(_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.AddListener(StreamStarted);
                _unityEvents.OnStreamEnded.AddListener(StreamStopped);
            }
        }
    }

    private void OnDisable()
    {
        if (BeamStreamingManager.Instance != null)
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.AddListener(StreamStarted);
                _unityEvents.OnStreamEnded.AddListener(StreamStopped);
            }
        }
    }

    private void StreamStarted()
    {
        if (_checkingForStream)
        {
            StopCoroutine(CheckHosts());
        }
    }

    private void StreamStopped()
    {
        if (!_checkingForStream)
        {
            StartCoroutine(CheckHosts());
        }
    }

    private IEnumerator CheckHosts()
    {
        _checkingForStream = true;
        WaitForSecondsRealtime hostTime = new WaitForSecondsRealtime(1f);
        while (true)
        {
            UpdateHostInfo();
            yield return hostTime;
        }

    }

    private void UpdateHostInfo()
    {
        if(BeamStreamingManager.Instance.StreamingState != StreamingState.Disconnected)
        {
            if (BeamStreamingManager.Instance.HostType.ToLower().Contains("cloud"))
            {
                CloudStream();
            }
            else
            {
                AppStream();
            }
        }
        else
        {
            if(BeamStreamingManager.Instance.AvailableStreamingHosts.Length > 0)
            {
                AppStream();
            }
            else
            {
                CloudStream();
            }
        }
    }

    private void CloudStream()
    {
        _text.text = "Cloud stream";
        _streamIcon.sprite = _cloudSprite;
    }

    private void AppStream()
    {
        _text.text = $"App stream to {BeamStreamingManager.Instance.AvailableStreamingHosts[0].hostName}";
        _streamIcon.sprite = _appSprite;
    }
}
