using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace View.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        private Vector3 _movePosition;
        private PlayerSwipe _playerSwipe;
        private Vector3 _positionReplay, _movePositionReplay;
        private Rigidbody _rigidbody;
        private RoundManager _roundManager;
        private TurnManager _turnManager;
        [SerializeField] private float forceClamp;
        [SerializeField] private float pushForce;

        [Inject]
        private void Construct(TurnManager turnManager, RoundManager roundManager)
        {
            _turnManager = turnManager;
            _roundManager = roundManager;
        }

        private void Start()
        {
            if (IsServer)
            {
                _turnManager.OnTurnEndedEvent += SaveReplay;
                _turnManager.OnTurnEndedEvent += ProceedAction;
                _roundManager.OnReplayEvent += PlayRound;
            }

            _playerSwipe = GetComponent<PlayerSwipe>();
            _playerSwipe.OnSwipedEvent += MakeAction;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void MakeAction(Vector3 direction)
        {
            MakeActionServerRpc(direction);
        }

        [ServerRpc(RequireOwnership = false)]
        private void MakeActionServerRpc(Vector3 direction)
        {
            _movePosition = direction;
        }

        private void ProceedAction()
        {
            if (_movePosition == Vector3.zero) return;
            var force = _movePosition * Mathf.Clamp(pushForce, 0f, forceClamp);
            _rigidbody.AddForce(force, ForceMode.Force);
            _movePosition = Vector3.zero;
        }

        private void SaveReplay()
        {
            _positionReplay = transform.position;
            _movePositionReplay = _movePosition;
        }

        private void PlayRound()
        {
            transform.position = _positionReplay;
            _movePosition = _movePositionReplay;
            ProceedAction();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _playerSwipe.OnSwipedEvent -= MakeAction;
            _turnManager.OnTurnEndedEvent -= SaveReplay;
            _turnManager.OnTurnEndedEvent -= ProceedAction;
            _roundManager.OnReplayEvent -= PlayRound;
        }
    }
}