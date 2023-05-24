using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Manager.GamePlay
{
    public class RoundManager : IStartable
    {
        public event Action OnPreStartEvent;
        public event Action OnStartEvent;
        public event Action OnReplayEvent;
        public event Action OnEndEvent;

        public Roles? GetLastWonTeamRoles() => _lastWonTeam?.Roles;

        public Team GetLastWonTeam() => _lastWonTeam;

        public bool IsReplay() => _isReplay;

        private bool _isReplay;
        private Team _lastWonTeam = new();
        private RigidbodiesManager _rigidbodiesManager;

        [Inject]
        private void Construct(RigidbodiesManager rigidbodiesManager)
        {
            _rigidbodiesManager = rigidbodiesManager;
        }

        public void Start() => PreStartRound();

        private void PreStartRound()
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

        public async void EndRound(Team teamWon)
        {
            _lastWonTeam = teamWon;
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