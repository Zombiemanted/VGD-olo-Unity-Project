using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    //Script is a lie this is just my bullethell controller (Using a waypoint system itll send out Things.. Dodge them)
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.destination = GameObject.Find("Player").transform.position;
    }
}
