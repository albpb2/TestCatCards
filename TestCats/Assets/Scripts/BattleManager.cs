using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattlePlayer _player;
    [SerializeField] private BattlePlayer _enemy;
    [SerializeField] private TMP_Text _turnOutcomeTmPro;
    [SerializeField] private GameObject _gameOverPanel;

    private BattleCat _selectedPlayerCard;
    private Attack _selectedPlayerAttack;

    private string _turnOutcomeText;

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

        ClearOutcomeText();
        EvaluateTurn();
        ApplyAlteredStates();
        PrintOutcomeText();
        FinishGameIfAnyPlayerIsDead();
    }

    private void EvaluateTurn()
    {
        var (enemyCard, enemyAttack) = _enemy.DrawRandomAttack();

        _selectedPlayerCard.GetComponent<BasicBattleBehaviour>().Attack(_selectedPlayerAttack, enemyCard, _enemy);
        enemyCard.GetComponent<BasicBattleBehaviour>().Attack(enemyAttack, _selectedPlayerCard, _player);

        _selectedPlayerCard.GetComponent<PlayerCatCard>().UseCard();
        enemyCard.GetComponent<PlayerCatCard>().UseCard();

        _selectedPlayerCard = null;
        _selectedPlayerAttack = null;
    }

    private void ApplyAlteredStates()
    {
        _player.ApplyAlteredStates();
        _enemy.ApplyAlteredStates();
    }

    public void ClearOutcomeText()
    {
        _turnOutcomeText = string.Empty;
        _turnOutcomeTmPro.text = string.Empty;
    }

    public void LogAttackOutcome(string outcomeText)
    {
        _turnOutcomeText += outcomeText;
    }

    public void PrintOutcomeText()
    {
        _turnOutcomeTmPro.text = _turnOutcomeText;
    }

    private void FinishGameIfAnyPlayerIsDead()
    {
        if (_player.HP <= 0 || _enemy.HP <= 0)
        {
            FinishGame();
        }
    }

    public void FinishGame()
    {
        _gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}