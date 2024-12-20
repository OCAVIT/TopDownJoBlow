using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

[System.Serializable]
public class Weapon
{
    public GameObject weaponObject;
    public float fireRate;
    public int magazineCapacity;
    public float damage;
    public bool isShotgun;
    public float spread;
    public int pellets;
    public float reloadTime;
    public float cameraShakeIntensity;
    public Camera weaponCamera;

    public AudioClip[] shootingSounds;
    public AudioClip reloadSound;

    public GameObject muzzleFlashPrefab;
    public GameObject imageWeapon;
}

public class PlayerShooting : MonoBehaviour
{
    public List<Weapon> weapons;
    public GameObject bulletPrefab;
    public TMP_Text ammoText;
    public GameObject reloadAnim;
    public float offsetRotation = 0f;

    [Range(0f, 1f)]
    public float shootingVolume = 1f;
    [Range(0f, 1f)]
    public float reloadVolume = 1f;
    [SerializeField] public GameObject[] weaponsObjs;
    private Transform currentFirePoint;
    private Weapon currentWeapon;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private AudioSource audioSource;

    private Dictionary<Weapon, int> ammoDictionary = new Dictionary<Weapon, int>();

    private void Start()
    {
       
        audioSource = GetComponent<AudioSource>();
        UpdateCurrentWeapon();
        if (!ammoDictionary.ContainsKey(currentWeapon))
        {
            ammoDictionary[currentWeapon] = currentWeapon.magazineCapacity;
        }
        UpdateAmmoText();
    }

    private void Update()
    {
        if (isReloading)
            return;

        UpdateCurrentWeapon();
        HandleShooting();
        HandleReloading();
        UpdateAmmoText();
    }
    public void WeaponLoad()
    {
        int weapon = PlayerPrefs.GetInt("weapon");
        if (weapon == 1) weaponsObjs[0].SetActive(true);
        if (weapon == 2) weaponsObjs[1].SetActive(true);
        if (weapon == 3) weaponsObjs[2].SetActive(true);
    }
    private void UpdateCurrentWeapon()
    {
        foreach (var weapon in weapons)
        {
            if (weapon.weaponObject.activeInHierarchy)
            {
                currentWeapon = weapon;
                currentFirePoint = weapon.weaponObject.transform.Find("FirePoint");

                if (!ammoDictionary.ContainsKey(currentWeapon))
                {
                    ammoDictionary[currentWeapon] = currentWeapon.magazineCapacity;
                }

                if (weapon.imageWeapon != null)
                {
                    weapon.imageWeapon.SetActive(true);
                }
            }
            else
            {
                if (weapon.imageWeapon != null)
                {
                    weapon.imageWeapon.SetActive(false);
                }
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && ammoDictionary[currentWeapon] > 0)
        {
            nextFireTime = Time.time + 1f / currentWeapon.fireRate;
            FireBullet();
            PlayRandomShootingSound();
            ammoDictionary[currentWeapon]--;
            ShakeCamera();
        }
    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && ammoDictionary[currentWeapon] < currentWeapon.magazineCapacity)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        PlayReloadSound();

        if (ammoText != null)
            ammoText.gameObject.SetActive(false);

        if (reloadAnim != null)
            reloadAnim.SetActive(true);

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        if (ammoText != null)
            ammoText.gameObject.SetActive(true);

        if (reloadAnim != null)
            reloadAnim.SetActive(false);

        ammoDictionary[currentWeapon] = currentWeapon.magazineCapacity;
        isReloading = false;
    }

    private void FireBullet()
    {
        if (currentWeapon.muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(currentWeapon.muzzleFlashPrefab, currentFirePoint.position, currentFirePoint.rotation);
            Destroy(muzzleFlash, 0.5f);
        }

        if (currentWeapon.isShotgun)
        {
            for (int i = 0; i < currentWeapon.pellets; i++)
            {
                float spreadAngle = UnityEngine.Random.Range(-currentWeapon.spread, currentWeapon.spread);
                Quaternion bulletRotation = currentFirePoint.rotation * Quaternion.Euler(0, offsetRotation + spreadAngle, 0);
                Instantiate(bulletPrefab, currentFirePoint.position, bulletRotation);
            }
        }
        else
        {
            Quaternion bulletRotation = currentFirePoint.rotation * Quaternion.Euler(0, offsetRotation, 0);
            Instantiate(bulletPrefab, currentFirePoint.position, bulletRotation);
        }
    }

    private void PlayRandomShootingSound()
    {
        if (currentWeapon.shootingSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, currentWeapon.shootingSounds.Length);
            audioSource.PlayOneShot(currentWeapon.shootingSounds[randomIndex], shootingVolume);
        }
    }

    private void PlayReloadSound()
    {
        if (currentWeapon.reloadSound != null)
        {
            audioSource.PlayOneShot(currentWeapon.reloadSound, reloadVolume);
        }
    }

    private void ShakeCamera()
    {
        if (currentWeapon.weaponCamera != null)
        {
            StartCoroutine(CameraShakeCoroutine(currentWeapon.cameraShakeIntensity));
        }
    }

    private IEnumerator CameraShakeCoroutine(float intensity)
    {
        Vector3 originalPosition = currentWeapon.weaponCamera.transform.position;
        Quaternion originalRotation = currentWeapon.weaponCamera.transform.rotation;
        float shakeDuration = 0.1f;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * intensity;
            float y = UnityEngine.Random.Range(-1f, 1f) * intensity;

            currentWeapon.weaponCamera.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        currentWeapon.weaponCamera.transform.position = originalPosition;
        currentWeapon.weaponCamera.transform.rotation = originalRotation;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null && currentWeapon != null)
        {
            ammoText.text = $"{currentWeapon.magazineCapacity}\n---\n{ammoDictionary[currentWeapon]}";
        }
    }
}