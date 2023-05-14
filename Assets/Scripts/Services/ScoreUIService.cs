using System;
using Manager.GamePlay;
using UnityEngine.UIElements;

namespace Services
{
    public class ScoreUIService
    {
        private readonly ScoreManager _scoreManager;
        
        public void UpdateUI(Label teamLeft, Label teamRight, Team team, byte score)
        {
            switch (team.Roles)
            {
                case Roles.Red:
                    teamRight.text = score.ToString();
                    break;
                case Roles.Blue:
                    teamLeft.text = score.ToString();
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}