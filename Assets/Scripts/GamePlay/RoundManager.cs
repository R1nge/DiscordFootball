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
        
        private bool _isReplay;
        private Roles? _lastWonTeam;

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
                    _lastWonTeam = Roles.Red;
                    break;
                case Roles.Blue:
                    _lastWonTeam = Roles.Blue;
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnEndEvent?.Invoke();
            await ShowReplay();
        }

        private async UniTask ShowReplay()
        {
            _isReplay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(3), DelayType.Realtime);
            OnReplayEvent?.Invoke();
            //TODO: await for rigidbodies stop
            await UniTask.Delay(TimeSpan.FromSeconds(10), DelayType.Realtime);
            PreStartRound();
        }
    }
}