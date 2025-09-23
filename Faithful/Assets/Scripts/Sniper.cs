using UnityEngine;
using UnityEngine.InputSystem;

public class Sniper : Weapon
{
    public void toggleFireMode()
    {
        currentFireMode++;

        if (currentFireMode >= fireModes)
            currentFireMode = 0;

        if (currentFireMode == 0)
            rof = 2;
        else
            rof = 1f;
    }
}