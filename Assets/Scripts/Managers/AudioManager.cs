using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum BGM_Songs { MENU, WAVE, BOSS, LAST }

    public enum SFX_Sounds
    {
        BASICSHOOT, HIT, HIT_CHARACTER, 
        FIRE, FIRE_EXPLOSION, FIRE_BOSS, FIRE_BOSS_CHARGE, 
        PLANT, PLANT_BOSS, PLANT_BOSS_CHARGE,
        LIGHTNING, LIGHTNING_BOSS, LIGHTNING_BOSS_BALL, LIGHTNING_BOSS_BALL_CHARGE, 
        SHIELD, DASH, HABILITY, UPGRADE, GEM, LAST
    }
    //AudioManager is a singleton. Only one instance of GameManager can exist
    public static AudioManager Instance { get; private set; }

    public List<AudioClip> bgmClipList = new List<AudioClip>();
    public List<AudioClip> sfxClipList = new List<AudioClip>();

    private AudioSource bgmSource;
    //private AudioSource sfxSource;

    public BGM_Songs currentBGM = BGM_Songs.LAST;

    // Gestion de Mixers --> se añade otra ranura aqui para modificarlo
    // Todos estan en el Master
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup menuMixerGroup;
    public AudioMixerGroup waveMixerGroup;
    public AudioMixerGroup bossMixerGroup;

    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup basicAttackMixerGroup;
    public AudioMixerGroup fireAttackMixerGroup;
    public AudioMixerGroup fireExplosionMixerGroup;
    public AudioMixerGroup fireBossAttackMixerGroup;
    public AudioMixerGroup fireBossChargeAttackMixerGroup;
    public AudioMixerGroup plantAttackMixerGroup;
    public AudioMixerGroup plantBossAttackMixerGroup;
    public AudioMixerGroup plantBossChargeAttackMixerGroup;
    public AudioMixerGroup lightningAttackMixerGroup;
    public AudioMixerGroup lightningBossAttackMixerGroup;
    public AudioMixerGroup lightningBossBallAttackMixerGroup;
    public AudioMixerGroup lightningBossBallChargeAttackMixerGroup;
    public AudioMixerGroup dashMixerGroup;
    public AudioMixerGroup shieldMixerGroup;
    public AudioMixerGroup habilityActivationMixerGroup;
    public AudioMixerGroup habilityUpgradeMixerGroup;
    public AudioMixerGroup hitMixerGroup;
    public AudioMixerGroup hitCharacterMixerGroup;
    public AudioMixerGroup gemMixerGroup;

    private List<AudioSource> sfxSounds = new List<AudioSource>();
    public int maxSFX = 7;
    public int currentSFX = -1;

    // Variacion de pitch y volumen
    //public float sfxVolumeVariation = 0.25f;
    //public float sfxPitchVariationMin = -0.30f;
    //public float sfxPitchVariationMax = 0.10f;

    //Awake is called before anything
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
            Debug.Log("There is already an AudioManager");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update

    // Modificador de los mixers
    void Start()
    {
        // BGM
        bgmSource = GetComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmMixerGroup;


        for (int i = 0; i < maxSFX; i++)
        {
            // SFX
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixerGroup;
            sfxSounds.Add(source);
        }

    }

    // Update is called once per frame
    void Update()
    {
        MusisSceneManager();
    }

    // Gestion de muscias de las escenas
    public void MusisSceneManager()
    {
        // MENU
        if (SceneManager.GetActiveScene().name == "MenuInicio")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.MENU);
        }
        else if (SceneManager.GetActiveScene().name == "MenuOpciones")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.MENU);
        }
        else if (SceneManager.GetActiveScene().name == "MenuControles")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.MENU);
        }
        else if (SceneManager.GetActiveScene().name == "Creditos")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.MENU);
        }
        else if (SceneManager.GetActiveScene().name == "DebugMenu")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.MENU);
        }
        // WAVE
        else if (SceneManager.GetActiveScene().name == "CambiarOleadas")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.WAVE);
        }
        else if (SceneManager.GetActiveScene().name == "Oleada2")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.WAVE);
        }
        else if (SceneManager.GetActiveScene().name == "Oleada3")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.WAVE);
        }
        else if (SceneManager.GetActiveScene().name == "OleadaFinal")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.WAVE);
        }
        // BOSS
        else if (SceneManager.GetActiveScene().name == "JefeFuego")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.BOSS);
        }
        else if (SceneManager.GetActiveScene().name == "JefePlanta")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.BOSS);
        }
        else if (SceneManager.GetActiveScene().name == "JefeRayo")
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.BOSS);
        }
        // NONE SOUND
        else
        {
            AudioManager.Instance.PlayBGMFromEnum(AudioManager.BGM_Songs.LAST);
        }

    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null) { return; }
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    public void StopBGM()
    {
        if (bgmSource == null) { return; }
        bgmSource.Stop();
        currentBGM = BGM_Songs.LAST;
    }


    public void PlayBGMFromEnum(BGM_Songs song)
    {
        if (currentBGM == song) { return; }
        currentBGM = song;

        int index = (int)song;
        if (index < 0 || index >= bgmClipList.Count) { return; }

        bgmSource.Stop();
        bgmSource.clip = bgmClipList[index];

        // Aquí se asigna el sonido correcto:
        switch (song)
        {
            case BGM_Songs.MENU:
                bgmSource.outputAudioMixerGroup = menuMixerGroup;
                break;
            case BGM_Songs.WAVE:
                bgmSource.outputAudioMixerGroup = waveMixerGroup;
                break;
            case BGM_Songs.BOSS:
                bgmSource.outputAudioMixerGroup = bossMixerGroup;
                break;
            case BGM_Songs.LAST:
            default:
                // Si no hay grupo específico, usa el grupo genérico de BGM
                bgmSource.outputAudioMixerGroup = bgmMixerGroup;
                break;

        }
        bgmSource.Play();


        //PlayBGM(bgmClipList[index]);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (currentSFX == -1)
        {
            currentSFX = 0;
        }
        else
        {
            currentSFX++;
            if (currentSFX >= maxSFX)
            {
                currentSFX = 0;
            }
        }
        if (sfxSounds[currentSFX] == null) { return; }
        if (sfxSounds[currentSFX].isPlaying)
        {
            sfxSounds[currentSFX].Stop();
        }
        sfxSounds[currentSFX].clip = clip;

        sfxSounds[currentSFX].Play();

        //Prueba de sonido random
        //if (currentSFX == -1) { currentSFX = 0; }
        //else {
        //    currentSFX++;
        //    if (currentSFX >= maxSFX) { currentSFX = 0; }
        //}
        //AudioSource efects = sfxSounds[currentSFX];
        //if (efects == null) return;
        //if (efects.isPlaying) { efects.Stop(); }
        //efects.clip = clip;
        //efects.pitch = 1.0f + Random.Range(sfxPitchVariationMin, sfxPitchVariationMax);
        //efects.volume = 1.0f - Random.Range(0f, sfxVolumeVariation);
        //efects.Play();
    }

    public void PlaySFXFromEnum(SFX_Sounds sound)
    {
        int index = (int)sound;
        if (index < 0 || index >= sfxClipList.Count) { return; }
        currentSFX = (currentSFX + 1) % maxSFX;
        AudioSource effect = sfxSounds[currentSFX];
        effect.clip = sfxClipList[index];



        // Aquí se asigna el sonido correcto:
        switch (sound)
        {
            case SFX_Sounds.BASICSHOOT:
                effect.outputAudioMixerGroup = basicAttackMixerGroup;
                break;
            case SFX_Sounds.HIT:
                effect.outputAudioMixerGroup = hitMixerGroup;
                break;
            case SFX_Sounds.HIT_CHARACTER:
                effect.outputAudioMixerGroup = hitCharacterMixerGroup;
                break;
            case SFX_Sounds.FIRE:
                effect.outputAudioMixerGroup = fireAttackMixerGroup;
                break;
            case SFX_Sounds.FIRE_EXPLOSION:
                effect.outputAudioMixerGroup = fireExplosionMixerGroup;
                break;
            case SFX_Sounds.FIRE_BOSS:
                effect.outputAudioMixerGroup = fireBossAttackMixerGroup;
                break;
            case SFX_Sounds.FIRE_BOSS_CHARGE:
                effect.outputAudioMixerGroup = fireBossChargeAttackMixerGroup;
                break;
            case SFX_Sounds.PLANT:
                effect.outputAudioMixerGroup = plantAttackMixerGroup;
                break;
            case SFX_Sounds.PLANT_BOSS:
                effect.outputAudioMixerGroup = plantBossAttackMixerGroup;
                break;
            case SFX_Sounds.PLANT_BOSS_CHARGE:
                effect.outputAudioMixerGroup = plantBossChargeAttackMixerGroup;
                break;
            case SFX_Sounds.LIGHTNING:
                effect.outputAudioMixerGroup = lightningAttackMixerGroup;
                break;
            case SFX_Sounds.LIGHTNING_BOSS:
                effect.outputAudioMixerGroup = lightningBossAttackMixerGroup;
                break;
            case SFX_Sounds.LIGHTNING_BOSS_BALL:
                effect.outputAudioMixerGroup = lightningBossBallAttackMixerGroup;
                break;
            case SFX_Sounds.LIGHTNING_BOSS_BALL_CHARGE:
                effect.outputAudioMixerGroup = lightningBossBallChargeAttackMixerGroup;
                break;
            case SFX_Sounds.SHIELD:
                effect.outputAudioMixerGroup = shieldMixerGroup;
                break;
            case SFX_Sounds.DASH:
                effect.outputAudioMixerGroup = dashMixerGroup;
                break;
            case SFX_Sounds.HABILITY:
                effect.outputAudioMixerGroup = habilityActivationMixerGroup;
                break;
            case SFX_Sounds.UPGRADE:
                effect.outputAudioMixerGroup = habilityUpgradeMixerGroup;
                break;
            case SFX_Sounds.GEM:
                effect.outputAudioMixerGroup = gemMixerGroup;
                break;
            case SFX_Sounds.LAST:
            default:
                // Si no hay grupo específico, usa el grupo genérico de SFX
                effect.outputAudioMixerGroup = sfxMixerGroup;
                break;

        }
        effect.Play();
    }

    public void SetBGMvolume(float volume)
    {
        bgmMixerGroup.audioMixer.SetFloat("volumeBGM", Mathf.Log10(volume) * 20);
        /*
        - Go to Assets -> Create -> Audio Mixer
        - In the Groups section click on + and add a group. Rename it as BGM
        - Select your group, right click on the free space of the Attenuation in the inspector, click Expose.
        - Click on Exposed Parameters in the Audio Mixer panel. Rename parameter name as volumeBGM and press enter
       
        */
    }
}
