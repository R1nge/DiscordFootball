using Manager.GamePlay;
using Unity.Netcode;
using VContainer;

namespace View
{
    public class ScoreManagerView : NetworkBehaviour
    {
        private RoundManager _roundManager;
        private ScoreManager _scoreManager;

        [Inject]
        private void Construct(RoundManager roundManager, ScoreManager scoreManager)
        {
            _roundManager = roundManager;
            _scoreManager = scoreManager;
        }

        private void Awake() => _roundManager.OnEndEvent += OnRoundEndServerRpc;

        [ServerRpc(RequireOwnership = false)]
        private void OnRoundEndServerRpc()
        {
            var teamWon = _roundManager.GetLastWonTeam();
            _scoreManager.OnRoundEnd(teamWon.Roles);
            OnRoundEndClientRpc(teamWon);
        }

        [ClientRpc]
        private void OnRoundEndClientRpc(Team teamWon)
        {
            _scoreManager.UpdateUIClient(teamWon);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _roundManager.OnEndEvent -= OnRoundEndServerRpc;
        }
    }
}