using Manager.GamePlay;
using Services;
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
        private BallService _ballService;

        [Inject]
        private void Construct(RoundManager roundManager, TurnManager turnManager, BallService ballService)
        {
            _roundManager = roundManager;
            _turnManager = turnManager;
            _ballService = ballService;
            Debug.Log("BALL INJECTED");
        }

        private void Start()
        {
            if (!IsServer) return;
            _roundManager.OnReplayEvent += PlayReplay;
            _turnManager.OnTurnEndedEvent += SaveReplay;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void SaveReplay() => _ballService.SaveReplay(_rigidbody, _replayPosition, out _replayPosition);

        private void PlayReplay() => _ballService.PlayReplay(_rigidbody, _replayPosition, transform);

        public override void OnDestroy()
        {
            base.OnDestroy();
            _roundManager.OnReplayEvent -= PlayReplay;
            _turnManager.OnTurnEndedEvent -= SaveReplay;
        }
    }
}