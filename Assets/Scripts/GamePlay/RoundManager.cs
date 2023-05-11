using System;
using System.Linq;
using Zenject;

namespace GamePlay
{
    public class RoundManager
    {
        public event Action OnPreStartEvent;
        public event Action OnStartEvent;
        public event Action OnEndEvent;
        public event Action OnReplayEvent;

        public bool IsReplay() => _isReplay;

        private ScoreManager _scoreManager;
        private TeamManager _teamManager;
        private bool _isReplay;
        
        [Inject]
        private void Construct(ScoreManager scoreManager, TeamManager teamManager)
        {
            _scoreManager = scoreManager;
            _teamManager = teamManager;
        }

        public void PreStartRound()
        {
            OnPreStartEvent?.Invoke();
        }

        public void StartRound()
        {
            //TODO: spawn a ball
            _isReplay = false;
            OnStartEvent?.Invoke();
        }

        public void EndRound(Roles role)
        {
            switch (role)
            {
                case Roles.Red:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Red), 1);
                    break;
                case Roles.Blue:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Red), 1);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnEndEvent?.Invoke();
            ShowReplay();
        }

        private void ShowReplay()
        {
            //TODO: replay delay
            _isReplay = true;
            OnReplayEvent?.Invoke();
        }
    }
}