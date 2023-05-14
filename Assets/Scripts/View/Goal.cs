using System;
using Services;
using UnityEngine;
using VContainer;

namespace View
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private Roles role;
        private GoalService _goalService;

        [Inject]
        private void Construct(GoalService goalService)
        {
            _goalService = goalService;
        }

        private void Start()
        {
            if (_goalService != null)
            {
                Debug.Log("GoalService");
            }
            else
            {
                Debug.Log("No GoalService");
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;

            if (ball.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.drag = 20f;
            }

            _goalService.AddGoal(role);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Ball ball)) return;

            if (ball.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.drag = .5f;
            }
        }
    }
}