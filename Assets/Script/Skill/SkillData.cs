using UnityEngine;

public enum SkillType { SlowTime, DoubleDamage, Invincible }

[CreateAssetMenu(fileName = "NewSkill", menuName = "Game/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Basic Info")]
    public string skillName;
    public SkillType type;
    [TextArea] public string description;

    [Header("UI & Gameplay")]
    public Sprite icon;
    public float duration;
    public float cooldown;
}
