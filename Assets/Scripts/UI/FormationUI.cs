using GamePlay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FormationUI : MonoBehaviour
    {
        private FormationManager _formationManager;
        private VisualElement _root;

        private void Awake()
        {
            _formationManager = FindObjectOfType<FormationManager>();
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void OnEnable()
        {
            _root.Q<Button>("Select1").clicked += () => { Select(0); };
            _root.Q<Button>("Select2").clicked += () => { Select(1); };
            _root.Q<Button>("Select3").clicked += () => { Select(2); };
        }

        private void Select(int index)
        {
            _formationManager.SelectFormation(index, NetworkManager.Singleton.LocalClientId);
            _root.style.display = DisplayStyle.None;
        }
    }
}