using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] private float _hp;
    [SerializeField] private List<BattleCat> _cards;
    [SerializeField] private bool _isPlayer;
    [SerializeField] private TMP_Text _hpTmPro;
    [SerializeField] private TMP_Text _alteredStatesTmPro;

    private Random _random;
    
    private readonly List<IAlteredState> _alteredStates = new List<IAlteredState>();

    public float HP => _hp;
    public bool IsPlayer => _isPlayer;
    public string PlayerName => IsPlayer ? "Player" : "Enemy";

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

        RefreshAlteredStateText();
        RefreshHpText();
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
        _hp = _hp - damage;
        if (_hp < 0)
        {
            _hp = 0;
        }
        Debug.Log($"Applying {damage} damage to {PlayerName}. HP: {_hp}");
        RefreshHpText();
    }

    private void RefreshHpText()
    {
        _hpTmPro.text = $"HP: {HP}";
    }

    public void ApplyAlteredStates()
    {
        foreach(var alteredState in _alteredStates)
        {
            alteredState.ApplyState(this);
        }

        _alteredStates.RemoveAll(alteredState => alteredState.IsFinished());

        RefreshAlteredStateText();
    }

    private void RefreshAlteredStateText()
    {
        var alteredStateText = string.Empty;
        foreach (var alteredState in _alteredStates)
        {
            alteredStateText += alteredState.GetStateText() + ", ";
        }
        if (alteredStateText.Length > 2)
        {
            alteredStateText = alteredStateText.Remove(alteredStateText.Length - 2, 2);
        }
        _alteredStatesTmPro.text = alteredStateText;
    }

    public void AddAlteredState(IAlteredState alteredState)
    {
        _alteredStates.Add(alteredState);
    }
}