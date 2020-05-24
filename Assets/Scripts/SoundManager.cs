using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip gameOverMusic;
    public static SoundManager instance;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMenuMusic()
    {
        audioSource.PlayOneShot(menuMusic);
    }
    public void PlayGameMusic()
    {
        audioSource.PlayOneShot(gameMusic);
    }

    public void PlayGameOverMusic()
    {
        audioSource.PlayOneShot(gameOverMusic);
    }
}