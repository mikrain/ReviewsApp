using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace Reviews.Common
{
    public class LocalCacheHelper
    {
        public const string PROJECT_FOLDER = "Reviews";
        public const string PINNED = "Pinned";
        public const string RECENT = "Recent";
        public static ObservableCollection<Entry> RecentApps = new ObservableCollection<Entry>();
        public static ObservableCollection<Entry> PinnedApps = new ObservableCollection<Entry>();

        private static async void SavePinned()
        {
            await SaveProject(PINNED, PinnedApps, CreationCollisionOption.ReplaceExisting);
        }

        private static async void SaveRecent()
        {
            await SaveProject(RECENT, RecentApps, CreationCollisionOption.ReplaceExisting);
        }

        public async static Task<ObservableCollection<Entry>> OpenRecent()
        {
            var openedProject = await OpenProject(RECENT);
            if (openedProject != null) RecentApps = openedProject;
            return RecentApps;
        }

        public async static Task<ObservableCollection<Entry>> OpenPinned()
        {
            var openedProject = await OpenProject(PINNED);
            if (openedProject != null) PinnedApps = openedProject;
            return PinnedApps;
        }

        internal static void AddRecentApp(Entry entry)
        {
            if (RecentApps.Count > 5) RecentApps.RemoveAt(RecentApps.Count - 1);
            RecentApps.Insert(0, entry);
            SaveRecent();
        }

        internal static void AddPinnedApp(Entry entry)
        {
            PinnedApps.Insert(0, entry);
            SavePinned();
        }

        internal static void RemoveRecentApp(Entry entry)
        {
            RecentApps.Remove(entry);
            SaveRecent();
        }

        internal static void RemovePinnedApp(Entry entry)
        {
            PinnedApps.Remove(entry);
            SavePinned();
        }

        private static async Task<ObservableCollection<Entry>> OpenProject(string name)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var folder = await FolderExist(Windows.Storage.ApplicationData.Current.LocalFolder, PROJECT_FOLDER, true);
                    if (folder != null)
                    {

                        var storageFile = await FileExist(folder, name, false);
                        if (storageFile != null)
                        {
                            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
                                using (var outStream = stream.AsStreamForWrite())
                                {
                                    return (ObservableCollection<Entry>)serializer.Deserialize(outStream);
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                    return null;
                }

                return null;
            });
        }

        private static async Task SaveProject(string name, ObservableCollection<Entry> entries, CreationCollisionOption creationCollisionOption)
        {
            await Task.Run(async () =>
           {
               var folder = await FolderExist(Windows.Storage.ApplicationData.Current.LocalFolder, PROJECT_FOLDER, true);
               if (folder != null)
               {

                   var file = await folder.CreateFileAsync(name, creationCollisionOption);

                   using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                   {
                       XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Entry>));
                       using (var outStream = stream.AsStreamForWrite())
                       {
                           serializer.Serialize(outStream, entries);
                       }
                   }
               }

           });
        }

        private static async Task<StorageFolder> FolderExist(StorageFolder rootFolder, string folderName, bool isCreate = false)
        {
            try
            {
                return await rootFolder.GetFolderAsync(folderName);
            }
            catch (Exception exc)
            {
            }

            if (isCreate)
            {
                return await rootFolder.CreateFolderAsync(folderName);
            }
            return null;

        }


        private static async Task<StorageFile> FileExist(StorageFolder rootFolder, string filename, bool isCreate = false)
        {
            try
            {
                return await rootFolder.GetFileAsync(filename);
            }
            catch (Exception exc)
            {
            }

            if (isCreate)
            {
                return await rootFolder.CreateFileAsync(filename);
            }
            return null;

        }


    }
}
