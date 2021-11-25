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

