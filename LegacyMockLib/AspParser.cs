using System.Reflection;

namespace LegacyMockLib;

public class AspParser {
    Assembly _assembly;
    string _rootDir;

    public AspParser(Assembly assembly, string workingDir) {
        _assembly = assembly;
        _rootDir = workingDir;
    }
    public AspParser() : this(Assembly.GetCallingAssembly(), AppDomain.CurrentDomain.BaseDirectory) { }

    /// <summary> Parse class name from <em>.svc</em>/<em>.asmx</em> file </summary>
    /// <param name="data">string data from <em>.svc</em>/<em>.asmx</em> file</param>
    /// <returns><see cref="Type"/> of founded class name</returns>
    /// <exception cref="Exception"></exception>
    public Type ParseFile(string data) {
        int state = 0;
        string buff = "";
        int type = 0;
        for(var i = 0; i < data.Length; i++) {
            switch(state) {
                case 0:
                    if ('<' != data[i]) throw new Exception("broken file");
                    state = 1;
                    continue;
                case 1:
                    if ('%' != data[i]) throw new Exception("broken file");
                    state = 2;
                    continue;
                case 2:
                    if ('@' != data[i]) throw new Exception("broken file");
                    state = 3;
                    continue;
                case 3:
                    if (char.IsWhiteSpace(data[i])) continue;
                    if (!char.IsAsciiLetter(data[i])) throw new Exception("broken file");
                    buff = data[i].ToString();
                    state = 4;
                    continue;
                case 4:
                    if (char.IsWhiteSpace(data[i])) {
                        if ("ServiceHost" == buff) type = 1;
                        if ("WebService" == buff) type = 2;
                        if (0 == type) throw new Exception("not service");
                        state = 5;
                        buff = "";
                        continue;
                    }
                    if (!char.IsAsciiLetter(data[i])) throw new Exception("broken file");
                    buff += data[i].ToString();
                    continue;
                case 5:
                    if (char.IsWhiteSpace(data[i])) continue;
                    if (!char.IsAsciiLetter(data[i])) throw new Exception("broken file");
                    buff = data[i].ToString();
                    state = 6;
                    continue;
                case 6:
                    if ('=' == data[i]) {
                        if ("Service" == buff || "Class" == buff) state = 9;
                        else state = 7;
                        buff = "";
                        continue;
                    }
                    if (char.IsWhiteSpace(data[i])) {
                        buff = "";
                        state = 5;
                        continue;
                    }
                    if (!char.IsAsciiLetter(data[i])) throw new Exception("broken file");
                    buff += data[i].ToString();
                    continue;
                case 7:
                    if ('"' == data[i]) {
                        state = 8;
                        continue;
                    }
                    if (char.IsWhiteSpace(data[i])) {
                        state = 5;
                        continue;
                    }
                    throw new Exception("broken file");
                case 8:
                    if ('"' == data[i]) {
                        state = 5;
                        continue;
                    }
                    continue;
                case 9:
                    if ('"' == data[i]) {
                        state = 10;
                        continue;
                    }
                    throw new Exception("broken file");
                case 10:
                    if ('"' == data[i]) {
                        state = 11;
                        continue;
                    }
                    buff += data[i];
                    continue;    
                case 11:
                    break;
            }
            break;
        }
        return _assembly.GetType(buff);
    }

    public Dictionary<string, Type> ParseDirectory(string path, Dictionary<string, Type> res) {
        var di = new DirectoryInfo(path);
        foreach(var fi in di.GetFiles()) {
            if (".svc" != fi.Extension && ".asmx" != fi.Extension) continue;
            using var sr = fi.OpenText();
            var s = sr.ReadToEnd();
            var type = ParseFile(s);
            res.Add(fi.FullName.Replace(di.FullName, ""), type);
        }
        return res;
    }

    public Dictionary<string, Type> ParseWorkDirectory() {
        return ParseDirectory(_rootDir, new Dictionary<string, Type>());
    }
}