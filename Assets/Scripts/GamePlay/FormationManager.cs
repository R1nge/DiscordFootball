using System;
using Player;
using UnityEngine;

namespace GamePlay
{
    public class FormationManager : MonoBehaviour
    {
        [Serializable]
        public struct Positions
        {
            public Transform[] positionsArray;
        }

        [SerializeField] private Positions[] positions;
        private PlayerSpawner _playerSpawner;


        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            SelectFormation(0);
        }

        public void SelectFormation(int index)
        {
            Vector3[] positionsVectors = new Vector3[5];

            for (int i = 0; i < positionsVectors.Length; i++)
            {
                positionsVectors[i] = positions[index].positionsArray[i].position;
            }

            //Remove players
            //Can make players of opponent's team invisible, then delete with exclusion, make visible
            var players = FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (int i = 0; i < players.Length; i++)
            {
                Destroy(players[i].gameObject);
            }

            StartCoroutine(_playerSpawner.SpawnPlayer(Teams.Red, positionsVectors));
        }
    }
}