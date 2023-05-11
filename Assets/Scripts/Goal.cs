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
        if (_roundManager.IsReplay()) return;
        if (other.TryGetComponent(out Ball ball))
        {
            switch (role)
            {
                case Roles.Red:
                    _roundManager.EndRound(Roles.Blue); //Blue win
                    break;
                case Roles.Blue:
                    _roundManager.EndRound(Roles.Red); //Red win
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}