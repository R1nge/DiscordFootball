using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class ScoreManager : IInitializable, IDisposable
    {
        public event Action<Team, byte> OnScoreChanged;
        private RoundManager _roundManager;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(RoundManager roundManager, TeamManager teamManager)
        {
            _roundManager = roundManager;
            _teamManager = teamManager;
        }

        public void Initialize()
        {
            _roundManager.OnEndEvent += OnRoundEnd;
        }

        private void OnRoundEnd()
        {
            var teamWon = _roundManager.GetLastWonTeam();
            if (teamWon == null)
            {
                Debug.LogError("ScoreManager: team won is null");
                return;
            }

            switch (teamWon.Value)
            {
                case Roles.Red:
                    IncreaseScore(_teamManager.GetAllTeams().First(team => team.Roles == Roles.Red), 1);
                    break;
                case Roles.Blue:
                    IncreaseScore(_teamManager.GetAllTeams().First(team => team.Roles == Roles.Blue), 1);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void IncreaseScore(Team team, byte amount)
        {
            if (team == null)
            {
                Debug.LogError("ScoreManager: Team is null");
                return;
            }

            team.Score += amount;
            OnScoreChanged?.Invoke(team, team.Score);
        }

        public void Dispose()
        {
            _roundManager.OnEndEvent -= OnRoundEnd;
        }
    }
}