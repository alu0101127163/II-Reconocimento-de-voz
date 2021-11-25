# II-Reconocimento-de-voz

## Demostracion

![Foo](https://github.com/alu0101127163/II-Reconocimento-de-voz/blob/main/img/Ejecucion.gif)

Se puede observar en pantalla la existencia de dos botones uno para el KeywordRecognizer y otro para el DictationRecognizer. Hay que tener en cuenta que ambas no pueden ser activas al mismo tiempo.

Para el KeywordRecognizer tenemos varios comandos para ir eliminando objetos de la escena (como se puede observar); y para el DictationRecognizer simplemente ponemos en pantalla lo que vamos diciendo. 

## KeywordRecognizer

```c#
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordScript : MonoBehaviour
{
	private bool activated_ = false;

  [SerializeField]
  public string[] m_Keywords;

  private KeywordRecognizer m_Recognizer;

  void Start()
  {
  	m_Keywords = new string[] {"Cubos", "Bolas", "Cilindros"};
    m_Recognizer = new KeywordRecognizer(m_Keywords);
    m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
  }

  private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
  {
    StringBuilder builder = new StringBuilder();
    builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
    builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
    builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
    Debug.Log(builder.ToString());
    Accion(args.text);
    }

  void OnGUI() 
	{

		if(!activated_) // No esta en uso
		{
			if(GUI.Button(new Rect(Screen.width/2+300, Screen.height/2-250, 200, 50), "KeywordRecognizer"))
			{
       	m_Recognizer.Start();
       	activated_ = true;
			}
		}
		else 
		{
			if(GUI.Button(new Rect(Screen.width/2+300, Screen.height/2-250, 200, 50), "Stop KeywordRecognizer"))
			{
				m_Recognizer.Stop();
				activated_ = false;
			}
				
			GUI.Label(new Rect(Screen.width/2+340, Screen.height/2-200, 200, 50), "Listening...");
		}
	}

	private void OnDestroy()
  {
    m_Recognizer.Dispose();
  }

  private void Accion (string word)
  {

  	switch (word)
  	{
  		case "Cubos": 
  		{
  			GameObject[] cubos;
        cubos = GameObject.FindGameObjectsWithTag("Cubo");        
        
        foreach(GameObject cubo in cubos) {
          cubo.GetComponent<Renderer>().enabled=false;
        }        
  		}
  		break;

  		case "Bolas": 
  		{
  			GameObject[] bolas;
        bolas = GameObject.FindGameObjectsWithTag("Bola");        
        
        foreach(GameObject bola in bolas) {
          bola.GetComponent<Renderer>().enabled=false;
        }        
  		}
  		break;

  		case "Cilindros": 
  		{
  			GameObject[] cilindros;
        cilindros = GameObject.FindGameObjectsWithTag("Cilindro");        
        
        foreach(GameObject cilindro in cilindros) {
          cilindro.GetComponent<Renderer>().enabled=false;
        }        
  		}
  		break;

  		default:
  		break;
  	}
  }
}
```

En mi caso he añadido un boton para "encnderlo" y "apargarlo"; además muestro por consola si se han escuchado o no las palabras claves. He puesto como palabras clave: ```{"Cubos", "Bolas", "Cilindros"};``` donde si digo "Cubos" elimina a todos los cubos, si digo "Bolas" pues elimina a todas las esferas y por ultimo si digo "Cilindros" elimina a todos los cilindros.

## DictationRecognizer

```c#
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour
{
		private bool activated_ = false;

    [SerializeField]
    private Text m_Hypotheses;

    [SerializeField]
    private Text m_Recognitions;

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            m_Recognitions.text += text + "\n";
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            m_Hypotheses.text += text;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

    }

    void OnDestroy()
    {
        m_DictationRecognizer.Dispose();
    }

  void OnGUI() 
	{

		if(!activated_) // No esta en uso
		{
			if(GUI.Button(new Rect(Screen.width/2+300, Screen.height/2-180, 200, 50), "DictationRecognizer"))
			{
       	m_DictationRecognizer.Start();
       	activated_ = true;
			}
		}
		else 
		{
			if(GUI.Button(new Rect(Screen.width/2+300, Screen.height/2-180, 200, 50), "Stop DictationRecognizer"))
			{
				m_DictationRecognizer.Stop();
				activated_ = false;
			}
				
			GUI.Label(new Rect(Screen.width/2+340, Screen.height/2-130, 200, 50), "Listening...");
		}
	}

}
```

Por último en DictationRecognizer simplemente mostramos el texto que estamos diciendo. Para ello tenemos un texto en la pantalla que nos muestra lo que decimos. Hice también un boton para poder acticarlo y desactivarlo cuando queramos.
