using System;
using System.Linq;
using GamePlay;
using UnityEngine;
using Zenject;

public class Goal : MonoBehaviour
{
    [SerializeField] private Roles role;
    private TeamManager _teamManager;
    private ScoreManager _scoreManager;

    [Inject]
    private void Construct(TeamManager teamManager, ScoreManager scoreManager)
    {
        _teamManager = teamManager;
        _scoreManager = scoreManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            switch (role)
            {
                case Roles.Red:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Blue), 1);
                    break;
                case Roles.Blue:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Red), 1);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}