﻿using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using View.Player;

namespace Manager.GamePlay
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerTeam playerPrefab;
        private readonly NetworkVariable<int> _playersAmount = new();
        private IObjectResolver _objectResolver;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(IObjectResolver objectResolver, RoundManager roundManager)
        {
            _objectResolver = objectResolver;
            _roundManager = roundManager;
        }

        public void SpawnPlayers(Roles role, Vector3[] position, ulong ownerID)
        {
            for (var i = 0; i < position.Length; i++)
            {
                if (role == Roles.Blue)
                {
                    position[i].x *= -1;
                }

                _playersAmount.Value++;
                var player = _objectResolver.Instantiate(playerPrefab.gameObject, position[i], quaternion.identity);
                player.GetComponent<PlayerTeam>().SetTeam(role);
                //TODO: spawn with ownership
                player.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID);
            }

            if (BothTeamsReady())
            {
                _roundManager.StartRound();
                _playersAmount.Value = 0;
            }
        }

        private bool BothTeamsReady() => _playersAmount.Value == 8;
    }
}