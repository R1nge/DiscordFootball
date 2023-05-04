using GamePlay;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private TurnManager _turnManager;

        [Inject]
        private void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
            print(turnManager);
        }

        private void Awake()
        {
            _turnManager.OnTurnEndedEvent += ProceedAction;
        }

        private void ProceedAction()
        {
            print("action");
        }

        private void OnDestroy()
        {
            _turnManager.OnTurnEndedEvent -= ProceedAction;
        }
    }
}