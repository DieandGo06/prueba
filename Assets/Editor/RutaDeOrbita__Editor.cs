using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RutaDeOrbita))]
public class RutaDeOrbita__Editor : Editor
{
    //Este script solo modifica el inspector de la script
    //El codigo es sacado de: https://www.youtube.com/watch?v=RInUu1_8aGw

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RutaDeOrbita rutaDeOrbita = (RutaDeOrbita)target;

        if (GUILayout.Button("Generar Orbita"))
        {
            rutaDeOrbita.radio = rutaDeOrbita.CalcularRadio();
            rutaDeOrbita.DibujarCirculo(100, rutaDeOrbita.radio);
        }
    }
}
