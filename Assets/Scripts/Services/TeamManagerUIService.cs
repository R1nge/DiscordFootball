using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public void SelectTeam(Team team)
        {
            _teamManager.SelectTeam(team);
            Debug.LogError(team.Roles);
            //TODO: keep track of all players and theirs teams
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            Debug.LogError(_teamManager.GetAllTeams()[0].Roles);
        }
    }
}