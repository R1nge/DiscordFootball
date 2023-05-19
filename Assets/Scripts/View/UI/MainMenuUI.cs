using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace View.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        private TextField _ipField;
        private VisualElement _root;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _ipField = _root.Q<TextField>("IP");
        }

        private void OnEnable()
        {
            _root.Q<Button>("Host").clicked += () =>
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = _ipField.value;
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.SceneManager.LoadScene("TeamSelection", LoadSceneMode.Single);
            };
            _root.Q<Button>("Join").clicked += () =>
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = _ipField.value;
                NetworkManager.Singleton.StartClient();
            };
        }
    }
}