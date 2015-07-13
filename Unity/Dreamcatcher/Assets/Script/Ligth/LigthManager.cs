using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LigthManager : MonoBehaviour {

    [SerializeField]
    private GameObject Support;

    [SerializeField]
    private List<LightClass> LightArray;

    [SerializeField]
    private float RotationSpeed=1.0f;

    public void DisposeLight()
    {
        float angleStep = 360 / LightArray.Count;
        for (int i = 0; i < LightArray.Count;i++ )
        {
            LightArray[i].gLight.transform.localPosition = new Vector3(Mathf.Cos(i * angleStep * Mathf.Deg2Rad)/2, 0, Mathf.Sin(i * angleStep * Mathf.Deg2Rad)/2);
        }
    }

    private void IntensityChange()
    {
        foreach(LightClass lightClass in LightArray)
        {
            if (Random.Range(0, 4) != 0) continue;
            lightClass.gLight.color = Color.Lerp(lightClass.gLight.color,new Color(Random.Range(0.0f, lightClass.MaxRedColor / 255.0f),
                Random.Range(0.0f, lightClass.MaxGreenColor / 255.0f),
                Random.Range(0.0f, lightClass.MaxBlueColor / 255.0f)),Time.time);
        }
    }

    float MaxXAngleRotation = 90;
    float MinXAngleRotation = 60;

    private void changeRotation()
    {
        foreach (LightClass CurrentLight in LightArray)
        {
            if (Random.Range(0, 4) == 0) continue;
            if (CurrentLight.gLight.transform.localRotation.eulerAngles.x >= MaxXAngleRotation)
            {
                CurrentLight.previousSign = 1;
            }
            else if (CurrentLight.gLight.transform.localRotation.eulerAngles.x <= 60)
            {
                CurrentLight.previousSign = -1;
            }
            CurrentLight.gLight.transform.Rotate(CurrentLight.previousSign * (RotationSpeed / 2), 0, 0, Space.Self);
        }
    }

    private void applyRandomChange()
    {
        Support.transform.Rotate(0, RotationSpeed,0,Space.Self);
        changeRotation();
        IntensityChange();
    }


    void FixedUpdate()
    {
        applyRandomChange();
    }
}
