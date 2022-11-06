using System;

[Serializable]
public class Attack
{
    public string AttackName;
    public CatType AttackType;
    public int DamagePoints;

    public override string ToString()
    {
        return $"{AttackName} ({AttackType}): {DamagePoints}";
    }
}