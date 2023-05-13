using System.Linq;
using UnityEngine;

namespace GamePlay
{
    public class RigidbodiesManager : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        
        public bool HaveRigidbodiesStopped()
        {
            if (_rigidbodies == null) return false;
            if (_rigidbodies.Length == 0) return false;
            if (_rigidbodies[0] == null) return false;
            return _rigidbodies.All(rb => rb.velocity == Vector3.zero);
        }

        public void DeleteAllRigidbodies()
        {
            if (_rigidbodies == null) return;
            for (int i = _rigidbodies.Length - 1; i >= 0; i--)
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