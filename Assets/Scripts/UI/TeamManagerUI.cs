using System;
using GamePlay;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class TeamManagerUI : MonoBehaviour
    {
        private TeamManager _teamManager;
        private UIDocument _document;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            var root = _document.rootVisualElement;
            root.Q<Button>("Red").clicked += () =>
            {
                _teamManager.SelectTeam(Teams.Red);
                root.style.display = DisplayStyle.None;
            };
            root.Q<Button>("Blue").clicked += () =>
            {
                _teamManager.SelectTeam(Teams.Blue);
                root.style.display = DisplayStyle.None;
            };
            root.Q<Button>("Spectator").clicked += () =>
            {
                _teamManager.SelectTeam(Teams.Spectator);
                root.style.display = DisplayStyle.None;
            };
        }
    }
}