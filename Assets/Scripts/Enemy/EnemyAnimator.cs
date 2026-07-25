using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour {
    private EnemyNavMesh enemyNavMesh;
    private Animator animator;

    void Start() {
        enemyNavMesh = GetComponent<EnemyNavMesh>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetFloat("Velocity", enemyNavMesh.navMeshAgent.velocity.magnitude);
    }
}
