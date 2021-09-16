using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioClip metalStepsAudio;
    [SerializeField] private AudioClip swordSwingAudio;
    [SerializeField] private AudioClip swordHitAudio;
    [SerializeField] private AudioClip gunShotAudio;
    [SerializeField] private AudioClip gunReloadAudio;
    [SerializeField] private AudioClip[] hitFemaleAudio;
    [SerializeField] private AudioClip enemyDeadAudio;
    [SerializeField] private AudioClip playerDeadAudio;
    [SerializeField] private AudioClip dashAudio;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void RandomPitch()
    {
        _audioSource.volume = Random.Range(0.8f, 1f);
        _audioSource.pitch = Random.Range(0.9f, 1.05f);
    }

    private void Step()
    {
        RandomPitch();
        _audioSource.PlayOneShot(metalStepsAudio);
    }

    private void SwordSwingAudio()
    {
        RandomPitch();
        _audioSource.PlayOneShot(swordSwingAudio);
    }

    public void SwordHitAudio()
    {
        RandomPitch();
        _audioSource.PlayOneShot(swordHitAudio);
    }
    
    public void GunShotAudio()
    {
        RandomPitch();
        _audioSource.PlayOneShot(gunShotAudio);
    }
    
    public void GunReloadAudio()
    {
        _audioSource.PlayOneShot(gunReloadAudio);
    }

    public void HitFemaleAudio()
    {
        int randIdx = Random.Range(0, hitFemaleAudio.Length);
        _audioSource.PlayOneShot(hitFemaleAudio[randIdx]);
    }

    public void EnemyDeadAudio()
    {
        RandomPitch();
        _audioSource.PlayOneShot(enemyDeadAudio);
    }

    public void PlayerDeadAudio()
    {
        _audioSource.PlayOneShot(playerDeadAudio);
    }

    public void DashAudio()
    {
        RandomPitch();
        _audioSource.PlayOneShot(dashAudio);
    }
}
