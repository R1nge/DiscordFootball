using System;
using UnityEngine;

namespace GamePlay
{
    public class ScoreManager
    {
        public event Action<Team, byte> OnScoreChanged;

        public void IncreaseScore(Team team, byte amount)
        {
            if (team == null)
            {
                Debug.LogError("ScoreManager: Team is null");
                return;
            }

            team.Score += amount;
            OnScoreChanged?.Invoke(team, team.Score);
        }
    }
}