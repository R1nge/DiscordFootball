using Manager.GamePlay;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class TimerUI : MonoBehaviour
    {
        private VisualElement _root;
        private Label _time;
        private TurnManager _turnManager;

        [Inject]
        private void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _time = _root.Q<Label>("RemainingTime");
            _turnManager.GetRemainingTime().OnValueChanged += UpdateUI;
        }

        private void UpdateUI(float oldValue, float newValue)
        {
            _time.text = newValue.ToString("#");
        }

        private void OnDestroy()
        {
            _turnManager.GetRemainingTime().OnValueChanged -= UpdateUI;
        }
    }
}