using UnityEngine;
using System.Collections;

public class PlayerSkillHandler : MonoBehaviour
{
    [Header("Slots")]
    public SkillData skillX;
    public SkillData skillC;

    // 쿨다운 추적
    float _cdX, _cdC;

    // 외부(UI) 접근용
    public float CooldownX => skillX ? skillX.cooldown : 0f;
    public float CooldownC => skillC ? skillC.cooldown : 0f;
    public float CooldownRemainingX => Mathf.Max(0f, _cdX);
    public float CooldownRemainingC => Mathf.Max(0f, _cdC);

    void Update()
    {
        if (_cdX > 0f) _cdX -= Time.deltaTime;
        if (_cdC > 0f) _cdC -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X)) TryUse(ref _cdX, skillX);
        if (Input.GetKeyDown(KeyCode.C)) TryUse(ref _cdC, skillC);
    }

    void TryUse(ref float cdTimer, SkillData data)
    {
        if (!data || cdTimer > 0f || GameManager.Instance.IsGameOver) return;
        StartCoroutine(RunSkill(data));
        cdTimer = data.cooldown;
    }

    IEnumerator RunSkill(SkillData s)
    {
        switch (s.type)
        {
            case SkillType.SlowTime:
                GameManager.Instance.StartSlow(s.duration);
                break;

            case SkillType.DoubleDamage:
                PlayerAttack.Instance.damageMultiplier = 2f;
                yield return new WaitForSeconds(s.duration);
                PlayerAttack.Instance.damageMultiplier = 1f;
                yield break;

            case SkillType.Invincible:
                PlayerHealth.Instance.isInvincible = true;
                yield return new WaitForSeconds(s.duration);
                PlayerHealth.Instance.isInvincible = false;
                yield break;
        }
    }
}
