using Manager.GamePlay;
using UnityEngine;
using VContainer;

namespace Services
{
    public class TeamManagerUIService
    {
        private readonly TeamManager _teamManager;

        [Inject]
        private TeamManagerUIService(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        public void SelectTeam(Team team, ulong playerId)
        {
            _teamManager.SelectTeam(team, playerId);
            Debug.LogError($"{playerId} : {team.Roles}");
            for (var i = 0; i < _teamManager.GetAllTeams().Length; i++)
            {
                Debug.LogError(_teamManager.GetAllTeams()[i].Roles);
            }
        }
    }
}