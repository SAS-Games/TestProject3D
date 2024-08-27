using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalSelector : MonoBehaviour
{
    [SerializeField] private int m_LocaleId;
    private bool _active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void ChangeLocale()
    {
        if (!_active)
        {
            StartCoroutine(SetLocale(m_LocaleId));
        }
    }

    IEnumerator SetLocale(int localeId)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        GetComponent<DialogueTrigger>().ShowDialogue();
        _active = false;
    }

}
