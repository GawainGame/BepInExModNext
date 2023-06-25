using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Text;

namespace SkySwordKill.Next.Scr.NextModEditor.Mod.CommonClass
{
    public static class JsonConvertEx
    {
        public static string SerializeObject<T>(T value)
        {
            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);

            var jsonSerializer = JsonSerializer.CreateDefault();
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.IndentChar = ' ';
                jsonWriter.Indentation = 4;

                jsonSerializer.Serialize(jsonWriter, value, typeof(T));
            }
            return sw.ToString();
        }
    }
    public static class DirectoryOperate
    {
        /// <summary>
        /// 删除掉所有空文件夹
        /// 所有没有子“文件系统”的都将被删除
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteEmptyDirectory(string path)
        {
            if (!Directory.Exists(path)) { return; }
            DirectoryInfo currentDirectory = new(path);
            foreach (DirectoryInfo directory in currentDirectory.GetDirectories())
            {
                DeleteEmptyDirectory(directory.FullName);
            }
            if (currentDirectory.GetFiles().Length == 0 && currentDirectory.GetDirectories().Length == 0)
            {
                currentDirectory.Delete();
            }
        }
    }
}
