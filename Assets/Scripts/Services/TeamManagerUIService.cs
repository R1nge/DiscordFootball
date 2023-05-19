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
            Debug.LogError(team.Roles);
            //TODO: keep track of all players and theirs teams
            //TODO: load game scene when host press a button "Start" or something
            //NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Additive);
            Debug.LogError(_teamManager.GetAllTeams()[0].Roles);
        }
    }
}