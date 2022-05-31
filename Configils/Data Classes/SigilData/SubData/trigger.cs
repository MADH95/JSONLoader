namespace JLPlugin.Data
{
    public enum TriggerType
	{
        OnDie,
        OnResolveOnBoard,
        OnStruck,
        OnKill,
        OnAttack,
        OnEndOfTurn,
        OnStartOfTurn,
        OnDamage,
        OnHealthLevel,
        OnCombatStart,
        OnEnemyCombatStart,
        OnDetect,
        OnDraw,
        OnActivate,
        OnPreDeath,
        OnPreKill,
        OnMove,
        Passive,
	}

    [System.Serializable]
    public class trigger
    {
        public string triggerType;
        public string activatesForCardsWithCondition;
    }
}