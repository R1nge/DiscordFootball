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
        private TurnManager _turnManager;
        private PlayerSwipe _playerSwipe;
        private Rigidbody _rigidbody;
        private Vector3 _movePosition;

        [Inject]
        private void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        private void Awake()
        {
            _playerSwipe = GetComponent<PlayerSwipe>();
            _playerSwipe.OnSwipedEvent += MakeAction;
            _turnManager.OnTurnEndedEvent += ProceedAction;
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

        private void OnDestroy()
        {
            _playerSwipe.OnSwipedEvent -= MakeAction;
            _turnManager.OnTurnEndedEvent -= ProceedAction;
        }
    }
}