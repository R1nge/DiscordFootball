using Manager.GamePlay;
using UnityEngine.UIElements;
using VContainer;

namespace Services
{
    public class TimerUIService
    {
        private readonly TurnManager _turnManager;
        
        [Inject]
        public TimerUIService(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public void UpdateUI(Label time)
        {
            time.text = _turnManager.GetRemainingTime().ToString("#");
        }
    }
}