using System.Linq;
using UnityEngine;

namespace Manager.GamePlay
{
    public class RigidbodiesManager : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private readonly Vector3 _zeroish = new(0.01f, 0.01f, 0.01f);

        public bool HaveRigidbodiesStopped()
        {
            if (_rigidbodies == null) return false;
            if (_rigidbodies.Length == 0) return false;
            if (_rigidbodies[0] == null) return false;
            return _rigidbodies.All(rb => (rb.velocity - _zeroish).magnitude <= 0.1f);
        }

        public void DeleteAllRigidbodies()
        {
            if (_rigidbodies == null) return;

            for (var i = _rigidbodies.Length - 1; i >= 0; i--)
            {
                Destroy(_rigidbodies[i].gameObject);
            }

            _rigidbodies = null;
        }

        public void FindAllRigidbodies()
        {
            _rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }
    }
}