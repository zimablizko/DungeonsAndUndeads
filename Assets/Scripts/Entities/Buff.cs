using System;

[Serializable]
public class Buff
{
    public BuffType type;
    public float additiveBonus;
    public float multipleBonus;
}

public enum BuffType
{
    MeleeDamage, RangeDamage, Health, Armor, AttackSpeed, MovementSpeed, JumpForce, Special
}