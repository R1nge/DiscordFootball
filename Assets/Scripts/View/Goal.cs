using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace View
{
    public class Goal : NetworkBehaviour
    {
        [SerializeField] private Roles ownedBy;
        private RoundManager _roundManager;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(RoundManager roundManager, TeamManager teamManager)
        {
            _roundManager = roundManager;
            _teamManager = teamManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;

            if (!other.TryGetComponent(out Ball ball)) return;

            if (ball.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.drag = 20f;
            }

            AddGoal(_teamManager.GetOpponentTeamByRole(ownedBy));
        }

        private void AddGoal(Team teamWon)
        {
            if (_roundManager.IsReplay()) return;
            _roundManager.EndRound(teamWon);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsServer) return;

            if (!other.TryGetComponent(out Ball ball)) return;

            if (ball.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.drag = .5f;
            }
        }
    }
}