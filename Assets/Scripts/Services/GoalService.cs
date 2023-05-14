using System;
using Manager.GamePlay;
using UnityEngine;
using VContainer;

namespace Services
{
    public class GoalService
    {
        private readonly RoundManager _roundManager;

        [Inject]
        public GoalService(RoundManager roundManager)
        {
            _roundManager = roundManager;
        }

        public void AddGoal(Roles role)
        {
            if (_roundManager.IsReplay()) return;

            switch (role)
            {
                case Roles.Red:
                    _roundManager.EndRound(Roles.Blue);
                    break;
                case Roles.Blue:
                    _roundManager.EndRound(Roles.Red);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}