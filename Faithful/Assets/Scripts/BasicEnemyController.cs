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
    Animator myAnim;
    void Start()
    {
        myAnim = GetComponent<Animator>();
        //if myAnim.SetBool("isAttacking", true); else stop agent myAnim.SetBool("isAttacking",false);
    }

    void Update()
    {

    }
}
