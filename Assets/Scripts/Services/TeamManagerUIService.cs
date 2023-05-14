using Manager.GamePlay;
using VContainer;

namespace Services
{
    public class TeamManagerUIService
    {
        private readonly TeamManager _teamManager;
        private readonly RoundManager _roundManager;

        [Inject]
        private TeamManagerUIService(TeamManager teamManager, RoundManager roundManager)
        {
            _teamManager = teamManager;
            _roundManager = roundManager;
        }

        public void SelectTeam(Team team)
        {
            _teamManager.SelectTeam(team);
            _roundManager.PreStartRound();
        }
    }
}