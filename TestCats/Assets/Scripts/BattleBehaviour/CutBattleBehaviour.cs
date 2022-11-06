using System;
using UnityEngine;

public class CutBattleBehaviour : BasicBattleBehaviour
{
    [SerializeField] private int _minDamageToCut;
    [SerializeField] private int _damagePerTurn;
    [SerializeField] private int _cutDurationTurns;

    protected override string GetSkillText()
    {
        return $"Si el corte hace + de {_minDamageToCut} de daño inflige "
            + $"heridas profundas que le causan {_damagePerTurn} " 
            + $"de daño durante {_cutDurationTurns} turnos";
    }

    public override void Attack(Attack attack, BattleCat targetCat, BattlePlayer targetPlayer)
    {
        var damage = CalculateDamage(attack, targetCat);
        targetPlayer.ApplyDamage(damage);

        var multiplier = CalculateMultiplier(attack, targetCat);
        var attackerName = targetPlayer.IsPlayer ? "Enemy" : "Player";
        var outcomeText = $"{attackerName} used {AttackerCat.Name} with {attack} vs {targetCat.Type} (multi: {multiplier}){Environment.NewLine}";
        outcomeText += $"{targetPlayer.PlayerName} received {damage} damage points. HP: {targetPlayer.HP}{Environment.NewLine}";

        if (damage > _minDamageToCut)
        {
            targetPlayer.AddAlteredState(new CutAlteredState(BattleManager, _cutDurationTurns, _damagePerTurn));
            outcomeText += $"Also, {targetPlayer.PlayerName} is cut and will receive 1 damage per turn during 2 turns.{Environment.NewLine}";
        }

        outcomeText += $"{Environment.NewLine}";
        BattleManager.LogAttackOutcome(outcomeText);
    }
}