using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager.GamePlay
{
    public class TeamManager
    {
        private readonly Dictionary<ulong, Team> _teamsDictionary = new();

        public TeamManager()
        {
            Debug.LogWarning("TEAM MANAGER CONSTRUCTED");
        }

        public void SelectTeam(Team team, ulong playerId)
        {
            if (_teamsDictionary.ContainsKey(playerId))
                _teamsDictionary[playerId] = team;
            else
                _teamsDictionary.TryAdd(playerId, team);
        }

        public Team GetTeam(ulong playerId)
        {
            if (_teamsDictionary.TryGetValue(playerId, out var team)) return team;

            return null;
        }

        public Team[] GetAllTeams()
        {
            return _teamsDictionary.Values.ToArray();
        }

        public bool CheckTeam(ulong playerId, Roles role)
        {
            if (!_teamsDictionary.ContainsKey(playerId)) return false;

            if (_teamsDictionary.TryGetValue(playerId, out var team)) return team.Roles == role;

            return false;
        }
    }
}