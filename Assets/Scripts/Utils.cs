using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public static class Util {

    /// <summary>
    /// マウスボタン
    /// </summary>
    public enum MouseButtonEnum {
        /// <summary>左ボタン</summary>
        LEFT,
        /// <summary>右ボタン</summary>
        RIGHT,
        /// <summary>中央ボタン</summary>
        MIDDLE,
    }

    public static bool FileExists(string path) {
        if (string.IsNullOrEmpty(path)) {
            return false;
        }

        if (!File.Exists(path)) {
            return false;
        }

        try {
            return true;
        }
        catch (FileNotFoundException e) {
            Debug.Log(e);
            return false;
        }
    }

    public static bool DirectoryExists(string path) {
        if (string.IsNullOrEmpty(path)) {
            return false;
        }

        if (!Directory.Exists(path)) {
            return false;
        }

        try {
            return true;
        }
        catch (FileNotFoundException e) {
            return false;
        }
    }

    /// <summary>
    /// 指定ディレクトリ内から指定拡張子のファイルパスを返す
    /// </summary>
    /// <param name="path">ディレクトリ指定</param>
    /// <param name="searchOption">検索オプション</param>
    /// <param name="extensions">拡張子</param>
    /// <returns>string[]</returns>
    public static string[] GetFiles(string path, SearchOption searchOption, params string[] extensions) {
        return Directory.GetFiles(path, "*.*", searchOption).Where(v => {
            return extensions.Any(extension => {
                return v.EndsWith(extension);
            });
        }).ToArray();
    }

    /// <summary>
    /// 指定パスを開く
    /// </summary>
    /// <param name="path"></param>
    public static void OpenPath(string path) {
        var fileInfo = new FileInfo(path);
        var directoryInfo = fileInfo.Directory;
        Application.OpenURL(directoryInfo.FullName);
    }
}
