using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCatCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTmPro;
    [SerializeField] private TMP_Text _typeTmPro;
    [SerializeField] private TMP_Text _skillTmPro;
    [SerializeField] private Button _attack1Button;
    [SerializeField] private Button _attack2Button;

    private BattleManager _battleManager;

    private BattleCat _catCard;
    private Image _image;

    private bool _interactionsEnabled;
    private bool _used;

    public delegate void CardClicked();
    public static event CardClicked _onCardClicked;

    private void Awake()
    {
        _catCard = GetComponent<BattleCat>();
        _image = GetComponent<Image>();

        if (!TryGetComponent<BasicBattleBehaviour>(out var _))
        {
            gameObject.AddComponent(typeof(BasicBattleBehaviour));
        }

        _nameTmPro.text = _catCard.Name;
        _typeTmPro.text = _catCard.Type.ToString();

        _attack1Button.GetComponentInChildren<TMP_Text>().text = $"{_catCard.Attacks[0]}";
        _attack1Button.interactable = false;
        if (_catCard.Attacks.Length > 1)
        {
            _attack2Button.GetComponentInChildren<TMP_Text>().text = $"{_catCard.Attacks[1]}";
            _attack2Button.interactable = false;
        }
        else
        {
            _attack2Button.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
    }

    private void OnDestroy()
    {
        if (_interactionsEnabled)
        {
            _onCardClicked -= UnSelectCard;
        }
    }

    public void EnableInteractions()
    {
        _interactionsEnabled = true;

        _attack1Button.interactable = true;
        if (_attack2Button.gameObject.activeInHierarchy)
        {
            _attack2Button.interactable = true;
        }

        _onCardClicked += UnSelectCard;
    }

    public void UnSelectCard()
    {
        if (!_used)
        {
            _attack1Button.image.color = Color.white;
            if (_attack2Button.gameObject.activeInHierarchy)
            {
                _attack2Button.image.color = Color.white;
            }
        }
    }

    public void UseCard()
    {
        UnSelectCard();
        _image.color = Color.gray;
        _used = true;
    }

    public void UseAttack(int attackIndex)
    {
        if (!_interactionsEnabled || _used)
        {
            return;
        }

        _onCardClicked?.Invoke();

        switch (attackIndex)
        {
            case 0:
                _attack1Button.image.color = Color.red;
                _battleManager.SelectPlayerAttack(_catCard, _catCard.Attacks[0]);
                break;
            case 1:
                _attack2Button.image.color = Color.red;
                _battleManager.SelectPlayerAttack(_catCard, _catCard.Attacks[1]);
                break;
            default: break;
        }
        Debug.Log($"Selected {_catCard.Name} {_catCard.Attacks[attackIndex].AttackType} attack");
    }

    public void PrintSkillText(string text)
    {
        _skillTmPro.text = text;
    }
}