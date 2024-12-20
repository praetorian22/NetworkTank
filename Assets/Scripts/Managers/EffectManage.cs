using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class EffectManage : GenericSingletonClass<EffectManage>
{
    [SerializeField] private GameObject soundObjectPrefab;
    [SerializeField] private GameObject _boom;
    [SerializeField] private GameObject _boomShot;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip playerShot;
    [SerializeField] private AudioClip enemyShot;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roundMusic;
    [SerializeField] private float volumeEffect = 1f;
    [SerializeField] private float volumeMusic = 1f;
    private Dictionary<typeEffect, GameObject> effectPrefabDict = new Dictionary<typeEffect, GameObject>();
    public Transform parent;

    private void Start()
    {
        effectPrefabDict.Add(typeEffect.explosion, _boom);
        effectPrefabDict.Add(typeEffect.explosionMini, _boomShot);
    }
    public void SetParent()
    {
        parent = GameObject.FindWithTag("parentForGameObject").transform;
    }

    public void Explosion(Vector3 position)
    {
        instantiateCmd(typeEffect.explosion, position, 5, 0, 0, 0);
    }
    
    public void ExplosionMini(Vector3 position)
    {
        instantiateCmd(typeEffect.explosionMini, position, 4, 0, 0, 0);
    }
    [Command(requiresAuthority = false)]
    private void instantiateCmd(typeEffect prefabType, Vector3 position, int time, float rotationx, float rotationy, float rotationz)
    {
        GameObject newparticleSystem = Instantiate(effectPrefabDict[prefabType], position, Quaternion.identity);
        newparticleSystem.transform.rotation = Quaternion.Euler(rotationx, rotationy, rotationz);        
        NetworkServer.Spawn(newparticleSystem);
        SetParentPSRPC(newparticleSystem);
        Destroy(newparticleSystem, time);        
    }
    [ClientRpc]
    private void SetParentPSRPC(GameObject ps)
    {
        if (parent != null)
        {
            var main = ps.GetComponent<ParticleSystem>().main;
            main.simulationSpace = ParticleSystemSimulationSpace.Custom;
            main.customSimulationSpace = parent;
        }
    }
    public void MakeExplosionSound(Vector3 position)
    {
        MakeSound(explosion, position);
    }
    public void MakePlayerShotSound(Vector3 position)
    {
        MakeSound(playerShot, position);
    }
    public void MakeEnemyShotSound(Vector3 position)
    {
        MakeSound(enemyShot, position);
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }

    private void MakeSound(AudioClip original, Vector3 position)
    {
        GameObject soundObject = Instantiate(soundObjectPrefab, position, Quaternion.identity, parent);
        soundObject.GetComponent<AudioSource>().clip = original;
        soundObject.GetComponent<AudioSource>().Play();
        Destroy(soundObject, original.length);
        //AudioSource.PlayClipAtPoint(original, position, volumeEffect);
    }

    private void MakeSoundCicle(AudioClip original)
    {
        audioSource.volume = volumeMusic;
        audioSource.clip = original;
        audioSource.Play();
    }

    public void VolumeMinus()
    {
        StartCoroutine(VolumeM());
    }

    public void SetVolumeMusic(Scrollbar sb)
    {
        audioSource.volume = sb.value;
        volumeMusic = sb.value;
    }

    public void SetVolumeSound(Scrollbar sb)
    {
        volumeEffect = sb.value;
    }

    public IEnumerator VolumeM()
    {
        int tik = 100;
        while (tik > 0 && audioSource.volume > 0f)
        {
            tik -= 1;
            if (audioSource.volume > 0f) audioSource.volume = audioSource.volume - 0.01f;
            yield return new WaitForFixedUpdate();
        }
        audioSource.volume = 0f;
    }

    public enum typeEffect
    { 
        explosion,
        explosionMini,
    }

}
