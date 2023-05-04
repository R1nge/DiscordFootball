using System;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private float turnTime;
        private float _turnTime;
        public event Action OnTurnStartedEvent;
        public event Action OnTurnEndedEvent;

        private void Awake()
        {
            NetworkManager.Singleton.NetworkTickSystem.Tick += Timer;
        }

        private void Start() => _turnTime = turnTime;

        private void Timer()
        {
            _turnTime -= 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;

            if (_turnTime <= 0)
            {
                OnTurnEndedEvent?.Invoke();
                
                //TODO: wait until everybody (including the ball) will stop
                OnTurnStartedEvent?.Invoke();
                _turnTime = turnTime;
            }
        }

        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Timer;
        }
    }
}