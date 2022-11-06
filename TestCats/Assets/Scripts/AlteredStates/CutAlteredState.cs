using System;

public class CutAlteredState : IAlteredState
{
    private BattleManager _battleManager;

    private int _remainingTurns;
    private int _damagePerTurn;
    private bool _firstTurnFinished;

    public CutAlteredState(BattleManager battleManager, int durationTurns, int damagePerTurn)
    {
        _battleManager = battleManager;

        _remainingTurns = durationTurns;
        _damagePerTurn = damagePerTurn;
    }

    public void ApplyState(BattlePlayer battlePlayer)
    {
        if (!_firstTurnFinished)
        {
            _firstTurnFinished = true;
            return;
        }

        battlePlayer.ApplyDamage(_damagePerTurn);
        _remainingTurns--;
        _battleManager.LogAttackOutcome($"{battlePlayer.PlayerName} received {_damagePerTurn} due to {nameof(CutAlteredState)}{Environment.NewLine}");
    }

    public string GetStateText() => $"Cut ({_remainingTurns} turns left)";

    public bool IsFinished() => _remainingTurns <= 0;
}