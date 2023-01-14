using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

/// <summary>
/// ファイルマネージャー
/// </summary>
public class FileManager : SingletonMonoBehaviour<FileManager> {

    private readonly string[] EXTENSIONS = {
        ".png",
        ".jpg",
    };

    public class FilePath {
        public string Path { private set; get; }

        public FilePath(string path) {
            Path = path;
        }

        public static FilePath[] Create(string[] files) {
            var filePaths = new List<FilePath>(files.Length);
            for (int i = 0; i < files.Length; i++) {
                filePaths.Add(new FilePath(files[i]));
            }
            return filePaths.ToArray();
        }
    }

    private Dictionary<string, FilePath[]> _filePathDict = new();
    public IReadOnlyDictionary<string, FilePath[]> FilePathDict => _filePathDict;

    private List<FilePath> _filePathList = new();
    public IReadOnlyList<FilePath> FilePathList => _filePathList;

    /// <summary>
    /// フォルダディレクトリだけ受け取る
    /// </summary>
    /// <param name="path"></param>
    public void SetDirectory(string path) {
        StartCoroutine(FileSearchCoroutine(path));
    }

    /// <summary>
    /// ファイル検索（コルーチン）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator FileSearchCoroutine(string path) {
        Common.Instance.LoadingActive(true);

        if (!Util.DirectoryExists(path)) {
            Common.Instance.LoadingActive(false);
            yield break;
        }

        Common.Instance.DisplayStop();
        _filePathDict.Clear();
        _filePathList.Clear();

        yield return null;

        // 指定ディレクトリを検索
        {
            var files = Util.GetFiles(path, SearchOption.TopDirectoryOnly, EXTENSIONS);
            var filePaths = FilePath.Create(files);
            _filePathDict.Add(path, filePaths);
            _filePathList.AddRange(filePaths);
        }

        // サブディレクトリを全検索する
        {
            yield return null;
            var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);

            yield return null;
            foreach (var folderStr in directories) {
                var files = Util.GetFiles(folderStr, SearchOption.AllDirectories, EXTENSIONS);
                var filePaths = FilePath.Create(files);
                _filePathDict.Add(folderStr, filePaths);
                _filePathList.AddRange(filePaths);
                yield return null;
            }
        }

        Common.Instance.LoadingActive(false);
        Common.Instance.DisplayStart();
    }
}
