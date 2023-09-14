using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeQueue : ScriptableObject
{
    public List<string> upgradeNames = new List<string>();
}
