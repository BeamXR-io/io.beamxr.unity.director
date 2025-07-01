using System;
using UnityEngine;
using UnityEngine.UI;
namespace BeamXR.Director.ControlPanel
{
    public abstract class ButtonBind : MonoBehaviour
    {
        protected Button _button;

        protected void Bind(Action action)
        {
            if(_button == null)
            {
                _button = GetComponent<Button>();
            }
            _button.onClick.AddListener(() => { action?.Invoke(); });
        }

    }
}