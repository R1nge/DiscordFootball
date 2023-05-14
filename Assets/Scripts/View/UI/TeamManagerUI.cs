using Services;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class TeamManagerUI : MonoBehaviour
    {
        private VisualElement _root;
        private TeamManagerUIService _teamManagerUIService;
        
        [Inject]
        private void Construct(TeamManagerUIService teamManagerUIService)
        {
            _teamManagerUIService = teamManagerUIService;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Red").clicked += () =>
            {
                _teamManagerUIService.SelectTeam(new Team("Red", Roles.Red));
                OnButtonPressed();
            };
            _root.Q<Button>("Blue").clicked += () =>
            {
                _teamManagerUIService.SelectTeam(new Team("Blue", Roles.Blue));
                OnButtonPressed();
            };
            _root.Q<Button>("Spectator").clicked += () =>
            {
                _teamManagerUIService.SelectTeam(new Team("Spectator", Roles.Spectator));
                OnButtonPressed();
            };
        }

        private void OnButtonPressed()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}