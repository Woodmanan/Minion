using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    Monster monster;

    public float damageAnimTime = 3;
    public float deathAnimTime = 0.6f;
    public float attackAnimTime = 2f;

    public float shakeIntensity = 1f;

    public Coroutine current;

    private CameraTrackingMode cameraTrackingMode;
    
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<Monster>();

        //cameraTrackingMode = CameraTracking.singleton.mode;

        //Start animation on random frame
        Animator anim = GetComponent<Animator>();
        if(anim != null) {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        }
        

        monster.connections.OnTakeDamage.AddListener(1, OnTakeDamage);
        monster.connections.OnStartAttack.AddListener(1, OnMeleeAttackAttempted);
        monster.connections.OnDeath.AddListener(1, OnDeath);
        monster.connections.OnMove.AddListener(1, OnMove);
    }

    void OnTakeDamage(ref int damageAmt, ref DamageType type, ref DamageSource source) {
        CancelAnimations();
        current = StartCoroutine("TakeDamageAnimation");
    }

    void OnMeleeAttackAttempted(ref AttackAction attack, ref bool outcome) {
        CancelAnimations();
        current = StartCoroutine (MeleeAttackAnimation (attack.target.transform));
    }

    void OnDeath() {
        CancelAnimations();
        current = StartCoroutine("DeathAnimation");
    }

    void OnMove()
    {
        CancelAnimations();
    }

    public void CancelAnimations()
    {
        StopAllCoroutines();
        //if(monster == Player.player) CameraTracking.singleton.mode = cameraTrackingMode;
        monster.transform.position = new Vector3(monster.location.x, monster.location.y, Monster.monsterZPosition);
        
        //Code from Lab 8, unsure what it does 0.0
        //Original comment: Quick dirty fix for death weirdness
        Color save = monster.renderer.color;
        save.a = 1;
        monster.renderer.color = save;
    }

    IEnumerator TakeDamageAnimation() {
        /*if(monster == Player.player) {
            cameraTrackingMode = CameraTracking.singleton.mode;
            CameraTracking.singleton.mode = CameraTrackingMode.Lerp;
        }*/
        Vector3 originalPosition = new Vector3(monster.location.x, monster.location.y, Monster.monsterZPosition);
        float time = 0;
        Vector3 direction;
        if(Random.value < .5) {
            direction = Vector3.left;
        } else {
            direction = Vector3.right;
        }
        Vector3 newPos = originalPosition + direction * 0.4f * shakeIntensity;
        monster.transform.position = newPos;
        while(time <= 1) {
            monster.transform.position = Berp(newPos, originalPosition, time, 1.0f);
            time += Time.deltaTime * damageAnimTime;
            yield return null;
        }
        monster.transform.position = originalPosition;
        //if(monster == Player.player) CameraTracking.singleton.mode = cameraTrackingMode;
        yield return null;
        current = null;
    }

    IEnumerator MeleeAttackAnimation(Transform target) {
        Vector3 originalPosition = new Vector3(monster.location.x, monster.location.y, Monster.monsterZPosition);
        float time = 0;
        Vector3 newPos = originalPosition + Vector3.Normalize(target.position - originalPosition);
        monster.transform.position = newPos;
        while(time <= 1) {
            monster.transform.position = easeOutQuart(newPos, originalPosition, time);
            time += Time.deltaTime * attackAnimTime;
            yield return null;
        }
        monster.transform.position = originalPosition;
        yield return null;
        current = null;
    }

    IEnumerator DeathAnimation() {
        Vector3 originalPosition = new Vector3(monster.location.x, monster.location.y, Monster.monsterZPosition);
        SpriteRenderer render = monster.renderer;
        Color color = render.color;
        Vector3 newPos = originalPosition + Vector3.left * 0.4f;
        monster.transform.position = newPos;
        float originalA = color.a;
        float time = 0;
        while(color.a > 0) {
            color.a = Mathf.Lerp(originalA, 0f, time);
            render.color = color;
            time += Time.deltaTime * deathAnimTime;
            monster.transform.position = Berp(newPos, originalPosition, time * damageAnimTime, 1.0f);
            yield return null;
        }

        yield return null;
        current = null;
    }

    //below easing functions from https://easings.net/

    public static float Berp(float start, float end, float value, float amt) {
        float c4 = (2 * Mathf.PI) / 3;
        if(value > 1) {
            return end;
        }
        value = Mathf.Pow(2, -2.5f * value) * Mathf.Sin((value * 10 - 0.75f) * c4) + 1;
        return start + (end - start) * value;
    }

    public static float easeOutQuart(float start, float end, float value) {
        value = 1 - Mathf.Pow(1 - value, 4);
        return start + (end - start) * value;
    }
 
    public static Vector3 Berp(Vector3 start, Vector3 end, float value, float amt)
    {
        return new Vector3(Berp(start.x, end.x, value, amt), Berp(start.y, end.y, value, amt), Berp(start.z, end.z, value, amt));
    }

    public static Vector3 easeOutQuart(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(easeOutQuart(start.x, end.x, value), easeOutQuart(start.y, end.y, value), easeOutQuart(start.z, end.z, value));
    }


}
