using System;
using GamePlay;
using UnityEngine;
using Zenject;

public class Goal : MonoBehaviour
{
    [SerializeField] private Roles role;
    private RoundManager _roundManager;

    [Inject]
    private void Construct(RoundManager roundManager)
    {
        _roundManager = roundManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Ball ball)) return;

        if (ball.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.drag = 20f;
        }

        if (_roundManager.IsReplay()) return;

        switch (role)
        {
            case Roles.Red:
                _roundManager.EndRound(Roles.Blue);
                break;
            case Roles.Blue:
                _roundManager.EndRound(Roles.Red);
                break;
            case Roles.Spectator:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Ball ball)) return;

        if (ball.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.drag = .5f;
        }
    }
}