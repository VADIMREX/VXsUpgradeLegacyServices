using System.Reflection;

namespace LegacyMockLib;

/// <summary> ASP File type </summary>
public enum AspFileType {
    Unknown,
    Svc,
    Asmx
}

public class AspParser {
    /// <summary> States of ASP File parser </summary>
    enum AspFileParsingStates {
        /// <summary> Begining of file, parsing opening <c>&lt;%@</c> of tag.<br />
        /// <c>&lt;</c> is expected </summary>
        Start,
        /// <summary> Continue parsing opening <c>&lt;%@</c> of tag.<br />
        /// <c>%</c> is expected </summary>
        OpenTagPart1,
        /// <summary> Continue parsing opening <c>&lt;%@</c> of tag.<br />
        /// <c>@</c> is expected </summary>
        OpenTagPart2,
        /// <summary> Space after opening of tag.<br />
        /// Any count of white space characters is expected </summary>
        WiteSpaceAfterOpenTag,
        /// <summary> Parsing of first Attribute - File Type.<br />
        /// Only <c>ServiceHost</c> and <c>WebService</c> are supported.<br />
        /// Any ASCII letter is expected </summary>
        FileTypeStart,
        /// <summary> Rest of first Attribute</summary>
        FileTypeEnd,
        /// <summary> Parsing rest attributes<br />
        /// Any ASCII letter is expected </summary>
        AttributeStart,
        /// <summary> rest of attribute</summary>
        AttributeEnd,
        /// <summary> Parsing attribute value<br />
        /// <c>"</c> expected </summary>
        AttributeValueStart,
        /// <summary> rest of attribute value</summary>
        AttributeValueEnd,
        /// <summary> Parsing attribute value containing class name<br />
        /// <c>"</c> expected </summary>
        ClassNameStart,
        /// <summary> rest of attribute value containing class name</summary>
        ClassNameEnd
    }

    /// <summary> Parse class name from <c>.svc</c> or <c>.asmx</c> file </summary>
    /// <param name="data">string data from <em>.svc</em>/<em>.asmx</em> file</param>
    /// <returns>founded class name and file type</returns>
    /// <exception cref="Exception"/>parsing error with position and description</exception>
    public static (string className, AspFileType fileType) ParseAspFile(string data) {
        var state = AspFileParsingStates.Start;
        var buff = "";
        var type = AspFileType.Unknown;
        for(var i = 0; i < data.Length; i++) {
            switch(state) {
                case AspFileParsingStates.Start:
                    if ('<' != data[i]) throw new Exception($"Incorrect file start, pos - {i}, '<' expected but '{data[i]}' found");
                    state = AspFileParsingStates.OpenTagPart1;
                    continue;
                case AspFileParsingStates.OpenTagPart1:
                    if ('%' != data[i]) throw new Exception($"Incorrect file start, pos - {i}, '%' expected but '{data[i]}' found");
                    state = AspFileParsingStates.OpenTagPart2;
                    continue;
                case AspFileParsingStates.OpenTagPart2:
                    if ('@' != data[i]) throw new Exception($"Incorrect file start, pos - {i}, '@' expected but '{data[i]}' found");
                    state = AspFileParsingStates.WiteSpaceAfterOpenTag;
                    continue;
                case AspFileParsingStates.WiteSpaceAfterOpenTag:
                    if (!char.IsWhiteSpace(data[i])) throw new Exception($"Incorrect file start, pos - {i}, white space expected but '{data[i]}' found");
                    state = AspFileParsingStates.FileTypeStart;
                    continue;
                case AspFileParsingStates.FileTypeStart:
                    if (char.IsWhiteSpace(data[i])) continue;
                    if (!char.IsAsciiLetter(data[i])) throw new Exception($"Incorrect file type, pos - {i}, ASCII letter expected but '{data[i]}' found");
                    buff = data[i].ToString();
                    state = AspFileParsingStates.FileTypeEnd;
                    continue;
                case AspFileParsingStates.FileTypeEnd:
                    if (char.IsWhiteSpace(data[i])) {
                        if ("ServiceHost" == buff) type = AspFileType.Svc;
                        if ("WebService" == buff) type = AspFileType.Asmx;
                        if (0 == type) throw new Exception($"Incorrect file type, pos - {i}, ServiceHost or WebService expected but {buff} found");
                        state = AspFileParsingStates.AttributeStart;
                        buff = "";
                        continue;
                    }
                    if (!char.IsAsciiLetter(data[i])) throw new Exception($"Incorrect file type, pos - {i}, ASCII letter expected but '{data[i]}' found");
                    buff += data[i].ToString();
                    continue;
                case AspFileParsingStates.AttributeStart:
                    if (char.IsWhiteSpace(data[i])) continue;
                    if (!char.IsAsciiLetter(data[i])) throw new Exception($"Incorrect attribute name, pos - {i}, ASCII letter expected but '{data[i]}' found");
                    buff = data[i].ToString();
                    state = AspFileParsingStates.AttributeEnd;
                    continue;
                case AspFileParsingStates.AttributeEnd:
                    if ('=' == data[i]) {
                        if ("Service" == buff || "Class" == buff) state = AspFileParsingStates.ClassNameStart;
                        else state = AspFileParsingStates.AttributeValueStart;
                        buff = "";
                        continue;
                    }
                    if (char.IsWhiteSpace(data[i])) {
                        buff = "";
                        state = AspFileParsingStates.AttributeStart;
                        continue;
                    }
                    if (!char.IsAsciiLetter(data[i])) throw new Exception($"Incorrect attribute name, pos - {i}, ASCII letter expected but '{data[i]}' found");
                    buff += data[i].ToString();
                    continue;
                case AspFileParsingStates.AttributeValueStart:
                    if ('"' == data[i]) {
                        state = AspFileParsingStates.AttributeValueEnd;
                        continue;
                    }
                    if (char.IsWhiteSpace(data[i])) {
                        state = AspFileParsingStates.AttributeStart;
                        continue;
                    }
                    throw new Exception($"Incorrect attribute value, pos - {i}, '\"' expected but '{data[i]}' found");
                case AspFileParsingStates.AttributeValueEnd:
                    if ('"' == data[i]) {
                        state = AspFileParsingStates.AttributeStart;
                        continue;
                    }
                    continue;
                case AspFileParsingStates.ClassNameStart:
                    if ('"' == data[i]) {
                        state = AspFileParsingStates.ClassNameEnd;
                        continue;
                    }
                    throw new Exception($"Incorrect attribute value, pos - {i}, '\"' expected but '{data[i]}' found");
                case AspFileParsingStates.ClassNameEnd:
                    if ('"' == data[i]) break;
                    buff += data[i];
                    continue;    
            }
            break;
        }
        return (buff, type);
    }

    /// <summary> Recursevely parse <paramref name="directory"/> with dictionary to be populated corresponding class name with file path </summary>
    /// <param name="directory">Path to the directory</param>
    /// <param name="fileClass">Dictionary to be populated corresponding class name with file path </param>
    /// <returns> Dictionary corresponding class name with file path </returns>
    public static Dictionary<string, List<string>> ParseDirectory(string rootPath, DirectoryInfo directory, Dictionary<string, List<string>> fileClass) {
        foreach(var fi in directory.GetFiles()) {
            if (".svc" != fi.Extension && ".asmx" != fi.Extension) continue;
            using var sr = fi.OpenText();
            var s = sr.ReadToEnd();
            var (className, _) = ParseAspFile(s);

            if (!fileClass.ContainsKey(className)) fileClass.Add(className, new List<string>());
            var relativePath = fi.FullName.Replace(rootPath, "");
            if ('/' != Path.DirectorySeparatorChar) relativePath = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            fileClass[className].Add(relativePath);
        }
        foreach(var subdi in directory.GetDirectories())
            fileClass = ParseDirectory(rootPath, subdi, fileClass);
        return fileClass;
    }

    /// <summary> Recursevely parse directory at specified <paramref name="path"/></summary>
    /// <param name="path">Path to the directory</param>
    /// <returns> Dictionary corresponding class name with file path </returns>
    public static Dictionary<string, List<string>> ParseDirectory(string path) {
        var di = new DirectoryInfo(path);
        return ParseDirectory(path, di, new Dictionary<string, List<string>>());
    }
}