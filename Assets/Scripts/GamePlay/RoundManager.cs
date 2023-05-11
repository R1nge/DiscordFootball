using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Zenject;

namespace GamePlay
{
    public class RoundManager
    {
        public event Action OnPreStartEvent;
        public event Action OnStartEvent;
        public event Action OnEndEvent;
        public event Action OnReplayEvent;

        public Roles? GetLastWonTeam() => _lastWonTeam;
        
        public bool IsReplay() => _isReplay;

        private ScoreManager _scoreManager;
        private TeamManager _teamManager;
        private BallSpawner _ballSpawner;
        private bool _isReplay;
        private Roles? _lastWonTeam;

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
            _isReplay = false;
            OnStartEvent?.Invoke();
        }

        public async void EndRound(Roles teamWon)
        {
            switch (teamWon)
            {
                case Roles.Red:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Red), 1);
                    _lastWonTeam = Roles.Red;
                    break;
                case Roles.Blue:
                    _scoreManager.IncreaseScore(_teamManager.GetAllTeams().First(team1 => team1.Roles == Roles.Blue), 1);
                    _lastWonTeam = Roles.Blue;
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            await ShowReplay();
            OnEndEvent?.Invoke();
        }

        private async UniTask ShowReplay()
        {
            _isReplay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(3), DelayType.Realtime);
            OnReplayEvent?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(10), DelayType.Realtime);
            PreStartRound();
        }
    }
}