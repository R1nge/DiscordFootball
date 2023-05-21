using System;
using Manager.GamePlay;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class ScoreUI : MonoBehaviour
    {
        private VisualElement _root;
        private Label _teamLeft, _teamRight;
        private ScoreManager _scoreManager;

        [Inject]
        private void Construct(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _teamLeft = _root.Q<Label>("TeamLeft");
            _teamRight = _root.Q<Label>("TeamRight");
            _scoreManager.OnScoreChanged += UpdateUI;
        }

        private void UpdateUI(Roles team, byte score)
        {
            switch (team)
            {
                case Roles.Red:
                    _teamRight.text = score.ToString();
                    break;
                case Roles.Blue:
                    _teamLeft.text = score.ToString();
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= UpdateUI;
        }
    }
}