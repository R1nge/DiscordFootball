using GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

//TODO: drag towards mouse, watch in drag direction, sent to server after time is up 

namespace Player
{
    public class PlayerMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private float swipeResistancePercent;
        [SerializeField] private float pushForce;
        [SerializeField] private Teams _team;
        private InputAction _position, _press;
        private TurnManager _turnManager;
        private TeamManager _teamManager;
        private Vector2 _initialPosition;
        private Vector3 _movePosition;
        private Rigidbody _rigidbody;
        private bool _isSelected;

        [Inject]
        private void Construct(TurnManager turnManager, TeamManager teamManager)
        {
            _turnManager = turnManager;
            _teamManager = teamManager;
        }

        public void SetTeam(Teams team) => _team = team;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake()
        {
            var map = actions.FindActionMap("Gameplay");
            _position = map.FindAction("Position");
            _press = map.FindAction("Press");
            _press.performed += SetInitialPosition;
            _press.canceled += DetectSwipe;
            _turnManager.OnTurnEndedEvent += ProceedAction;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void SetInitialPosition(InputAction.CallbackContext callback) => _initialPosition = CurrentPosition();

        private Vector2 CurrentPosition() => _position.ReadValue<Vector2>();

        private void DetectSwipe(InputAction.CallbackContext callback)
        {
            if (!_isSelected)
            {
                return;
            }

            if (!_teamManager.CheckTeam(NetworkManager.Singleton.LocalClientId, _team))
            {
                return;
            }

            var delta = CurrentPosition() - _initialPosition;
            var direction = Vector3.zero;

            if (Mathf.Abs(delta.x / Screen.width) * 100 > swipeResistancePercent)
            {
                direction.x = Mathf.Clamp(delta.x, -1, 1);
            }

            if (Mathf.Abs(delta.y / Screen.height) * 100 > swipeResistancePercent)
            {
                direction.z = Mathf.Clamp(delta.y, -1, 1);
            }

            if (direction != Vector3.zero)
            {
                MakeAction(direction);
            }
        }

        private void MakeAction(Vector3 direction) => _movePosition = direction;

        private void ProceedAction()
        {
            if (_movePosition == Vector3.zero) return;
            _rigidbody.AddForce(_movePosition * pushForce, ForceMode.Force);
            _movePosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            _press.performed -= SetInitialPosition;
            _press.canceled -= DetectSwipe;
            _turnManager.OnTurnEndedEvent -= ProceedAction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSelected = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isSelected = false;
        }
    }
}