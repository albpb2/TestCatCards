using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicBattleBehaviour : MonoBehaviour
{
    protected BattleManager BattleManager { get; private set; }
    protected BattleCat AttackerCat { get; private set; }

    protected virtual void Awake()
    {
        AttackerCat = GetComponent<BattleCat>();
    }

    protected virtual void Start()
    {
        BattleManager = FindObjectOfType<BattleManager>();

        var playerCatCard = GetComponent<PlayerCatCard>();
        playerCatCard.PrintSkillText(GetSkillText());
    }

    protected virtual string GetSkillText() => string.Empty;

    public virtual void Attack(Attack attack, BattleCat targetCat, BattlePlayer targetPlayer)
    {
        var damage = CalculateDamage(attack, targetCat);
        targetPlayer.ApplyDamage(damage);

        var multiplier = CalculateMultiplier(attack, targetCat);
        var attackerName = targetPlayer.IsPlayer ? "Enemy" : "Player";
        var outcomeText = $"{attackerName} used {AttackerCat.Name} with attack {attack} vs {targetCat.Type} (multi: {multiplier}){Environment.NewLine}";
        outcomeText += $"{targetPlayer.PlayerName} received {damage} damage points. HP: {targetPlayer.HP}{Environment.NewLine}{Environment.NewLine}";
        BattleManager.LogAttackOutcome(outcomeText);
    }

    private static Dictionary<CatType, Dictionary<CatType, float>> _damageMultipliers =
        new Dictionary<CatType, Dictionary<CatType, float>>
        {
            [CatType.Aggresive] = new Dictionary<CatType, float>
            {
                [CatType.Calm] = 2,
                [CatType.Tender] = .5f,
            },
            [CatType.Calm] = new Dictionary<CatType, float>
            {
                [CatType.Aggresive] = .5f,
                [CatType.Agile] = .5f,
                [CatType.Fearful] = 2,
                [CatType.Tender] = 2,
            },
            [CatType.Agile] = new Dictionary<CatType, float>
            {
                [CatType.Calm] = 2,
                [CatType.Fearful] = .5f,
                [CatType.Tender] = .5f,
            },
            [CatType.Fearful] = new Dictionary<CatType, float>
            {
                [CatType.Calm] = .5f,
                [CatType.Agile] = 2,
                [CatType.Fearful] = 0,
                [CatType.Tender] = 2,
            },
            [CatType.Tender] = new Dictionary<CatType, float>
            {
                [CatType.Aggresive] = 2,
                [CatType.Calm] = .5f,
                [CatType.Agile] = 2,
                [CatType.Fearful] = .5f,
            },
        };

    protected float CalculateDamage(Attack attack, BattleCat defendingCat, int extraPoints = 0)
    {
        var damage = (attack.DamagePoints + extraPoints) * CalculateMultiplier(attack, defendingCat);
        var maxDamage = defendingCat.GetComponent<BasicBattleBehaviour>().GetMaxDamageTaken(AttackerCat);
        if (maxDamage.HasValue)
        {
            damage = Mathf.Min(damage, maxDamage.Value);
        }
        return damage;
    }

    protected static float CalculateMultiplier(Attack attack, BattleCat defendingCat)
    {
        var multipliers = _damageMultipliers[attack.AttackType];
        if (!multipliers.TryGetValue(defendingCat.Type, out var multiplier))
        {
            multiplier = 1;
        }
        return multiplier;
    }

    protected virtual int? GetMaxDamageTaken(BattleCat attackerCat) => null;
}