using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour {
    private EnemyNavigation enemyNavigation;
    private Animator animator;

    void Start() {
        enemyNavigation = GetComponent<EnemyNavigation>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetFloat("Velocity", enemyNavigation.agent.velocity.magnitude);
    }
}
