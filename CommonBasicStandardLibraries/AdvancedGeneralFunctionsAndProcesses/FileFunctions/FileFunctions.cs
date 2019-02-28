﻿using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.Exceptions;
using System.IO;
using CommonBasicStandardLibraries.CollectionClasses;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.ListsExtensions;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions
{
    public static class FileFunctions
    {
        //i think that setting the path caused too many problems.  so that will no longer be static

        public static string GetApplicationDataForMobileDevices()
        {
            // Dim ThisPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // i think this is where it should be.
        }

        public static string GetSDCardReadingPathForAndroid()
        {
            if (DirectoryExists("/storage/sdcard1") == true)
                return "/storage/sdcard1"; // can't assume its always accessing the music folder
            else if (DirectoryExists("/storage/emulated/0") == true)
                return "/storage/emulated/0";
            else
                throw new Exception("Either an unusual situation or not even an android device");
        }

        public static string GetWriteLocationForExternalOnAndroid()
        {
            return "/storage/emulated/0"; // for writing, its always this location.  so for cristina, it will be internal.  for andy, external but the path is still the same
        }

        public static bool FileExists(string FilePath)
        {
            return System.IO.File.Exists(FilePath);
        }

        public static bool DirectoryExists(string DirectoryPath)
        {
            return System.IO.Directory.Exists(DirectoryPath);
        }

        public static async  Task<string> GetDriveLetterAsync(string Label)
        {
            string ThisStr = default;
            await Task.Run(() =>
            {
                var CompleteDrives = System.IO.DriveInfo.GetDrives();
                //System.IO.DriveInfo Temps;
                foreach (var Temps in CompleteDrives)
                {
                    if (Temps.DriveType == System.IO.DriveType.Fixed)
                    {
                        if (Temps.IsReady == true)
                        {
                          if (Label.ToLower() == Temps.VolumeLabel.ToLower())
                               ThisStr =  Temps.Name.Substring(0, 1);
                           break;
                        } 
                    }
                }
            } 
            );

            if (ThisStr == default)
            {
                throw new BasicBlankException("No label for " + Label);
            }
            return ThisStr;
        }  

		public static async Task FilterDirectory(CustomBasicList<string> FirstList)
		{
			CustomBasicList<string> RemoveList = new CustomBasicList<string>();
			await Task.Run(() =>
			{
				FirstList.ForEach(Items =>
				{
					if (System.IO.Directory.EnumerateFiles(Items).Count() == 0)
						RemoveList.Add(Items);
				});
				
			});
			FirstList.RemoveGivenList(RemoveList, System.Collections.Specialized.NotifyCollectionChangedAction.Remove); //i think
		}

        public static async  Task<CustomBasicList<string>> GetDriveListAsync()
        {

            System.IO.DriveInfo[] CompleteDrives = null;
            CustomBasicList<string> NewList = new CustomBasicList<string>();
            await Task.Run(() =>
            {
                CompleteDrives = System.IO.DriveInfo.GetDrives();
                foreach (var Temps in CompleteDrives)
                {
                    if (Temps.DriveType == System.IO.DriveType.Fixed)
                    {
                        if (Temps.IsReady == true)
                            NewList.Add(Temps.RootDirectory.FullName);
                    }
                }
            }
            );
            
            return NewList;
        }

        private static DateTime GetCreatedDate(System.IO.FileInfo ThisFile)
        {
            if (ThisFile.CreationTime.IsDaylightSavingTime() == true)
                return ThisFile.CreationTime.AddHours(-1);
            return ThisFile.CreationTime;
        }

        private static DateTime GetModifiedDate(System.IO.FileInfo ThisFile)
        {
            if (ThisFile.LastWriteTime.IsDaylightSavingTime() == true)
                return ThisFile.LastWriteTime.AddHours(-1);
            return ThisFile.LastWriteTime;
        }

        public static string GetParentPath(string ThisPath)
        {
            var ThisDir = System.IO.Directory.GetParent(ThisPath);
            return ThisDir.FullName;
        }

        public static async Task <FileInfo> GetFileAsync(string FilePath)
        {
            System.IO.FileInfo ThisFile;
            FileInfo TempFile = null;
            await Task.Run(() =>
            {
               ThisFile = new System.IO.FileInfo(FilePath);

               TempFile = new FileInfo()
               {
                   FileSize = ThisFile.Length,
                   DateCreated = GetCreatedDate(ThisFile),
                   Directory = ThisFile.DirectoryName,
                   DateAccessed = ThisFile.LastAccessTime,
                   DateModified = GetModifiedDate(ThisFile),
                   FilePath = FilePath // this is needed so if i only have the fileinfo, i know the full path.
               };
               if (TempFile.DateModified > TempFile.DateAccessed)
                   TempFile.DateAccessed = TempFile.DateModified;// this means something is wrong.
            });           
            return TempFile;
        }

        //since i have more async support now, its best to use more awaits.


        public static string DirectoryName(string DirectoryPath)
        {
            var ThisItem = new System.IO.DirectoryInfo(DirectoryPath);
            return ThisItem.Name;
        }

        public static string FullFile(string FilePath)
        {
            var ThisItem = new System.IO.FileInfo(FilePath);
            return ThisItem.Name;
        }

        public static string FileName(string FilePath)
        {
            var ThisItem = new System.IO.FileInfo(FilePath);
            var ThisName = ThisItem.Name;
            return ThisName.Substring(0, ThisName.Count() - ThisItem.Extension.Count());
        }

        public static async Task<CustomBasicList<string>> DirectoryListAsync(string WhatPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var TDirectoryList = new CustomBasicList<string>();
            //IEnumerable<string> TempList;
            await Task.Run(() =>
            {
                TDirectoryList = System.IO.Directory.EnumerateDirectories(WhatPath, "", searchOption).ToCustomBasicList();
                //foreach (var ThisItem in TempList)
                //    TDirectoryList.Add(ThisItem);
            });
            return TDirectoryList;
        }

		public static async Task<CustomBasicList<string>> FileListAsync(string DirectoryPath)
        {
            CustomBasicList<string> tFileList = new CustomBasicList<string>();
            //IEnumerable<string> TempList;
            await Task.Run(() =>
            {
                tFileList = System.IO.Directory.EnumerateFiles(DirectoryPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToCustomBasicList(); // hopefully will still work
                                                                                                                            // Dim Tests=System.IO.Directory.EnumerateFiles(DirectoryPath, )
                //foreach (var ThisItem in TempList)
                //    tFileList.Add(ThisItem);
            });
            return tFileList;
        }

		public static async Task<CustomBasicList<string>> FileListAsync(CustomBasicList<string> DirectoryList)
		{
			CustomBasicList<string> NewList = new CustomBasicList<string>();
			await DirectoryList.ForEachAsync(async x =>
			{
				var Temps = await FileListAsync(x);
				NewList.AddRange(Temps);
			});
			return NewList;
		}

        private static async  Task<string> PrivateAllTextAsync(string FilePath, Encoding encodes)
        {
            string ThisText;
            using (StreamReader s = new StreamReader(FilePath, encodes))
            {
                ThisText = await s.ReadToEndAsync();
                s.Close();
            }
            return ThisText;
        }

        private static async Task PrivateWriteAllTextAsync(string FilePath, string Text, bool Append, Encoding encoding, bool IsLine = false)
        {
            using (StreamWriter w = new StreamWriter(FilePath, Append, encoding))
            {
                if (IsLine == true)
                    await w.WriteLineAsync(Text);
                else
                    await w.WriteAsync(Text);
                await w.FlushAsync();
                w.Close();
            }
        }

        public static async Task<string> AllTextAsync(string FilePath)
        {
            return await PrivateAllTextAsync(FilePath, Encoding.Default); //i think
            
        }

        public static async Task<string> AllTextAsync(string FilePath, Encoding encoding)
        {
            return await PrivateAllTextAsync(FilePath, encoding);
        }


        public static async Task  WriteAllTextAsync(string FilePath, string WhatText)
        {
            await PrivateWriteAllTextAsync(FilePath, WhatText, false, Encoding.UTF8);
        }

        public static async Task WriteTextAsync(string FilePath, string WhatText, bool Append)
        {
            await PrivateWriteAllTextAsync(FilePath, WhatText, Append, Encoding.Default, true);
        }

        public static async Task FileCopyAsync(string OriginalFile, string NewFile)
        {
            await Task.Run(() => File.Copy(OriginalFile, NewFile, true));
        }

        public static async Task DeleteFolderAsync(string Path) //i like just delete folder instead
        {
            await Task.Run(() => Directory.Delete(Path));
        }

        public static async Task CreateFolderAsync(string Path)
        {
            await Task.Run(() => Directory.CreateDirectory(Path));
        }

        public static async Task DeleteFileAsync(string Path)
        {
            await Task.Run(() => File.Delete(Path));
        }

        public static async Task RenameFileAsync(string OldFile, string NewName)
        {
            await Task.Run(() => File.Move(OldFile, NewName));
        }

        public static async Task RenameDirectoryAsync(string OldDirectory, string NewName)
        {
            await Task.Run(() => Directory.Move(OldDirectory, NewName));
        }

        public static async  Task<CustomBasicList<string>> TextFromFileListAsync(string FilePath)
        {
            CustomBasicList<string> ThisList = new CustomBasicList<string>();
            await Task.Run(() => ThisList = File.ReadAllLines(FilePath).ToCustomBasicList());
            return ThisList;
        }


        public static Stream GetStreamForReading(string FilePath) // some classes require the actual stream.  therefore will use this
        {
            return new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        } // this

    }
}