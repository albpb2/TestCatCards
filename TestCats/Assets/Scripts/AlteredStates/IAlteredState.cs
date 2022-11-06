public interface IAlteredState
{
    void ApplyState(BattlePlayer battlePlayer);
    bool IsFinished();
    string GetStateText();
}