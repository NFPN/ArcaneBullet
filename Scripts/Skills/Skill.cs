using Godot;

namespace Arcanum.Skills
{
    public abstract partial class Skill : Node
    {
        public abstract void Activate();
        public abstract void Activate(SkillParameter[] parameters);
        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
    }
}

