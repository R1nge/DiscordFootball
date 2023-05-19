using Manager.GamePlay;
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
        for (var i = 0; i < _teamManager.GetAllTeams().Length; i++)
        {
            Debug.LogError(_teamManager.GetAllTeams()[i].Roles);
        }
    }
}