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
    public class FormationUI : MonoBehaviour
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
            _root.Q<Button>("Select1").clicked += () => { Select(0); };
            _root.Q<Button>("Select2").clicked += () => { Select(1); };
            _root.Q<Button>("Select3").clicked += () => { Select(2); };
        }

        private void Start()
        {
            _roundManager.OnPreStartEvent += EnableUI;
        }

        private void EnableUI()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        private async void Select(int index)
        {
            //Await never returns
            _formationUIService.SelectFormation(index, NetworkManager.Singleton.LocalClientId);
            _root.style.display = DisplayStyle.None;
        }

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= EnableUI;
        }
    }
}