using System;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Manager.GamePlay
{
    public class ScoreManager
    {
        public event Action<Roles, byte> OnScoreChanged;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        public void OnRoundEnd(Roles? teamWon)
        {
            if (teamWon == null)
            {
                Debug.LogError("ScoreManager: team won is null");
                return;
            }

            IncreaseScore(_teamManager.GetAllTeams().First(team => team.Roles == teamWon.Value), 1);
        }

        public void UpdateUIClient(Team teamWon)
        {
            OnScoreChanged?.Invoke(teamWon.Roles, teamWon.Score);
        }

        private void IncreaseScore(Team team, byte amount)
        {
            if (team == null)
            {
                Debug.LogError("ScoreManager: Team is null");
                return;
            }

            team.Score += amount;
            Debug.Log($"Team {team.Name} score increased; {team.Score}");
            OnScoreChanged?.Invoke(team.Roles, team.Score);
        }
    }
}