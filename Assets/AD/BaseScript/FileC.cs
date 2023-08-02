using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AD.BASE
{
    public static class FileC
    {
        //生成这个文件的文件路径（不包括本身）
        public static void TryCreateDirectroryOfFile(string filePath)
        {
            Debug.Log($"CreateDirectrory {filePath}[folder_path],");
            if (!string.IsNullOrEmpty(filePath))
            {
                var dir_name = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir_name))
                {
                    Debug.Log($"No Exists {dir_name}[dir_name],");
                    Directory.CreateDirectory(dir_name);
                }
                else
                {
                    Debug.Log($"Exists {dir_name}[dir_name],");
                }
            }
        }

        //移动整个路径
        public static void MoveFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建 
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception(" public static void MoveFolder(string sourcePath, string destPath),Target Directory fail to create" + ex.Message);
                        Debug.LogWarning("public static void MoveFolder(string sourcePath, string destPath),Target Directory fail to create" + ex.Message);
                        return;
                    }
                }
                //获得源文件下所有文件 
                List<string> files = new(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //覆盖模式 
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(c, destFile);
                });
                //获得源文件下所有目录文件 
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));

                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //Directory.Move必须要在同一个根目录下移动才有效，不能在不同卷中移动。 
                    //Directory.Move(c, destDir); 

                    //采用递归的方法实现 
                    MoveFolder(c, destDir);
                });
            }
            else
            {
                //throw new Exception(" public static void MoveFolder(string sourcePath, string destPath),sourcePath cannt find");
                Debug.Log("public static void MoveFolder(string sourcePath, string destPath),sourcePath cannt find");
            }
        }

        //拷贝整个路径
        public static void CopyFilefolder(string sourceFilePath, string targetFilePath)
        {
            //获取源文件夹中的所有非目录文件
            string[] files = Directory.GetFiles(sourceFilePath);
            string fileName;
            string destFile;
            //如果目标文件夹不存在，则新建目标文件夹
            if (!Directory.Exists(targetFilePath))
            {
                Directory.CreateDirectory(targetFilePath);
            }
            //将获取到的文件一个一个拷贝到目标文件夹中 
            foreach (string s in files)
            {
                fileName = Path.GetFileName(s);
                destFile = Path.Combine(targetFilePath, fileName);
                File.Copy(s, destFile, true);
            }
            //上面一段在MSDN上可以看到源码 

            //获取并存储源文件夹中的文件夹名
            string[] filefolders = Directory.GetFiles(sourceFilePath);
            //创建Directoryinfo实例 
            DirectoryInfo dirinfo = new DirectoryInfo(sourceFilePath);
            //获取得源文件夹下的所有子文件夹名
            DirectoryInfo[] subFileFolder = dirinfo.GetDirectories();
            for (int j = 0; j < subFileFolder.Length; j++)
            {
                //获取所有子文件夹名 
                string subSourcePath = sourceFilePath + "\\" + subFileFolder[j].ToString();
                string subTargetPath = targetFilePath + "\\" + subFileFolder[j].ToString();
                //把得到的子文件夹当成新的源文件夹，递归调用CopyFilefolder
                CopyFilefolder(subSourcePath, subTargetPath);
            }
        }

        public static void CopyFile(string sourceFile, string targetFilePath)
        {
            File.Copy(sourceFile, targetFilePath, true);
        }
        public static void DeleteFile(string sourceFile)
        {
            File.Delete(sourceFile);
        }


        //重命名文件
        public static void FileRename(string sourceFile, string newNameWithFullPath)
        {
            CopyFile(sourceFile, newNameWithFullPath);
            DeleteFile(sourceFile);
        }

        /*public static /*ExecutionResult void FileRename(string sourceFile, string destinationPath, string destinationFileName)
        {
            //ExecutionResult result;
            FileInfo tempFileInfo;
            FileInfo tempBakFileInfo;
            DirectoryInfo tempDirectoryInfo;

            //result = new ExecutionResult();
            tempFileInfo = new FileInfo(sourceFile);
            tempDirectoryInfo = new DirectoryInfo(destinationPath);
            tempBakFileInfo = new FileInfo(destinationPath + "\\" + destinationFileName);
            try
            {
                if (!tempDirectoryInfo.Exists)
                    tempDirectoryInfo.Create();
                if (tempBakFileInfo.Exists)
                    tempBakFileInfo.Delete();
                //move file to bak
                tempFileInfo.MoveTo(destinationPath + "\\" + destinationFileName);

            //    result.Status = true;
            //    result.Message = "Rename file OK";
            //    result.Anything = "OK";
            }
            catch (Exception ex)
            {
                //    result.Status = false;
                //    result.Anything = "Mail";
                //   result.Message = ex.Message;
                //    if (mesLog.IsErrorEnabled)
                //    {
                //        mesLog.Error(MethodBase.GetCurrentMethod().Name, "Rename file error. Msg :" + ex.Message);
                //        mesLog.Error(ex.StackTrace);
                //    }
                Debug.LogWarning(MethodBase.GetCurrentMethod().Name + "Rename file error. Msg :" + ex.Message);
            }

           // return result;
        }*/



        private static Dictionary<string, List<FileInfo>> Files = new();

        public static List<FileInfo> GetFiles(string group)
        {
            Files.TryGetValue(group, out var files);
            return files;
        }

        public static void LoadFiles(string group,string dictionary, Predicate<string> _Right)
        {
            if (Files.ContainsKey(group))
                Files[group] = Files[group].Union(FindAll(dictionary, _Right)).ToList();
            else Files[group] = FindAll(dictionary, _Right);
        }

        public static List<FileInfo> FindAll(string dictionary,string extension)
        {
            return FindAll(dictionary, T => Path.GetExtension(T) == extension);
        }

        public static List<FileInfo> FindAll(string dictionary, Predicate<string> _Right)
        {
            DirectoryInfo direction = new(dictionary);
            FileInfo[] files = direction.GetFiles("*");
            List<FileInfo> result = new();
            foreach (var it in files)
                if (_Right(it.Name)) result.Add(it);
            return result.Count == 0 ? result : null;
        }

        public static FileInfo First(string dictionary, string name)
        {
            return First(dictionary, T => Path.GetFileNameWithoutExtension(T) == name);
        }

        public static FileInfo First(string dictionary, Predicate<string> _Right)
        {
            DirectoryInfo direction = new(dictionary);
            FileInfo[] files = direction.GetFiles("*");
            foreach (var it in files)
                if (_Right(it.Name)) return it;
            return null;
        } 

    }
}