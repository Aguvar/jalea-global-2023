using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancestor
{
    public string Name;
    public int BodyNum;
    public int ArmorNum;
    public int HelmetNum;
    public int DifficultyModifier;
    public int Iteration;
    public List<Buff> Buffs = new List<Buff>();
}
