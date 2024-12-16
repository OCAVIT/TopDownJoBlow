using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // ����� ��������
    public GameObject bulletPrefab; // ������ ����

    private int currentWeapon = 1; // 1 - �������, 2 - ��������

    private int rifleAmmoReserve = 30; // ����� �������� ��� ��������
    private int rifleAmmoInClip = 30; // ������� � ������ ��������
    private const int rifleClipSize = 30; // ������ ������ ��������

    private int pistolAmmoReserve = 20; // ����� �������� ��� ���������
    private int pistolAmmoInClip = 10; // ������� � ������ ���������
    private const int pistolClipSize = 10; // ������ ������ ���������

    private bool isReloading = false;

    private void Update()
    {
        HandleWeaponSwitch();
        HandleReload();
        HandleShooting();
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 2;
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (currentWeapon == 1)
            {
                StartCoroutine(ReloadRifle());
            }
            else if (currentWeapon == 2)
            {
                StartCoroutine(ReloadPistol());
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon == 1 && rifleAmmoInClip >= 3)
            {
                StartCoroutine(FireRifleBurst());
            }
            else if (currentWeapon == 2 && pistolAmmoInClip > 0)
            {
                FirePistol();
            }
        }
    }

    private IEnumerator FireRifleBurst()
    {
        for (int i = 0; i < 3; i++)
        {
            if (rifleAmmoInClip > 0)
            {
                FireBullet();
                rifleAmmoInClip--;
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    private void FirePistol()
    {
        if (pistolAmmoInClip > 0)
        {
            FireBullet();
            pistolAmmoInClip--;
        }
    }

    private void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private IEnumerator ReloadRifle()
    {
        isReloading = true;
        yield return new WaitForSeconds(2f);

        int ammoNeeded = rifleClipSize - rifleAmmoInClip;
        int ammoToReload = Mathf.Min(ammoNeeded, rifleAmmoReserve);

        rifleAmmoInClip += ammoToReload;
        rifleAmmoReserve -= ammoToReload;

        isReloading = false;
    }

    private IEnumerator ReloadPistol()
    {
        isReloading = true;
        yield return new WaitForSeconds(1.5f);

        int ammoNeeded = pistolClipSize - pistolAmmoInClip;
        int ammoToReload = Mathf.Min(ammoNeeded, pistolAmmoReserve);

        pistolAmmoInClip += ammoToReload;
        pistolAmmoReserve -= ammoToReload;

        isReloading = false;
    }
}