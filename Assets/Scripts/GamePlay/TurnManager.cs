using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

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


        public void StartTimer()
        {
            _hasTimerStarted = true;
            _turnTime = turnTime;
        }

        public void StopTimer()
        {
            _hasTimerStarted = false;
            _turnTime = turnTime;
        }

        public float GetRemainingTime() => _turnTime;

        private void Awake()
        {
            NetworkManager.Singleton.NetworkTickSystem.Tick += Timer;
        }

        private void Start() => _turnTime = turnTime;

        public void FindAllRigidbodies()
        {
            _rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        private void Timer()
        {
            if (!_hasTimerStarted) return;
            _turnTime -= 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;

            if (_turnTime <= 0)
            {
                OnTurnEndedEvent?.Invoke();

                //TODO: find all rigidbodies
                //When velocity of all of them equals 0 - StartTurn 
                if (_rigidbodies.All(rb => rb.velocity == Vector3.zero))
                {
                    OnTurnStartedEvent?.Invoke();
                    _turnTime = turnTime;
                    _hasTimerStarted = false;
                }
            }
        }

        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Timer;
        }
    }
}