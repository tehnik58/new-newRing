using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Hand_AC_Controller : MonoBehaviour
{
    private Animator _animator;
    [FormerlySerializedAs("shiftButtonAction")] public InputActionReference shiftButtonAction;
    [FormerlySerializedAs("kurokButtonAction")] public InputActionReference kurokButtonAction;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        _animator.SetFloat("shift", shiftButtonAction.action.ReadValue<float>());
        _animator.SetFloat("kurok", kurokButtonAction.action.ReadValue<float>());
    }
}
