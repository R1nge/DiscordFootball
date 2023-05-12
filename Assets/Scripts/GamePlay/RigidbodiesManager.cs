﻿using System.Linq;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class RigidbodiesManager : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(RoundManager roundManager)
        {
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _roundManager.OnPreStartEvent += DeleteAllRigidbodies;
            _roundManager.OnStartEvent += FindAllRigidbodies;
        }

        //TODO: fix
        public bool HaveRigidbodiesStopped()
        {
            if (_rigidbodies == null) return false;
            if (_rigidbodies.Length == 0) return false;
            return _rigidbodies.All(rb => rb.velocity == Vector3.zero);
        }

        private void DeleteAllRigidbodies()
        {
            if (_rigidbodies == null) return;
            for (int i = _rigidbodies.Length - 1; i >= 0; i--)
            {
                Destroy(_rigidbodies[i].gameObject);
            }

            _rigidbodies = null;
        }

        private void FindAllRigidbodies()
        {
            _rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= DeleteAllRigidbodies;
            _roundManager.OnStartEvent -= FindAllRigidbodies;
        }
    }
}