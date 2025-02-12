using System.IO.Compression;

namespace EnchCoreApi.Common.IO
{
    public static class ZipHelper
    {
        /// <summary>
        /// 对目录下所有次级文件压缩为一个zip，并保存在给给定路径下。
        /// </summary>
        /// <param name="folderPath">被创建zip的目录</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="onlySubFile">指示是否仅压缩路径下的次级文件</param>
        public static void CreateFolderZip(string folderPath, string savePath, bool onlySubFile) {
            if (folderPath.EndsWith("/") || folderPath.EndsWith("\\")) {
                folderPath.Substring(0, folderPath.Length - 1);
            }
            Directory.CreateDirectory(folderPath);
            var folder = new DirectoryInfo(folderPath);
            using var fs = File.Create(Path.Combine(savePath, folder.Name + ".zip"));
            CreateFolderZip(folder, fs, onlySubFile);
        }
        /// <summary>
        /// 对目录下所有文件压缩为一个zip，并保存在流中。
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="destination">目标流</param>
        /// <param name="onlySubFile">指示是否仅压缩路径下的次级文件</param>
        public static void CreateFolderZip(DirectoryInfo folder, Stream destination, bool onlySubFile) {
            if (onlySubFile) {
                CreateFolderZip(folder.GetFiles(), destination);
            }
            else {
                using var archive = new ZipArchive(destination, ZipArchiveMode.Update);
                Recursion(folder, "", archive, null);
            }
        }
        /// <summary>
        /// 对目录下所有文件压缩为一个zip，并保存在流中。
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="destination">目标流</param>
        /// <param name="onlySubFile">指示是否仅压缩路径下的次级文件</param>
        public static void CreateFolderZip(DirectoryInfo folder, Stream destination, DirectoryInfo[]? ignoreFolders = null) {
            using var archive = new ZipArchive(destination, ZipArchiveMode.Update);
            Recursion(folder, "", archive, ignoreFolders);
        }
        private static void Recursion(DirectoryInfo folder, string folderpath, ZipArchive destination, DirectoryInfo[]? ignoreFolders = null) {
            foreach (var f in folder.GetDirectories()) {
                if (ignoreFolders is not null && ignoreFolders.Any(i => i.FullName == f.FullName)) {
                    continue;
                }
                Recursion(f, Path.Combine(folderpath, f.Name), destination, ignoreFolders);
            }
            foreach (var f in folder.GetFiles()) {
                var readmeEntry = destination.CreateEntry(Path.Combine(folderpath, f.Name));
                using var stream = readmeEntry.Open();
                using var ffs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ffs.CopyTo(stream);
            }
        }
        /// <summary>
        /// 将所有文件压缩成给定名字的zip文件，并保存在给给定路径下。
        /// </summary>
        /// <param name="zipName">zip文件名,需后缀</param>
        /// <param name="fileNames">文件</param>
        /// <param name="savePath">保存路径</param>
        public static void CreateFolderZip(string zipName, IEnumerable<string> fileNames, string savePath) {
            CreateFolderZip(zipName, from name in fileNames where File.Exists(name) select new FileInfo(name), savePath);
        }
        /// <summary>
        /// 将所有文件压缩成给定名字的zip文件，并保存在给给定路径下。
        /// </summary>
        /// <param name="zipName">zip文件名(无需后缀)</param>
        /// <param name="files">文件</param>
        /// <param name="savePath">保存路径</param>
        public static void CreateFolderZip(string zipName, IEnumerable<FileInfo> files, string savePath) {
            using FileStream fs = new FileStream(Path.Combine(savePath, zipName), FileMode.Create);
            CreateFolderZip(files, fs);
        }
        /// <summary>
        /// 将所有文件压缩，保存在流中
        /// </summary>
        /// <param name="files"></param>
        /// <param name="destination"></param>
        public static void CreateFolderZip(IEnumerable<FileInfo> files, Stream destination) {
            using ZipArchive archive = new ZipArchive(destination, ZipArchiveMode.Update);
            foreach (var file in files) {
                var readmeEntry = archive.CreateEntry(file.Name);
                using var stream = readmeEntry.Open();
                using var ffs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ffs.CopyTo(stream);
            }
        }
        /// <summary>
        /// 获取zip文件中所有的文件名
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <returns></returns>
        public static string[] ReadFileNames(string zipFileName) {
            using var fs = new FileStream(zipFileName, FileMode.Open);
            return ReadFileNames(fs);
        }
        /// <summary>
        /// 获取zip流中所有的文件名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string[] ReadFileNames(Stream source) {
            using ZipArchive archive = new ZipArchive(source, ZipArchiveMode.Read);
            return (from e in archive.Entries select e.Name).ToArray();
        }
        /// <summary>
        /// 读取zip文件中所有的文件，并保存在指定路径，并且可以指定是否先于该路径创建与压缩包同名文件夹。
        /// </summary>
        /// <param name="zipFileName">zip文件位置</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="createFolder">指定是否于该路径创建与压缩包同名文件夹</param>
        /// <returns></returns>
        public static bool ReadFolderZip(string zipFileName, string savePath, bool createFolder) {
            var zipFileInfo = new FileInfo(zipFileName);
            using var fs = zipFileInfo.OpenRead();
            if (createFolder) {
                var name = zipFileInfo.Name;
                if (name.EndsWith(".zip")) {
                    name = name.Substring(0, name.Length - 4);
                }
                return ReadFolderZip(name, fs, savePath);
            }
            return ReadFolderZip(fs, savePath);
        }
        /// <summary>
        /// 读取zip流中所有的文件，并在指定路径创建文件夹，将所有文件保存在文件夹内
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="source"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool ReadFolderZip(string folderName, Stream source, string savePath) {
            var path = Path.Combine(savePath, folderName);
            return ReadFolderZip(source, path);
        }
        /// <summary>
        /// 读取zip流中所有的文件，并将所有文件保存在指定路径
        /// </summary>
        /// <param name="source"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool ReadFolderZip(Stream source, string savePath) {
            try {
                using ZipArchive archive = new ZipArchive(source, ZipArchiveMode.Read);
                foreach (var name in archive.Entries.Select(e => e.FullName)) {
                    var entry = archive.GetEntry(name) ?? throw new Exception("Unexpected entry");
                    using var sfs = entry.Open();
                    var path = Path.Combine(savePath, Path.Combine(name.Split('/')));
                    if (name.EndsWith('/')) {
                        Directory.CreateDirectory(path);
                    }
                    else {
                        var fileInfo = new FileInfo(path);
                        if (fileInfo.Exists) {
                            continue;
                        }
                        if (fileInfo.DirectoryName != null && !Directory.Exists(fileInfo.DirectoryName)) {
                            Directory.CreateDirectory(fileInfo.DirectoryName);
                        }
                        using var destination = File.Open(path, FileMode.OpenOrCreate);
                        sfs.CopyTo(destination);
                    }
                }
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        /// <summary>
        /// 将压缩文件中所有文件或文件夹全部解压至目标路径。
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="folderPath"></param>
        public static void ExtractToDirectory(string zipFileName, string folderPath) {
            using var fs = new FileStream(zipFileName, FileMode.Open);
            ExtractToDirectory(zipFileName, folderPath);
        }
        /// <summary>
        /// 将压缩流中所有文件或文件夹全部解压至目标路径。
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="folderPath"></param>
        public static void ExtractToDirectory(Stream zipStream, string folderPath) {
            using ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
            archive.ExtractToDirectory(folderPath, true);
        }

        /// <summary>
        /// 向zip文件中添加新文件，如果存在则替换。
        /// </summary>
        /// <param name="zipFileName">包含路径的zip文件名</param>
        /// <param name="fileName">包含路径的文件名</param>
        /// <returns></returns>
        public static bool AppendOrUpdate(string zipFileName, string fileName) {
            var fileInfo = new FileInfo(fileName);
            using var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return AppendOrUpdate(zipFileName, fileInfo.Name, fs);
        }
        /// <summary>
        /// 向zip文件中创建新文件，并写入数据，如果文件名重复则覆写。
        /// </summary>
        /// <param name="zipFileName">包含路径的zip文件名</param>
        /// <param name="fileName">要创建的文件名</param>
        /// <param name="source">数据流</param>
        /// <returns></returns>
        public static bool AppendOrUpdate(string zipFileName, string fileName, Stream source) {
            using var fs = File.OpenWrite(zipFileName);
            return AppendOrUpdate(fs, fileName, source);
        }
        /// <summary>
        /// 向zip流中创建新文件，并写入数据，如果文件名重复则覆写。
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AppendOrUpdate(Stream zipStream, string fileName, Stream source) {
            try {
                using ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update);
                archive.GetEntry(fileName)?.Delete();
                var entry = archive.CreateEntry(fileName);
                using var efs = entry.Open();
                source.CopyTo(efs);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        /// <summary>
        /// 向zip文件中添加新文件，如果已经存在则不操作。
        /// </summary>
        /// <param name="zipFileName">包含路径的zip文件名</param>
        /// <param name="fileName">包含路径的文件名</param>
        /// <returns>指示是否操作成功</returns>
        public static bool Append(string zipFileName, string fileName) {
            var fileInfo = new FileInfo(fileName);
            using var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Append(zipFileName, fileInfo.Name, fs);
        }
        /// <summary>
        /// 向zip文件中创建新文件，并写入数据，如果文件名重复则不操作。
        /// </summary>
        /// <param name="zipFileName">包含路径的zip文件名</param>
        /// <param name="fileName">要创建的文件名</param>
        /// <param name="source">数据流</param>
        /// <returns>指示是否操作成功</returns>
        public static bool Append(string zipFileName, string fileName, Stream source) {
            using var fs = File.OpenWrite(zipFileName);
            return Append(fs, fileName, source);
        }
        /// <summary>
        /// 向zip流中创建新文件，并写入数据，如果文件名重复则不操作。
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        /// <returns>指示是否操作成功</returns>
        public static bool Append(Stream zipStream, string fileName, Stream source) {
            try {
                using ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update);
                if (archive.GetEntry(fileName) is null) {
                    var entry = archive.CreateEntry(fileName);
                    using var efs = entry.Open();
                    source.CopyTo(efs);
                    return true;
                }
                return false;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
