using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public static class Util {

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
}
