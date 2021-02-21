using System;

[Serializable]
public class Buff
{
    public BuffType type;
    public float additiveBonus;
    public float multipleBonus;


    public Buff(BuffType type, float additiveBonus, float multipleBonus)
    {
        this.type = type;
        this.additiveBonus = additiveBonus;
        this.multipleBonus = multipleBonus;
    }
}

public enum BuffType
{
    MeleeDamage, RangeDamage, Health, Armor, AttackSpeed, MovementSpeed, JumpForce, Special
}

