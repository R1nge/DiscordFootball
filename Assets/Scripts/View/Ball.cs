using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace View
{
    public class Ball : NetworkBehaviour
    {
        private RoundManager _roundManager;
        private TurnManager _turnManager;
        private Vector3 _replayPosition;
        private Rigidbody _rigidbody;

        [Inject]
        private void Construct(RoundManager roundManager, TurnManager turnManager)
        {
            _roundManager = roundManager;
            _turnManager = turnManager;
        }

        private void Start()
        {
            if (!IsServer) return;
            _roundManager.OnReplayEvent += PlayReplay;
            _turnManager.OnTurnEndedEvent += SaveReplay;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void SaveReplay()
        {
            _rigidbody.velocity = Vector3.zero;
            _replayPosition = transform.position;
        }

        private void PlayReplay()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = _replayPosition;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!IsServer) return;
            _roundManager.OnReplayEvent -= PlayReplay;
            _turnManager.OnTurnEndedEvent -= SaveReplay;
        }
    }
}