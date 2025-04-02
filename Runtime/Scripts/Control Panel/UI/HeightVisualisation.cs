using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BeamXR.Director.ControlPanel
{
    public class HeightVisualisation : BeamCameraControlElement
    {
        [SerializeField]
        private TextMeshProUGUI _cameraText, _headText;
        private RectTransform _cameraHeight, _headHeight;

        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private float _maxUIHeight = 16;

        [SerializeField]
        private float _maxVisHeight = 1f;

        protected override void Awake()
        {
            base.Awake();
            FindParts();
        }

        private void FindParts()
        {
            if(_cameraText != null)
            {
                _cameraHeight = _cameraText.GetComponent<RectTransform>();
            }

            if(_headText != null)
            {
                _headHeight = _headText.GetComponent<RectTransform>();
            }
        }

        protected override void UpdateSettings()
        {
            float camHeight = _streamingCamera.CurrentCameraSettings.cameraHeight;
            float headHeight = _streamingCamera.CurrentCameraSettings.headHeight;

            Vector3 camPos = _lineRenderer.GetPosition(0), headPos = _lineRenderer.GetPosition(1);

            camPos.y = Mathf.Lerp(-_maxUIHeight, _maxUIHeight, Mathf.InverseLerp(-_maxVisHeight, _maxVisHeight, camHeight));
            headPos.y = Mathf.Lerp(-_maxUIHeight, _maxUIHeight, Mathf.InverseLerp(-_maxVisHeight, _maxVisHeight, headHeight));

            _lineRenderer.SetPosition(0, camPos);
            _lineRenderer.SetPosition(1, headPos);

            _cameraHeight.anchoredPosition = new Vector2(_cameraHeight.anchoredPosition.x, camPos.y);
            _headHeight.anchoredPosition = new Vector2(_headHeight.anchoredPosition.x, headPos.y);

            _cameraText.text = (camHeight > 0 ? "+" : "") + camHeight + "m";
            _headText.text = (headHeight > 0 ? "+" : "") + headHeight + "m";
        }
    }
}