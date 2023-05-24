using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Manager.GamePlay
{
    public class PowerupsSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject[] powerups;
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
            print("Spawn powerup");
            if (Random.value > SpawnPercent) return;
            var index = Random.Range(0, powerups.Length);
            //TODO: spawn at random position
            var powerup = _objectResolver.Instantiate(powerups[index]);
            powerup.GetComponent<NetworkObject>().Spawn();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _turnManager.OnTurnEndedEvent -= SpawnPowerup;
        }
    }
}