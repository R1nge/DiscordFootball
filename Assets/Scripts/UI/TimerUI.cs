using System;
using GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class TimerUI : MonoBehaviour
    {
        private TurnManager _turnManager;
        private VisualElement _root;
        private Label _time;

        [Inject]
        private void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _time = _root.Q<Label>("RemainingTime");
            NetworkManager.Singleton.NetworkTickSystem.Tick += UpdateUI;
        }

        private void UpdateUI()
        {
            _time.text = _turnManager.GetRemainingTime().ToString("#");
        }

        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= UpdateUI;
        }
    }
}