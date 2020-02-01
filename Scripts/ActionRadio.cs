using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ActionRadio : MonoBehaviour, Actionable
{
    private AudioSource _audioSource;
    private Animator _animator;
    private static readonly int IsPlaying = Animator.StringToHash("isPlaying");

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public void OnAction()
    {
        if(_audioSource.isPlaying)
            _audioSource.Pause();
        else
            _audioSource.Play();
        
        _animator.SetBool(IsPlaying, _audioSource.isPlaying);
    }
}
