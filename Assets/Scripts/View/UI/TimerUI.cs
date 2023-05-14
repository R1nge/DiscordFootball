using Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace View.UI
{
    public class TimerUI : MonoBehaviour
    {
        private VisualElement _root;
        private Label _time;
        private TimerUIService _timerUIService;

        [Inject]
        private void Construct(TimerUIService timerUIService)
        {
            _timerUIService = timerUIService;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _time = _root.Q<Label>("RemainingTime");
            NetworkManager.Singleton.NetworkTickSystem.Tick += UpdateUI;
        }

        private void UpdateUI() => _timerUIService.UpdateUI(_time);

        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;
            NetworkManager.Singleton.NetworkTickSystem.Tick -= UpdateUI;
        }
    }
}