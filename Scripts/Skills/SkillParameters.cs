using Godot;

namespace Arcanum.Skills
{
    public partial class SkillParameter : Node
    {
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public object Value { get; set; }
    }
}
