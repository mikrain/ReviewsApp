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
        private static ObservableCollection<Entry> _recentApps = new ObservableCollection<Entry>();
        private static ObservableCollection<Entry> _pinnedApps = new ObservableCollection<Entry>();

        public static ObservableCollection<Entry> RecentApps
        {
            get { return _recentApps; }
        }

        public static ObservableCollection<Entry> PinnedApps
        {
            get { return _pinnedApps; }
        }

        private static async void SavePinned()
        {
            await SaveProject(PINNED, _pinnedApps.ToList(), CreationCollisionOption.ReplaceExisting);
        }

        private static async void SaveRecent()
        {
            await SaveProject(RECENT, _recentApps.ToList(), CreationCollisionOption.ReplaceExisting);
        }

        public async static Task<ObservableCollection<Entry>> OpenRecent()
        {
            var openedProject = await OpenProject(RECENT);
            if (openedProject != null)
            {
                _recentApps.Clear();
                foreach (var entry in openedProject)
                {
                    _recentApps.Add(entry);
                }

            }
            return _recentApps;
        }

        public async static Task<ObservableCollection<Entry>> OpenPinned()
        {
            var openedProject = await OpenProject(PINNED);
            if (openedProject != null)
            {
                _pinnedApps.Clear();
                foreach (var entry in openedProject)
                {
                    _pinnedApps.Add(entry);
                }
            }
            return _pinnedApps;
        }

        internal static void AddRecentApp(Entry entry)
        {
            if (_recentApps.FirstOrDefault(entry1 => entry1.Title == entry.Title) == null)
            {
                if (_recentApps.Count > 5) _recentApps.RemoveAt(_recentApps.Count - 1);
                _recentApps.Insert(0, entry);
                SaveRecent();
            }

        }

        internal static void AddPinnedApp(Entry entry)
        {
            _pinnedApps.Insert(0, entry);
            SavePinned();
        }

        internal static void RemoveRecentApp(Entry entry)
        {
            _recentApps.Remove(entry);
            SaveRecent();
        }

        internal static void RemovePinnedApp(Entry entry)
        {
            _pinnedApps.Remove(entry);
            SavePinned();
        }

        private static async Task<List<Entry>> OpenProject(string name)
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
                                    return (List<Entry>)serializer.Deserialize(outStream);
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

        private static async Task SaveProject(string name, List<Entry> entries, CreationCollisionOption creationCollisionOption)
        {
            await Task.Run(async () =>
           {
               var folder = await FolderExist(Windows.Storage.ApplicationData.Current.LocalFolder, PROJECT_FOLDER, true);
               if (folder != null)
               {

                   var file = await folder.CreateFileAsync(name, creationCollisionOption);

                   using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                   {
                       XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
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
