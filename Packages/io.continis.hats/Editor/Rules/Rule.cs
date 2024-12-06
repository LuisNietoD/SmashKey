namespace Hats.Editor.Rules
{
    public abstract class RuleBase : IRule
    {
        public abstract string RuleName { get; }
        public abstract string RuleDescription { get; }
        public abstract bool OnBecameActive(Hats.ChangeReason reason);
        public abstract bool OnBecameInactive();
    }
}