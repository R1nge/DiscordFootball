using System;
using GamePlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FormationUI : MonoBehaviour
    {
        private FormationManager _formationManager;
        private VisualElement _root;

        private void OnEnable()
        {
            _root.Q<Button>("Select1").clicked += () => { Select(0); };
            _root.Q<Button>("Select2").clicked += () => { Select(1); };
            _root.Q<Button>("Select3").clicked += () => { Select(2); };
        }

        private void Awake()
        {
            _formationManager = FindObjectOfType<FormationManager>();
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.Q<Button>("Select1").clicked += () => { Select(0); };
            _root.Q<Button>("Select2").clicked += () => { Select(1); };
            _root.Q<Button>("Select3").clicked += () => { Select(2); };
            //TODO: make UI (formation selection)
        }

        private void Select(int index)
        {
            _formationManager.SelectFormation(index);
        }
    }
}