using Manager.GamePlay;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class TeamTest : MonoBehaviour
{
    private TeamManager _teamManager;

    [Inject]
    private void Construct(TeamManager teamManager)
    {
        _teamManager = teamManager;
    }

    private void Update()
    {
        Debug.LogError(_teamManager.GetAllTeams()[0].Roles);
    }
}