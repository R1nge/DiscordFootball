using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace View.Player
{
    public class PlayerRotation : NetworkBehaviour
    {
        [SerializeField] private LayerMask ignore;
        private Camera _camera;
        private PlayerSwipe _playerSwipe;
        private PlayerTeam _playerTeam;
        private float _rotationDelta;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        private void Awake()
        {
            NetworkManager.Singleton.NetworkTickSystem.Tick += Raycast;
            _playerSwipe = GetComponent<PlayerSwipe>();
            _playerTeam = GetComponent<PlayerTeam>();
            _camera = Camera.main;
        }

        private void Raycast()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~ignore))
            {
                if (_playerSwipe.IsSelected())
                {
                    LookAtTargetServerRpc(hit.point, 1f);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void LookAtTargetServerRpc(Vector3 hitPoint, float duration, ServerRpcParams rpcParams = default)
        {
            //TODO: fix team manager is null on clients
            if (!_teamManager.CheckTeam(rpcParams.Receive.SenderClientId, _playerTeam.GetTeam())) return;
            LookAtTarget(hitPoint, duration, Vector3.up);
        }

        private void LookAtTarget(Vector3 worldPoint, float duration, Vector3 upAxis)
        {
            var startRot = transform.rotation;
            var endRot = Quaternion.LookRotation(worldPoint - transform.position, upAxis);
            endRot.x = transform.rotation.x;
            endRot.z = transform.rotation.z;

            if (transform.rotation == endRot)
            {
                _rotationDelta = 0;
                return;
            }

            if (_rotationDelta < duration)
            {
                _rotationDelta += 1f / NetworkManager.Singleton.NetworkTickSystem.TickRate;
            }
            else
            {
                _rotationDelta = 0;
                transform.rotation = endRot;
                return;
            }

            transform.rotation = Quaternion.Slerp(startRot, endRot, _rotationDelta / duration);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!NetworkManager.Singleton) return;
            if (NetworkManager.Singleton.NetworkTickSystem == null) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= Raycast;
        }
    }
}