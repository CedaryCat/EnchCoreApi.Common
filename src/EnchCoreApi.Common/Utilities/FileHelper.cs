using System.Text;

namespace EnchCoreApi.Common.Utilities
{
    public static class FileHelper
    {
        public static Encoding GetFileTextEncoding(string fileName) {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Encoding r = GetFileTextEncoding(fs);
            fs.Close();
            return r;
        }

        public static string ReadFileTextInCorrectEncoding(string fileName) {
            var encoding = GetFileTextEncoding(fileName);
            return File.ReadAllText(fileName, encoding);
        }

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name=“fileTextStream“>文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetFileTextEncoding(FileStream fileTextStream) {
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fileTextStream, System.Text.Encoding.Default);

            byte[] ss = r.ReadBytes((int)fileTextStream.Length);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)) {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00) {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41) {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name=“data“></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data) {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++) {
                curByte = data[i];
                if (charByteCounter == 1) {
                    if (curByte >= 0x80) {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0) {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                        if (charByteCounter == 1 || charByteCounter > 6) {
                            return false;
                        }
                    }
                }
                else {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80) {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1) {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}
