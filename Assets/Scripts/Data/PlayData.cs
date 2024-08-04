public readonly struct PlayData
{
    public readonly int currentMana;
    public readonly int minute;
    public readonly float second;

    public PlayData(int mana, int min, float sec)
    {
        currentMana = mana;
        minute = min;
        second = sec;
    }
}
