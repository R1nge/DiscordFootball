using Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class TeamManagerUI : NetworkBehaviour
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
                //TODO: make it server rpc
                SelectTeamServerRpc("Red", Roles.Red);
                OnButtonPressed();
            };
            _root.Q<Button>("Blue").clicked += () =>
            {
                SelectTeamServerRpc("Blue", Roles.Blue);
                OnButtonPressed();
            };
            _root.Q<Button>("Spectator").clicked += () =>
            {
                SelectTeamServerRpc("Spectator", Roles.Spectator);
                OnButtonPressed();
            };
        }
        
        //TODO: move team selection to a separate scene
        [ServerRpc(RequireOwnership = false)]
        private void SelectTeamServerRpc(string teamName, Roles role)
        {
            _teamManagerUIService.SelectTeam(new Team(teamName, role));
        }

        private void OnButtonPressed()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}