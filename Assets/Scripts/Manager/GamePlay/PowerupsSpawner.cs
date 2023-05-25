using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Manager.GamePlay
{
    public class PowerupsSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject[] powerupPickups;
        [SerializeField] private Transform[] spawnPositions;
        private const float SpawnPercent = 0.25f;
        private IObjectResolver _objectResolver;
        private TurnManager _turnManager;

        [Inject]
        private void Construct(IObjectResolver objectResolver, TurnManager turnManager)
        {
            _objectResolver = objectResolver;
            _turnManager = turnManager;
        }

        public void Awake()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _turnManager.OnTurnEndedEvent += SpawnPowerup;
        }

        private void SpawnPowerup()
        {
            //TODO: set spawn positions
            print("Spawn powerup");
            if (Random.value > SpawnPercent) return;
            var index = Random.Range(0, powerupPickups.Length);
            var spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
            var powerup = _objectResolver.Instantiate(powerupPickups[index], spawnPosition.position, Quaternion.identity);
            powerup.GetComponent<NetworkObject>().Spawn();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _turnManager.OnTurnEndedEvent -= SpawnPowerup;
        }
    }
}