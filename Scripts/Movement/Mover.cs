using Tears_Of_Void.Core;
using Tears_Of_Void.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tears_Of_Void.Combat;

namespace Tears_Of_Void.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        AIHealth aiHealth;
        CharacterController controller;

        private void Awake()
        {
            aiHealth = GetComponent<AIHealth>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            controller = GetComponent<CharacterController>();
        }
        void Update()
        {
            navMeshAgent.enabled = !aiHealth.IsDead();
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFaction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFaction);

        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void MoveTo(Vector3 destination, float speedFaction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFaction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
