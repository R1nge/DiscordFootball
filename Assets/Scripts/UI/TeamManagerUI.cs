using GamePlay;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    public class TeamManagerUI : MonoBehaviour
    {
        private VisualElement _root;
        private TeamManager _teamManager;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(TeamManager teamManager, RoundManager roundManager)
        {
            _teamManager = teamManager;
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Red").clicked += () =>
            {
                _teamManager.SelectTeam(new Team("Red", Roles.Red));
                OnButtonPressed();
            };
            _root.Q<Button>("Blue").clicked += () =>
            {
                _teamManager.SelectTeam(new Team("Blue", Roles.Blue));
                OnButtonPressed();
            };
            _root.Q<Button>("Spectator").clicked += () =>
            {
                _teamManager.SelectTeam(new Team("Spectator", Roles.Spectator));
                OnButtonPressed();
            };
        }

        private void OnButtonPressed()
        {
            _root.style.display = DisplayStyle.None;
            _roundManager.PreStartRound();
        }
    }
}