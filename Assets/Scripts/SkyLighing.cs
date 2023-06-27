using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SkyLighing : MonoBehaviour
{
    // Start is called before the first frame update
    public Gradient skyColors;
    public Gradient equatorColors;
    public Gradient groundColors;
    public Gradient sunColors;

    public float speed;

    float daytime = 0;
    float dayLength = 100;


    public GameObject earth;
    public Light sun;
    public Light moon;


    void Start()
    {
    }

    float timeOfDay = 0;

    public bool TimeTest;
    
    [Range(0f,1f)]
    public float timeInput = 0;
    // Update is called once per frame
    void Update()
    {
        daytime = (daytime + speed * Time.deltaTime) % dayLength;
        // Time of day + Debug time controls

        #if UNITY_EDITOR
        timeOfDay = TimeTest ? timeInput : daytime / dayLength;
        #else
        timeOfDay =  daytime / dayLength;
        #endif
        // COLORS
        RenderSettings.ambientSkyColor = skyColors.Evaluate(timeOfDay);
        RenderSettings.ambientEquatorColor = equatorColors.Evaluate(timeOfDay);
        RenderSettings.ambientGroundColor = groundColors.Evaluate(timeOfDay);

        //Earth Rotation
        earth.transform.rotation = Quaternion.Euler(timeOfDay * 270, 45 ,0);
        if(timeOfDay  > 0.8) earth.transform.rotation = Quaternion.identity; // Reset cus stoopid at maths
        
        // Sun and Moon colors
        sun.intensity = Mathf.Max(Mathf.Sin(timeOfDay * 2 * Mathf.PI * 0.75f) * 2, 0);
        sun.color = sunColors.Evaluate(timeOfDay * 2f * 0.75f);

        moon.intensity = Mathf.Clamp(Mathf.Sin(timeOfDay * 2 * Mathf.PI - Mathf.PI) * 0.5f, 0, .2f);
        moon.shadowStrength = Mathf.Clamp(timeOfDay *100 - 67.5f, 0 ,1);
       
    }
}
