using System;
using GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class FormationUI : MonoBehaviour
    {
        private FormationManager _formationManager;
        private VisualElement _root;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(RoundManager roundManager)
        {
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _formationManager = FindObjectOfType<FormationManager>();
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.style.display = DisplayStyle.None;
            _roundManager.OnPreStartEvent += EnableUI;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Select1").clicked += () => { Select(0); };
            _root.Q<Button>("Select2").clicked += () => { Select(1); };
            _root.Q<Button>("Select3").clicked += () => { Select(2); };
        }

        private void EnableUI()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        private async void Select(int index)
        {
            await _formationManager.SelectFormation(index, NetworkManager.Singleton.LocalClientId);
            _root.style.display = DisplayStyle.None;
        }

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= EnableUI;
        }
    }
}