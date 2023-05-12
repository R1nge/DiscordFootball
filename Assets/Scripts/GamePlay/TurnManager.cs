using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class TurnManager : MonoBehaviour
    {
        public event Action OnTurnStartedEvent;
        public event Action OnTurnEndedEvent;

        [SerializeField] private float turnTime;
        private float _turnTime;
        private bool _hasTimerStarted;
        private RoundManager _roundManager;
        private RigidbodiesManager _rigidbodiesManager;

        [Inject]
        private void Construct(RoundManager roundManager, RigidbodiesManager rigidbodiesManager)
        {
            _roundManager = roundManager;
            _rigidbodiesManager = rigidbodiesManager;
        }

        private void StartTimer()
        {
            _hasTimerStarted = true;
            _turnTime = turnTime;
        }

        private void StopTimer()
        {
            _hasTimerStarted = false;
            _turnTime = turnTime;
        }

        public float GetRemainingTime() => _turnTime;

        private void Awake()
        {
            NetworkManager.Singleton.NetworkTickSystem.Tick += Timer;
            _roundManager.OnStartEvent += StartTimer;
            _roundManager.OnReplayEvent += StopTimer;
            _roundManager.OnEndEvent += StopTimer;
        }

        private void Start() => _turnTime = turnTime;

        private async void Timer()
        {
            if (!_hasTimerStarted) return;
            if (_turnTime > 0)
            {
                _turnTime -= 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;
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
            _turnTime = turnTime;
            _hasTimerStarted = true;
        }

        private void OnDestroy()
        {
            _roundManager.OnStartEvent -= StartTimer;
            _roundManager.OnReplayEvent -= StopTimer;
            _roundManager.OnEndEvent -= StopTimer;
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Timer;
        }
    }
}