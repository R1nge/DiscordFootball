using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class TeamManagerUI : NetworkBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement _root;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        private void Awake()
        {
            _root = uiDocument.rootVisualElement;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Red").clicked += () =>
            {
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
            _root.Q<Button>("Start").clicked += () =>
            {
                _root.style.display = DisplayStyle.None;
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Additive);
                NetworkManager.Singleton.SceneManager.OnLoadComplete += (id, sceneName, mode) => { SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); };
            };
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectTeamServerRpc(string teamName, Roles role, ServerRpcParams rpcParams = default)
        {
            _teamManager.SelectTeam(new Team(teamName, role), rpcParams.Receive.SenderClientId);
        }

        private void OnButtonPressed()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer) return;
            _root.style.display = DisplayStyle.None;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += (id, sceneName, mode) => { SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); };
        }
    }
}