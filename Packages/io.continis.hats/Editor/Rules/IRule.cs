namespace Hats.Editor.Rules
{
    public interface IRule
    {
        public string RuleName { get; }
        public string RuleDescription { get; }
        public bool OnBecameActive(Hats.ChangeReason reason);
        public bool OnBecameInactive();
    }
}