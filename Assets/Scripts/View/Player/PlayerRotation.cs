﻿using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace View.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        private Camera _camera;
        private PlayerSwipe _playerSwipe;
        private PlayerTeam _playerTeam;
        private float _rotationDelta;
        private TeamManager _teamManager;
        [SerializeField] private LayerMask ignore;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        private void Awake()
        {
            _playerSwipe = GetComponent<PlayerSwipe>();
            _playerTeam = GetComponent<PlayerTeam>();
            _camera = Camera.main;
        }

        private void Update()
        {
            LookAtTargetServerRpc();
        }

        [ServerRpc]
        private void LookAtTargetServerRpc(ServerRpcParams rpcParams = default)
        {
            if (!_teamManager.CheckTeam(rpcParams.Receive.SenderClientId, _playerTeam.GetTeam())) return;
            if (_playerSwipe.IsSelected())
            {
                var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~ignore))
                {
                    LookAtTarget(hit.point, 1f, Vector3.up);
                }
            }
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
                _rotationDelta += Time.deltaTime;
            }
            else
            {
                _rotationDelta = 0;
                transform.rotation = endRot;
                return;
            }

            transform.rotation = Quaternion.Slerp(startRot, endRot, _rotationDelta / duration);
        }
    }
}