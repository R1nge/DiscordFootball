using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Manager.GamePlay
{
    public class TurnManager : NetworkBehaviour
    {
        public event Action OnTurnStartedEvent;
        public event Action OnTurnEndedEvent;

        [SerializeField] private float turnTime;
        private NetworkVariable<float> _turnTime;
        private bool _hasTimerStarted;
        private RoundManager _roundManager;
        private RigidbodiesManager _rigidbodiesManager;

        [Inject]
        private void Construct(RoundManager roundManager, RigidbodiesManager rigidbodiesManager)
        {
            _roundManager = roundManager;
            _rigidbodiesManager = rigidbodiesManager;
        }

        private void Awake()
        {
            _turnTime = new(turnTime);
            NetworkManager.Singleton.NetworkTickSystem.Tick += Timer;
            _roundManager.OnStartEvent += StartTimer;
            _roundManager.OnReplayEvent += StopTimer;
            _roundManager.OnEndEvent += StopTimer;
        }

        public NetworkVariable<float> GetRemainingTime() => _turnTime;

        private void StartTimer()
        {
            _hasTimerStarted = true;
            _turnTime.Value = turnTime;
        }

        private void StopTimer()
        {
            _hasTimerStarted = false;
            _turnTime.Value = turnTime;
        }

        private async void Timer()
        {
            if (!_hasTimerStarted) return;

            if (_turnTime.Value > 0)
            {
                _turnTime.Value -= 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;
            }
            else
            {
                OnTurnEndedEvent?.Invoke();
                _hasTimerStarted = false;
                await WaitUntilAllStops();
            }
        }

        private async UniTask WaitUntilAllStops()
        {
            await UniTask.DelayFrame(10);
            await UniTask.WaitUntil(() => _rigidbodiesManager.HaveRigidbodiesStopped());
            await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.Realtime);

            OnTurnStartedEvent?.Invoke();
            _turnTime.Value = turnTime;
            _hasTimerStarted = true;
        }

        public override void OnDestroy()
        {
            _roundManager.OnStartEvent -= StartTimer;
            _roundManager.OnReplayEvent -= StopTimer;
            _roundManager.OnEndEvent -= StopTimer;
            if (!NetworkManager.Singleton) return;
            if (NetworkManager.Singleton.NetworkTickSystem == null) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Timer;
        }
    }
}