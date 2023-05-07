using System.Collections.Generic;
using Unity.Netcode;

namespace GamePlay
{
    public class TeamManager
    {
        private readonly Dictionary<ulong, Teams> _teamsDictionary = new();

        public void SelectTeam(Teams team)
        {
            var localId = NetworkManager.Singleton.LocalClientId;
            if (_teamsDictionary.ContainsKey(localId))
            {
                _teamsDictionary[localId] = team;
            }
            else
            {
                _teamsDictionary.Add(localId, team);
            }
        }

        public Teams? GetTeam(ulong playerId)
        {
            if (_teamsDictionary.TryGetValue(playerId, out Teams team))
            {
                return team;
            }

            return null;
        }

        public bool CheckTeam(ulong playerId, Teams team)
        {
            if (!_teamsDictionary.ContainsKey(NetworkManager.Singleton.LocalClientId)) return false;

            if (_teamsDictionary.TryGetValue(playerId, out Teams value))
            {
                return team == value;
            }

            return false;
        }
    }
}