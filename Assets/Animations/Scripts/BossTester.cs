using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTester : MonoBehaviour
{
    public BossController boss;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) boss.ChangeState(BossState.Attack1);
        if (Input.GetKeyDown(KeyCode.Alpha6)) boss.ChangeState(BossState.Attack2);
        if (Input.GetKeyDown(KeyCode.Alpha7)) boss.ChangeState(BossState.Attack3);
        if (Input.GetKeyDown(KeyCode.Alpha8)) boss.ChangeState(BossState.Death);
    }
}