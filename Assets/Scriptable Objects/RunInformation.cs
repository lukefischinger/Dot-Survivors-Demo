using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RunInformation : ScriptableObject
{
    public string weapon;

    public float damage;
    public float damageWhite;
    public float damageRed;
    public float damageBlue;
    public float damageYellow;
    public float enemiesKilled;
    public float healing;

    public int level;
    public List<string> upgrades;
    public List<int> upgradeLevels;


    public void ClearAll()
    {
        weapon = "";
        damage = 0;
        damageWhite = 0;
        damageRed = 0;
        damageBlue = 0;
        damageYellow = 0;
        enemiesKilled = 0;
        healing = 0;

    }

}
