using Manager.GamePlay;
using Manager.Powerups;
using Scriptables;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace View.Powerups
{
    public class PowerupViewBase : NetworkBehaviour
    {
        [SerializeField] protected PowerupDataSO powerupDataSo;
        protected Powerup Powerup;
        protected IObjectResolver ObjectResolver;
        private PowerupManager _powerupManager;

        [Inject]
        private void Construct(IObjectResolver objectResolver, PowerupManager powerupManager)
        {
            ObjectResolver = objectResolver;
            _powerupManager = powerupManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out NetworkObject networkObject))
            {
                PickupServerRpc(networkObject.OwnerClientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PickupServerRpc(ulong playerID) => Pickup(playerID);

        private void Pickup(ulong playerID)
        {
            _powerupManager.AddPowerup(playerID, Powerup);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}