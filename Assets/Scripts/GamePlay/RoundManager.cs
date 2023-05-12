using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace GamePlay
{
    public class RoundManager
    {
        public event Action OnPreStartEvent;
        public event Action OnStartEvent;
        public event Action OnReplayEvent;
        public event Action OnEndEvent;

        public Roles? GetLastWonTeam() => _lastWonTeam;

        public bool IsReplay() => _isReplay;

        private bool _isReplay;
        private Roles? _lastWonTeam;
        private RigidbodiesManager _rigidbodiesManager;

        [Inject]
        private void Construct(RigidbodiesManager rigidbodiesManager)
        {
            _rigidbodiesManager = rigidbodiesManager;
        }

        public void PreStartRound()
        {
            _rigidbodiesManager.DeleteAllRigidbodies();
            OnPreStartEvent?.Invoke();
        }

        public void StartRound()
        {
            _rigidbodiesManager.FindAllRigidbodies();
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
            _isReplay = true;
            await ShowReplay();
            await UniTask.Delay(TimeSpan.FromSeconds(3), DelayType.Realtime);
            PreStartRound();
        }

        private async UniTask ShowReplay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), DelayType.Realtime);
            OnReplayEvent?.Invoke();
            await UniTask.WaitUntil(() => _rigidbodiesManager.HaveRigidbodiesStopped());
        }
    }
}