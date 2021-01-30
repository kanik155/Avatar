using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This camera input class is an example of how to get input from a connected mouse using Unity's default input system;
    //It also includes an optional mouse sensitivity setting;
    public class CameraMouseInput : CameraInput
    {
        //Mouse input axes;
        public string mouseHorizontalAxis = "Mouse X";
        public string mouseVerticalAxis = "Mouse Y";

        //Invert input options;
        public bool invertHorizontalInput = true;
        public bool invertVerticalInput = false;

        //Use this value to fine-tune mouse movement;
        //All mouse input will be multiplied by this value;
        public float mouseInputMultiplier = 0.0005f;

        private float screenPressStartPointX = 0f;
        private float screenPressStartPointY = 0f;

        public override float GetHorizontalCameraInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPressStartPointX = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                //Get raw mouse input;
                // float _input = Input.GetAxisRaw(mouseHorizontalAxis);
                float _input = (screenPressStartPointX - Input.mousePosition.x) / 100;

                if (_input < -1.0f)
                {
                    _input = -1.0f;
                }
                else if (_input > 1.0f)
                {
                    _input = 1.0f;
                }

                //Since raw mouse input is already time-based, we need to correct for this before passing the input to the camera controller;
                if (Time.timeScale > 0f)
                {
                    _input /= Time.deltaTime;
                    _input *= Time.timeScale;
                }
                else
                    _input = 0f;

                //Apply mouse sensitivity;
                _input *= mouseInputMultiplier;

                //Invert input;
                if (invertHorizontalInput)
                    _input *= -1f;

                return _input;
            }

            return 0;
        }

        public override float GetVerticalCameraInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPressStartPointY = Input.mousePosition.y;
            }

            if (Input.GetMouseButton(0))
            {
                //Get raw mouse input;
                // float _input = -Input.GetAxisRaw(mouseVerticalAxis);
                float _input = (screenPressStartPointY - Input.mousePosition.y) / 100;

                if (_input < -1.0f)
                {
                    _input = -1.0f;
                }
                else if (_input > 1.0f)
                {
                    _input = 1.0f;
                }

                //Since raw mouse input is already time-based, we need to correct for this before passing the input to the camera controller;
                if (Time.timeScale > 0f)
                {
                    _input /= Time.deltaTime;
                    _input *= Time.timeScale;
                }
                else
                    _input = 0f;

                //Apply mouse sensitivity;
                _input *= mouseInputMultiplier;

                //Invert input;
                if (invertVerticalInput)
                    _input *= -1f;

                return _input;
            }

            return 0;
        }
    }
}
