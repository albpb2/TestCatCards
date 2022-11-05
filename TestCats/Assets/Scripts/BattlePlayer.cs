using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] private float _hp;
    [SerializeField] private List<BattleCat> _cards;
    [SerializeField] private bool _isPlayer;

    private Random _random;

    public float HP => _hp;

    private void Start()
    {
        _random = new Random();

        if (_isPlayer)
        {
            foreach(var card in _cards)
            {
                card.GetComponent<PlayerCatCard>().EnableInteractions();
            }
        }
    }

    public (BattleCat, Attack) DrawRandomAttack()
    {
        int index = _random.Next(_cards.Count);
        var card = _cards[index];
        _cards.RemoveAt(index);

        var attackIndex = _random.Next(card.Attacks.Length);
        Debug.Log($"Randomly selected {card.Name} {card.Attacks[attackIndex].AttackType}");
        return (card, card.Attacks[attackIndex]);
    }

    public void ApplyDamage(float damage)
    {
        var playerName = _isPlayer ? "player" : "enemy";
        _hp = _hp - damage;
        if (_hp < 0)
        {
            _hp = 0;
        }
        Debug.Log($"Applying {damage} damage to {playerName}. HP: {_hp}");
    }
}