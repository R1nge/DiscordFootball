using GamePlay;
using UnityEngine;
using Zenject;

//TODO: drag towards mouse, watch in drag direction, sent to server after time is up 

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float pushForce;
        [SerializeField] private float forceClamp;
        private PlayerSwipe _playerSwipe;
        private TurnManager _turnManager;
        private RoundManager _roundManager;
        private Rigidbody _rigidbody;
        private Vector3 _movePosition;
        private Vector3 _positionReplay, _movePositionReplay;

        [Inject]
        private void Construct(TurnManager turnManager, RoundManager roundManager)
        {
            _turnManager = turnManager;
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _playerSwipe = GetComponent<PlayerSwipe>();
            _playerSwipe.OnSwipedEvent += MakeAction;
            _turnManager.OnTurnEndedEvent += SaveReplay;
            _turnManager.OnTurnEndedEvent += ProceedAction;
            _roundManager.OnReplayEvent += PlayReplay;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void MakeAction(Vector3 direction) => _movePosition = direction;

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

        private void PlayReplay()
        {
            transform.position = _positionReplay;
            _movePosition = _movePositionReplay;
            ProceedAction();
        }

        private void OnDestroy()
        {
            _playerSwipe.OnSwipedEvent -= MakeAction;
            _turnManager.OnTurnEndedEvent -= ProceedAction;
        }
    }
}