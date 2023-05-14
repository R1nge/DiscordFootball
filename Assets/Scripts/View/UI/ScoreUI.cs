using Manager.GamePlay;
using Services;
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
        private ScoreUIService _scoreUIService;

        [Inject]
        private void Construct(ScoreManager scoreManager, ScoreUIService scoreUIService)
        {
            _scoreManager = scoreManager;
            _scoreUIService = scoreUIService;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _teamLeft = _root.Q<Label>("TeamLeft");
            _teamRight = _root.Q<Label>("TeamRight");
            _scoreManager.OnScoreChanged += UpdateUI;
        }

        private void UpdateUI(Team team, byte score)
        {
            _scoreUIService.UpdateUI(_teamLeft, _teamRight, team, score);
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= UpdateUI;
        }
    }
}