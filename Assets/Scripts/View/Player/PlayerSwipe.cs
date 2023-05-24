using System;
using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

namespace View.Player
{
    public class PlayerSwipe : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector3> OnSwipedEvent;
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private float deltaDivider;
        [SerializeField] private float swipeResistancePercent;
        private Vector2 _initialPosition;
        private bool _isSelected;
        private PlayerTeam _playerTeam;
        private InputAction _position, _press;
        private TeamManager _teamManager;

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

        public void OnPointerDown(PointerEventData eventData) => _isSelected = true;

        public void OnPointerUp(PointerEventData eventData) => _isSelected = false;

        public bool IsSelected() => _isSelected;

        private void SetInitialPosition(InputAction.CallbackContext callback) => _initialPosition = CurrentPosition();

        private Vector2 CurrentPosition() => _position.ReadValue<Vector2>();

        //UI can block swipe
        //TODO: redo swipe detection
        //TODO: drag towards mouse, watch in drag direction, send server rpc after time is up 
        [ServerRpc(RequireOwnership = false)]
        private void DetectSwipeServerRpc(Vector3 direction, ServerRpcParams rpcParams = default)
        {
            var localId = rpcParams.Receive.SenderClientId;
            if (_teamManager.CheckTeam(localId, _playerTeam.GetTeam()))
            {
                print(direction);

                if (direction != Vector3.zero)
                {
                    OnSwipedEvent?.Invoke(direction);
                }
            }
            else
            {
                Debug.LogError(
                    $"PlayerSwipe: trying to control a character of the opposite team; {localId} : {_teamManager.GetTeam(localId)}",
                    this);
            }
        }

        private void DetectSwipe(InputAction.CallbackContext callback)
        {
            if (!_isSelected) return;

            var delta = new Vector2(
                (CurrentPosition().x - _initialPosition.x) / deltaDivider,
                (CurrentPosition().y - _initialPosition.y) / deltaDivider
            );

            print(delta);

            var direction = Vector3.zero;

            if (Mathf.Abs(delta.x) * 100 > swipeResistancePercent)
            {
                direction.x = Mathf.Clamp(delta.x, -1f, 1f);
            }

            if (Mathf.Abs(delta.y) * 100 > swipeResistancePercent)
            {
                direction.z = Mathf.Clamp(delta.y, -1f, 1f);
            }

            DetectSwipeServerRpc(direction);
        }

        private void OnDisable() => actions.Disable();

        public override void OnDestroy()
        {
            base.OnDestroy();
            _press.performed -= SetInitialPosition;
            _press.canceled -= DetectSwipe;
        }
    }
}