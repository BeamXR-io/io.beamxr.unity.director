using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class ToggleGroupController : MonoBehaviour
    {
        [System.Serializable]
        public class TogglePair
        {
            public Toggle toggle;
            public List<GameObject> objectsToEnable = new List<GameObject>();
            public UnityEvent actionOnEnable;
        }

        private ColorBlock _normalBlock, _selectedBlock;

        [SerializeField]
        private int _selectedElement = 0;
        
        [SerializeField]
        private bool _selectOnEnable = true;

        [SerializeField]
        private List<TogglePair> _togglePairs = new List<TogglePair>();

        private void Start()
        {
            if (_togglePairs.Count <= _selectedElement)
            {
                _selectedElement = _togglePairs.Count;
            }

            _normalBlock = _togglePairs[_selectedElement].toggle.colors;
            _selectedBlock = _normalBlock;
            _selectedBlock.normalColor = _normalBlock.selectedColor;

            _togglePairs[_selectedElement].toggle.colors = _selectedBlock;

            for (int i = 0; i < _togglePairs.Count; i++)
            {
                int ind = i;

                _togglePairs[i].toggle.onValueChanged.AddListener((val) => { SelectElement(ind); });
            }
            if (_selectOnEnable)
            {
                SelectElement(_selectedElement);
            }
        }

        private void SelectElement(int index)
        {
            for (int i = 0; i < _togglePairs.Count; i++)
            {
                if(i == index)
                {
                    continue;
                }
                _togglePairs[i].toggle.colors = _normalBlock;
                if (_togglePairs[i].objectsToEnable != null && _togglePairs[i].objectsToEnable.Count > 0)
                {
                    for (int j = 0; j < _togglePairs[i].objectsToEnable.Count; j++)
                    {
                        _togglePairs[i].objectsToEnable[j].SetActive(false);
                    }
                }
            }

            _togglePairs[index].toggle.colors = _selectedBlock;
            _togglePairs[index].actionOnEnable?.Invoke();

            if (_togglePairs[index].objectsToEnable != null && _togglePairs[index].objectsToEnable.Count > 0)
            {
                for (int j = 0; j < _togglePairs[index].objectsToEnable.Count; j++)
                {
                    _togglePairs[index].objectsToEnable[j].SetActive(true);
                }
            }
        }
    }
}