using Zenject;

namespace GamePlay
{
    public class RoundManager
    {
        private TurnManager _turnManager;

        [Inject]
        private void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public void PreStartRound()
        {
            //TODO: formation selection
        }

        public void StartRound()
        {
            //TODO: spawn a ball
            _turnManager.FindAllRigidbodies();
            _turnManager.StartTimer();
        }

        public void EndRound()
        {
            //TODO: add score, show a replay
            _turnManager.StopTimer();
        }

        public void ShowReplay()
        {
        }
    }
}