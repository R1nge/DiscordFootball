using Manager.GamePlay;
using Services;
using Unity.Netcode;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class FormationUI : NetworkBehaviour
    {
        private FormationUIService _formationUIService;
        private VisualElement _root;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(RoundManager roundManager, FormationUIService formationUIService)
        {
            _roundManager = roundManager;
            _formationUIService = formationUIService;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
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

        private void Start()
        {
            //_roundManager.OnPreStartEvent += EnableUI;
            EnableUI();
        }

        private void EnableUI()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectServerRpc(int index, ServerRpcParams rpcParams = default)
        {
            //Await never returns
            _formationUIService.SelectFormation(index, rpcParams.Receive.SenderClientId);
            //_root.style.display = DisplayStyle.None;
        }

        private void OnButtonClicked()
        {
            _root.style.display = DisplayStyle.None;
        }

        public override void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= EnableUI;
        }
    }
}