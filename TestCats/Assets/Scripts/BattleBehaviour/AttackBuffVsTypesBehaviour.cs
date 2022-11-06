using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuffVsTypesBehaviour : BasicBattleBehaviour
{
    [SerializeField] private string _attackName;
    [SerializeField] private int _extraDamagePoints;
    [SerializeField] private List<CatType> _typesImpacted;

    protected override string GetSkillText()
    {
        var skillText = $"{_attackName} tiene un modificador de +{_extraDamagePoints} "
            + $"si el gato enemigo es ";
        for (var i = 0; i < _typesImpacted.Count - 1; i++)
        {
            skillText += _typesImpacted[i].ToString() + ", ";
        }
        if (_typesImpacted.Count > 1)
        {
            skillText = skillText.Remove(skillText.Length - 2, 2);
            skillText += " o ";
        }
        skillText += _typesImpacted[_typesImpacted.Count - 1].ToString();
        skillText += " (se aplica antes del multiplicador)";
        return skillText;
    }

    public override void Attack(Attack attack, BattleCat targetCat, BattlePlayer targetPlayer)
    {
        var extraPoints = 0;
        if (_typesImpacted.Contains(targetCat.Type))
        {
            extraPoints = _extraDamagePoints;
        }
        var damage = CalculateDamage(attack, targetCat, extraPoints);
        targetPlayer.ApplyDamage(damage);

        var multiplier = CalculateMultiplier(attack, targetCat);
        var attackerName = targetPlayer.IsPlayer ? "Enemy" : "Player";
        var outcomeText = $"{attackerName} used {AttackerCat.Name} with attack {attack} (+ {extraPoints}) vs {targetCat.Type} (multi: {multiplier}){Environment.NewLine}";
        outcomeText += $"{targetPlayer.PlayerName} received {damage} damage points. HP: {targetPlayer.HP}{Environment.NewLine}";

        outcomeText += $"{Environment.NewLine}";
        BattleManager.LogAttackOutcome(outcomeText);
    }
}