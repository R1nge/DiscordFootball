﻿using GamePlay;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    private RoundManager _roundManager;
    private TurnManager _turnManager;
    private Vector3 _replayPosition;
    private Rigidbody _rigidbody;

    [Inject]
    private void Construct(RoundManager roundManager, TurnManager turnManager)
    {
        _roundManager = roundManager;
        _turnManager = turnManager;
    }

    private void Awake()
    {
        _roundManager.OnReplayEvent += PlayReplay;
        _turnManager.OnTurnEndedEvent += SaveReplay;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void SaveReplay()
    {
        _rigidbody.velocity = Vector3.zero;
        _replayPosition = transform.position;
    }

    private void PlayReplay()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = _replayPosition;
    }

    private void OnDestroy()
    {
        _roundManager.OnReplayEvent -= PlayReplay;
        _turnManager.OnTurnEndedEvent -= SaveReplay;
    }
}