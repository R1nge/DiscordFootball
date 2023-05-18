using System;
using Cysharp.Threading.Tasks;
using Manager.GamePlay;
using Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class FormationUI : NetworkBehaviour
    {
        private VisualElement _root;
        private RoundManager _roundManager;
        private FormationUIService _formationUIService;

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
            _root.Q<Button>("Select1").clicked += () => { SelectServerRpc(0); };
            _root.Q<Button>("Select2").clicked += () => { SelectServerRpc(1); };
            _root.Q<Button>("Select3").clicked += () => { SelectServerRpc(2); };
        }

        private void Start()
        {
            _roundManager.OnPreStartEvent += EnableUI;
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

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= EnableUI;
        }
    }
}