using System;
using System.Linq;
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
        private Rigidbody[] _rigidbodies;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(RoundManager roundManager)
        {
            _roundManager = roundManager;
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
            _roundManager.OnStartEvent += FindAllRigidbodies;
            _roundManager.OnStartEvent += StartTimer;
            _roundManager.OnEndEvent += StopTimer;
        }

        private void Start() => _turnTime = turnTime;

        private void FindAllRigidbodies()
        {
            _rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

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
            if (_rigidbodies == null) return;
            await UniTask.WaitUntil(() => _rigidbodies.All(rb => rb.velocity == Vector3.zero));
            await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.Realtime);

            OnTurnStartedEvent?.Invoke();
            _turnTime = turnTime;
            _hasTimerStarted = true;
        }

        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Timer;
        }
    }
}