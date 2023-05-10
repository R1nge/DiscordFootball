using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class ScoreManager
    {
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        public void IncreaseScore(ulong playerId, byte amount)
        {
            var team = _teamManager.GetTeam(playerId);
            if (team == null)
            {
                Debug.LogError("ScoreManager: Team is null");
                return;
            }

            team.Score += amount;
        }

        public byte GetScore(ulong playerId)
        {
            var team = _teamManager.GetTeam(playerId);
            if (team != null) return team.Score;
            Debug.LogError("ScoreManager: Team is null");
            return 0;
        }
    }
}