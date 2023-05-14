using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Manager.GamePlay
{
    public class TeamManager
    {
        private readonly Dictionary<ulong, Team> _teamsDictionary = new();

        public void SelectTeam(Team team)
        {
            var localId = NetworkManager.Singleton.LocalClientId;
            if (_teamsDictionary.ContainsKey(localId))
            {
                _teamsDictionary[localId] = team;
            }
            else
            {
                _teamsDictionary.TryAdd(localId, team);
            }
        }

        public Team GetTeam(ulong playerId)
        {
            if (_teamsDictionary.TryGetValue(playerId, out Team team))
            {
                return team;
            }

            return null;
        }

        public Team[] GetAllTeams() => _teamsDictionary.Values.ToArray();

        public bool CheckTeam(ulong playerId, Roles role)
        {
            if (!_teamsDictionary.ContainsKey(playerId)) return false;

            if (_teamsDictionary.TryGetValue(playerId, out Team team))
            {
                return team.Roles == role;
            }

            return false;
        }
    }
}