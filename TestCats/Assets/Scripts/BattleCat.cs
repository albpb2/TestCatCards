using UnityEngine;

public class BattleCat : MonoBehaviour
{
    [SerializeField] private CatType _type;
    [SerializeField] private int _attackPoints;
    [SerializeField] private string _name;
    [SerializeField] private Attack[] _attacks;

    public CatType Type => _type;
    public int AttackPoints => _attackPoints;
    public string Name => _name;
    public Attack[] Attacks => _attacks;
}
