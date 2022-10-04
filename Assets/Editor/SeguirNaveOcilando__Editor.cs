using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SeguirNaveOcilando))]
public class SeguirNaveOcilando__Editor : Editor
{
    //Este script solo modifica el inspector de la script
    //El codigo es sacado de: https://www.youtube.com/watch?v=RInUu1_8aGw

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SeguirNaveOcilando _seguirNaveOcilando = (SeguirNaveOcilando)target;

        if (GUILayout.Button("Ubicar en Posicion Inicial"))
        {
            Transform _centro = _seguirNaveOcilando.centro;
            Transform _satelite = _seguirNaveOcilando.satelite;
            _seguirNaveOcilando.radio = _seguirNaveOcilando.CalcularRadio(_centro, _satelite);

            #region Explicacion: Referencias en editor scripts y movimiento
            /* Las funciones ejecutadas en el inspector no son in-game, por lo que muchas referencias 
             * que son buscadas en el "awake" son "null".
             * Por ello, hay que buscar todas las referencias necesarias en el script "editor"
             *
             * Por otro lado, creo que el script "editor" solo puede mover objetos con "transform"
             * y no con rigidbodies. Por ello tuve que duplicar la funcion que genera el movimeinto circular
             * pero con "transform.position" enves de "rigidbody.MovePostion"
             */
            #endregion
            MovimientoCircularPlayer naveScript = FindObjectOfType<MovimientoCircularPlayer>();
            _seguirNaveOcilando.rotacion = naveScript.rotacionInicial;
            _seguirNaveOcilando.MoverAPosicionInicial();
        }
    }

}