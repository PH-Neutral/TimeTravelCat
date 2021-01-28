using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour {
    public static LanguageManager Instance;

    /// <summary>
    /// The path to the file from the "Assets" folder. (ex: "testFoler/files" would translate to "[projectPath]/Assets/testFolder/files")
    /// </summary>
    [SerializeField] string languageFileFolderPath = "";
    /// <summary>
    /// The name of the file with its extension. (ex: "fileName.txt")
    /// </summary>
    [SerializeField] string languageFileName = "";
    [SerializeField] Lang selectedLanguage = Lang.French;

    Dictionary<string, LangItem> items;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        LoadLanguageFile();
    }

    void LoadLanguageFile() {
        string path = Path.Combine(Application.dataPath, languageFileFolderPath, languageFileName); // get the file path
        if(!File.Exists(path)) {
            Debug.LogError("Language File could not be found at: " + path);
            return;
        }
        FileStream st = File.OpenRead(path); // open the file for reading only and get a fileStream from it
        List<string> lines = new List<string>();
        using(StreamReader sr = new StreamReader(st)) { // open a stream dedicated to reading fileStreams
            while(sr.Peek() >= 0) { // check if there is still something to read
                lines.Add(sr.ReadLine()); // get the current line of the file
            }
        }
        st.Close(); // close the file to avoid conflicts
        //Debug.Log("File successfuly read! (" + lines.Count + " lines extracted)");

        items = new Dictionary<string, LangItem>();
        for (int i=1; i<lines.Count; i++) { // convert extracted lines into LangItem objects and store them in a dictionary
            LangItem item = ConvertLineToItem(lines[i]);
            if (item == null) { 
                Debug.LogWarning("LanguageFile line could not be converted. Please check file syntax to fix the problem."); 
                continue; 
            }
            items[item.Key] = item;
        }

        /*/ +++++ DEBUG +++++ //
        Debug.Log("Loaded LangItems:");
        string itemStr;
        foreach(LangItem item in items.Values) {
            itemStr = "id: " + item.id + "; name: " + item.name;
            for (int i = 0; i < item.translations.Length; i++) {
                itemStr += "; " + ((Lang)i).ToString() + ": " + item.translations[i];
            }
            itemStr += ";";
            Debug.Log(itemStr);
        }//*/
    }

    LangItem ConvertLineToItem(string line) {
        List<string> elements = new List<string>();
        string temp = "";
        for (int i=0; i<line.Length; i++) { // separate line with tabulations as separator
            if (line[i] == '\t') {
                elements.Add(temp);
                temp = "";
            } else {
                temp += line[i];
                if (i == line.Length - 1) {
                    elements.Add(temp);
                }
            }
        }

        if (elements.Count < 4) { return null; }
        if (!int.TryParse(elements[0].Trim(), out int id)) { return null; } // get the id
        elements.RemoveAt(0);
        string action = elements[0].Trim().ToLower(); // get the action
        elements.RemoveAt(0);
        string target = elements[0].Trim().ToLower(); // get the target
        elements.RemoveAt(0);
        string other = elements[0].Trim().ToLower(); // get the other
        elements.RemoveAt(0);
        string[] translations = elements.ToArray();

        return new LangItem(id, action, target, other, translations);
    }

    private void Start() {
        /*/ +++++ DEBUG +++++ //
        Debug.Log("Loaded LangItems:");
        string itemStr;
        foreach(string k in items.Keys) {
            itemStr = "key: " + k + "; text: " + GetText(k) + ";";
            Debug.Log(itemStr);
        }//*/
    }

    public void ChangeLanguage(Lang lang) {
        selectedLanguage = lang;
    }

    public string GetText(Action act, InteractableType target, InteractableType other) {
        return GetText(act.ToString().ToLower() + target.ToString().ToLower() + other.ToString().ToLower());
    }

    public string GetText(string textKey) {
        if (!items.ContainsKey(textKey)) {
            throw new TextNotFoundException();
            //return "[NO TRANSLATION IS LINKED TO THAT KEY (key: " + textKey + ")]";
        }
        return items[textKey].GetText();
    }

    public class LangItem {
        public string Key {
            get { return action + target + other; }
        }
        public int id;
        public string action;
        public string target;
        public string other;
        public string[] translations;

        public LangItem(int id, string action, string target, string other, string[] translations) {
            this.id = id;
            this.action = action;
            this.target = target;
            this.other = other;
            this.translations = translations;
        }

        public string GetText() {
            int langIndex = (int)Instance.selectedLanguage;
            if (translations.Length < langIndex + 1) { return "[NO TRANSLATON WAS FOUND FOR THAT LANGUAGE (id: " + id + ")]"; }
            return translations[langIndex];
        }
    }

    public enum Lang {
        French, English
    }
}
