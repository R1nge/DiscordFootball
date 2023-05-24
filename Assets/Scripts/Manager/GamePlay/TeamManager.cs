using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.GamePlay
{
    public class TeamManager
    {
        private readonly Dictionary<ulong, Team> _teamsDictionary = new();

        public void SelectTeam(Team team, ulong playerId)
        {
            if (_teamsDictionary.ContainsKey(playerId))
            {
                _teamsDictionary[playerId] = team;
            }
            else
            {
                _teamsDictionary.TryAdd(playerId, team);
            }
        }

        public Team GetTeam(ulong playerId)
        {
            if (_teamsDictionary.TryGetValue(playerId, out var team))
            {
                return team;
            }

            return null;
        }

        public Team GetOpponentTeamByRole(Roles role)
        {
            switch (role)
            {
                case Roles.Red:
                    return _teamsDictionary.Values.ToArray().First(t => t.Roles == Roles.Blue);
                case Roles.Blue:
                    return _teamsDictionary.Values.ToArray().First(t => t.Roles == Roles.Red);
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public Team[] GetAllTeams() => _teamsDictionary.Values.ToArray();

        public bool CheckTeam(ulong playerId, Roles role)
        {
            if (!_teamsDictionary.ContainsKey(playerId))
            {
                return false;
            }

            if (_teamsDictionary.TryGetValue(playerId, out var team))
            {
                return team.Roles == role;
            }

            return false;
        }
    }
}