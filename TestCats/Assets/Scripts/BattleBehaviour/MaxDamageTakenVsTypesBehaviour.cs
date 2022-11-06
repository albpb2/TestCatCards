using System;
using System.Collections.Generic;
using UnityEngine;

public class MaxDamageTakenVsTypesBehaviour : BasicBattleBehaviour
{
    [SerializeField] private int _maxDamagePointsTaken;
    [SerializeField] private List<CatType> _typesImpacted;

    protected override string GetSkillText()
    {
        var skillText = "Si el gato enemigo es ";
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
        skillText += $" no puede quitarle más de {_maxDamagePointsTaken} a tu jugador este turno";
        return skillText;
    }

    protected override int? GetMaxDamageTaken(BattleCat attackerCat)
        => _typesImpacted.Contains(attackerCat.Type) ? _maxDamagePointsTaken : null;
}