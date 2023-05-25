using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class PowerupUI : NetworkBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private Button _icon;
        private PowerupManager _powerupManager;
        private PowerupsDataManager _powerupsDataManager;

        [Inject]
        private void Construct(PowerupManager powerupManager, PowerupsDataManager powerupsDataManager)
        {
            _powerupManager = powerupManager;
            _powerupsDataManager = powerupsDataManager;
        }

        private void OnEnable()
        {
            _icon.clicked += () => { UsePowerupServerRpc(); };
        }

        private void Awake()
        {
            _powerupManager.OnPowerupChangedEvent += PowerupChanged;
            _powerupManager.OnPowerupUsedEvent += OnPowerupUsed;
            _icon = uiDocument.rootVisualElement.Q<Button>("powerup-icon");
            _icon.style.visibility = Visibility.Hidden;
        }

        [ServerRpc(RequireOwnership = false)]
        private void UsePowerupServerRpc(ServerRpcParams rpcParams = default)
        {
            _powerupManager.UsePowerup(rpcParams.Receive.SenderClientId);
        }

        private void PowerupChanged(string powerupName, ulong playerId)
        {
            var rpcParams = new ClientRpcParams
            {
                Send = new()
                {
                    TargetClientIds = new[] { playerId }
                }
            };

            PowerupAddedClientRpc(powerupName, rpcParams);
        }

        [ClientRpc]
        private void PowerupAddedClientRpc(string powerupName, ClientRpcParams _ = default)
        {
            _icon.style.visibility = Visibility.Visible;
            _icon.style.backgroundImage = new(_powerupsDataManager.GetPowerupData(powerupName).Icon);
        }

        private void OnPowerupUsed(ulong playerId)
        {
            var rpcParams = new ClientRpcParams
            {
                Send = new()
                {
                    TargetClientIds = new[] { playerId }
                }
            };

            PowerupUsedClientRpc(rpcParams);
        }

        [ClientRpc]
        private void PowerupUsedClientRpc(ClientRpcParams _ = default)
        {
            _icon.style.visibility = Visibility.Hidden;
            _icon.style.backgroundImage = null;
        }
    }
}