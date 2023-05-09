using System;
using GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class PlayerSwipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector3> OnSwipedEvent;
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private float swipeResistancePercent;
        [SerializeField] private float deltaDivider;
        private bool _isSelected;
        private InputAction _position, _press;
        private TeamManager _teamManager;
        private PlayerTeam _playerTeam;
        private Vector2 _initialPosition;

        public bool IsSelected() => _isSelected;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        private void Awake()
        {
            _playerTeam = GetComponent<PlayerTeam>();
            var map = actions.FindActionMap("Gameplay");
            _position = map.FindAction("Position");
            _press = map.FindAction("Press");
            _press.performed += SetInitialPosition;
            _press.canceled += DetectSwipe;
        }

        private void OnEnable() => actions.Enable();

        private void SetInitialPosition(InputAction.CallbackContext callback) => _initialPosition = CurrentPosition();

        private Vector2 CurrentPosition() => _position.ReadValue<Vector2>();

        private void DetectSwipe(InputAction.CallbackContext callback)
        {
            if (!_isSelected)
            {
                return;
            }

            var localId = NetworkManager.Singleton.LocalClientId;
            if (!_teamManager.CheckTeam(localId, _playerTeam.GetTeam()))
            {
                return;
            }

            var delta = new Vector2(
                (CurrentPosition().x - _initialPosition.x) / deltaDivider,
                (CurrentPosition().y - _initialPosition.y) / deltaDivider
            );

            var direction = Vector3.zero;

            if (Mathf.Abs(delta.x) * 100 > swipeResistancePercent)
            {
                direction.x = Mathf.Clamp(delta.x, -1f, 1f);
            }

            if (Mathf.Abs(delta.y) * 100 > swipeResistancePercent)
            {
                direction.z = Mathf.Clamp(delta.y, -1f, 1f);
            }

            print(delta);
            print(direction);

            if (direction != Vector3.zero)
            {
                OnSwipedEvent?.Invoke(direction);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSelected = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isSelected = false;
        }

        private void OnDisable() => actions.Disable();

        private void OnDestroy()
        {
            _press.performed -= SetInitialPosition;
            _press.canceled -= DetectSwipe;
        }
    }
}