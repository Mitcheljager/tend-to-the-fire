using UnityEngine;

public class EnemyAudio : MonoBehaviour {
    public EnemyNavigation enemyNavigation;
    public AudioHelper audioHelperFollowing;
    public float followingAudioMaxDistance = 50f;
    public float followingAudioLerpSpeed = 1f;

    private float initialFollowingAudioMaxDistance = 0f;

    void Start() {
        initialFollowingAudioMaxDistance = audioHelperFollowing.audioSource.maxDistance;
    }

    void Update() {
        float followingAudioMaxRangeTarget = enemyNavigation.isFollowingPlayer ? followingAudioMaxDistance : initialFollowingAudioMaxDistance;

        audioHelperFollowing.audioSource.maxDistance = Mathf.Lerp(audioHelperFollowing.audioSource.maxDistance, followingAudioMaxRangeTarget, followingAudioLerpSpeed * Time.deltaTime);
    }
}
