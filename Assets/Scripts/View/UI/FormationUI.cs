using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class FormationUI : NetworkBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement _root;
        private RoundManager _roundManager;
        private FormationManager _formationManager;

        [Inject]
        private void Construct(RoundManager roundManager, FormationManager formationManager)
        {
            _roundManager = roundManager;
            _formationManager = formationManager;
        }

        private void Awake()
        {
            _roundManager.OnPreStartEvent += EnableUI;
            _root = uiDocument.rootVisualElement;
            _root.style.display = DisplayStyle.None;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Select1").clicked += () =>
            {
                SelectServerRpc(0);
                OnButtonClicked();
            };
            _root.Q<Button>("Select2").clicked += () =>
            {
                SelectServerRpc(1);
                OnButtonClicked();
            };
            _root.Q<Button>("Select3").clicked += () =>
            {
                SelectServerRpc(2);
                OnButtonClicked();
            };
        }

        private void EnableUI()
        {
            if (!IsServer) return;
            _root.style.display = DisplayStyle.Flex;
            EnableUIClientRpc();
        }

        [ClientRpc]
        private void EnableUIClientRpc() => _root.style.display = DisplayStyle.Flex;

        [ServerRpc(RequireOwnership = false)]
        private void SelectServerRpc(int index, ServerRpcParams rpcParams = default)
        {
            _formationManager.SelectFormationServerRpc(index, rpcParams.Receive.SenderClientId);
        }

        private void OnButtonClicked() => _root.style.display = DisplayStyle.None;

        public override void OnDestroy()
        {
            base.OnDestroy();
            _roundManager.OnPreStartEvent -= EnableUI;
        }
    }
}