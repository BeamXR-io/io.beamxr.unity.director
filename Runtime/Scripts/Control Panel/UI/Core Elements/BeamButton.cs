using System;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class BeamButton : BeamComponent
    {
        [SerializeField]
        protected ButtonAnimator _button;

        protected override void FindParts()
        {
            base.FindParts();
            if(_button == null)
            {
                _button = GetComponent<ButtonAnimator>();
            }
        }

        protected void Bind(Action action)
        {
            if (_button == null)
            {
                _button = GetComponent<ButtonAnimator>();
            }
            _button.OnClick += () => { action?.Invoke(); };
        }
    }
}