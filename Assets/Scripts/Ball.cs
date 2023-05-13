using GamePlay;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Ball : NetworkBehaviour
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
        if (IsOwner)
        {
            _roundManager.OnReplayEvent += PlayRound;
            _turnManager.OnTurnEndedEvent += SaveReplay;
        }
        
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void SaveReplay()
    {
        _rigidbody.velocity = Vector3.zero;
        _replayPosition = transform.position;
    }

    private void PlayRound()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = _replayPosition;
    }

    private void OnDestroy()
    {
        _roundManager.OnReplayEvent -= PlayRound;
        _turnManager.OnTurnEndedEvent -= SaveReplay;
    }
}