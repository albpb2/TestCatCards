using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattlePlayer _player;
    [SerializeField] private BattlePlayer _enemy;
    [SerializeField] private TMP_Text _turnOutcomeTmPro;

    private BattleCat _selectedPlayerCard;
    private Attack _selectedPlayerAttack;

    public void SelectPlayerAttack(BattleCat cat, Attack attack)
    {
        _selectedPlayerCard = cat;
        _selectedPlayerAttack = attack;
    }

    public void PlayTurn()
    {
        if (_selectedPlayerAttack == null)
        {
            return;
        }

        EvaluateTurn();
        _selectedPlayerAttack = null;
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

    private void EvaluateTurn()
    {
        var (enemyCard, enemyAttack) = _enemy.DrawRandomAttack();

        var damageToEnemy = CalculateDamage(_selectedPlayerAttack, enemyCard);
        var damageToPlayer = CalculateDamage(enemyAttack, _selectedPlayerCard);

        _enemy.ApplyDamage(damageToEnemy);
        _player.ApplyDamage(damageToPlayer);

        var playerMultiplier = CalculateMultiplier(_selectedPlayerAttack, enemyCard);
        var enemyMultiplier = CalculateMultiplier(enemyAttack, _selectedPlayerCard);
        var outcomeText = $"Player used {_selectedPlayerCard.Name} with attack {_selectedPlayerAttack.AttackType} ({_selectedPlayerAttack.DamagePoints}) vs {enemyCard.Type} (multi: {playerMultiplier}){Environment.NewLine}";
        outcomeText += $"Enemy received {damageToEnemy} damage poitns. HP: {_enemy.HP}{Environment.NewLine}{Environment.NewLine}";
        outcomeText += $"Enemy used {enemyCard.Name} with attack {enemyAttack.AttackType} ({enemyAttack.DamagePoints}) vs {_selectedPlayerCard.Type} (multi: {enemyMultiplier}){Environment.NewLine}";
        outcomeText += $"Player received {damageToPlayer} damage poitns. HP: {_player.HP}";
        _turnOutcomeTmPro.text = outcomeText;

        _selectedPlayerCard.GetComponent<PlayerCatCard>().UseCard();
        enemyCard.GetComponent<PlayerCatCard>().UseCard();
    }

    private static float CalculateDamage(Attack attack, BattleCat defendingCat) 
        => attack.DamagePoints * CalculateMultiplier(attack, defendingCat);

    private static float CalculateMultiplier(Attack attack, BattleCat defendingCat)
    {
        var multipliers = _damageMultipliers[attack.AttackType];
        if (!multipliers.TryGetValue(defendingCat.Type, out var multiplier))
        {
            multiplier = 1;
        }
        return multiplier;
    }
}