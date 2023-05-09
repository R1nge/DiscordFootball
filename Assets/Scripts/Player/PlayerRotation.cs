using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private LayerMask ignore;
        private PlayerSwipe _playerSwipe;
        private Camera _camera;
        private float _rotationDelta;

        private void Awake()
        {
            _playerSwipe = GetComponent<PlayerSwipe>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_playerSwipe.IsSelected())
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~ignore))
                {
                    LookAtTarget(hit.point, 1f, Vector3.up);
                }
            }
        }

        private void LookAtTarget(Vector3 worldPoint, float duration, Vector3 upAxis)
        {
            Quaternion startRot = transform.rotation;
            Quaternion endRot = Quaternion.LookRotation(worldPoint - transform.position, upAxis);
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