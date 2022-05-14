using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DBFile
{
    public class FileHelper
    {
        /// <summary>
        /// 添加文件到数据库
        /// </summary>
        /// <param name="FileID">文件ID</param>
        /// <param name="FileName">文件名称</param>
        /// <param name="FileContent">文件内容</param>
        /// <returns></returns>
        public static bool AddFile(string FileID, string FilePath)
        {
            if (File.Exists(FilePath))
            {
                SqlCommand cmd = GetInsertCommand(FileID, FilePath);
                return SQLHelper.Instance.ExecuteSql(cmd);
            }
            else
            {
                return false;
            }
        }

        private static SqlCommand GetInsertCommand(string FileID, string FilePath)
        {
            string FileName = Path.GetFileName(FilePath);//获取文件名
            string sql = string.Format("INSERT INTO T_FILE(FILEID,FILENAME,FILECONTENT) VALUES('{0}','{1}',@FILECONTENT)"
                , FileID, FileName);
            SqlCommand sc = new SqlCommand(sql);
            SqlParameter sqlParm = new SqlParameter("@FILECONTENT", SqlDbType.Image);

            FileInfo fi = new FileInfo(FilePath);
            long len = fi.Length;
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            byte[] buffer = new byte[len];
            fs.Read(buffer, 0, (int)len);
            fs.Close();
            sqlParm.Value = buffer;
            sc.Parameters.Add(sqlParm);
            return sc;
        }

        /// <summary>
        /// 读取并打开文件
        /// </summary>
        /// <param name="FileID">文件ID</param>
        public static void GetAndOpenFile(string FileID)
        {
            string fileName = string.Empty;
            Byte[] FileContent = GetFile(FileID, out fileName);
            if (!string.IsNullOrEmpty(fileName) && FileContent != null)
            {
                string FilePath= Application.StartupPath+ @"\temp";
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                string FileName = Path.Combine(FilePath,fileName);
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(FileContent, 0, Convert.ToInt32(FileContent.Length));
                fs.Close();

                //打开文件
                System.Diagnostics.Process.Start(FileName); //打开此文件。
            }
            else
            {
                throw new Exception("读取文件失败，可能是文件不存在！");
            }
        }

        public static void DownLoadFile(string FileID,String FilePath)
        {
            string fileName = string.Empty;
            Byte[] FileContent = GetFile(FileID, out fileName);
            if (!string.IsNullOrEmpty(fileName) && FileContent != null)
            {
                //string FilePath = Application.StartupPath + @"\temp";
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                string FileName = Path.Combine(FilePath, fileName);
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(FileContent, 0, Convert.ToInt32(FileContent.Length));
                fs.Close();

                //打开文件
                //System.Diagnostics.Process.Start(FileName); //打开此文件。
                MessageBox.Show("保存成功！");
            }
            else
            {
                throw new Exception("读取文件失败，可能是文件不存在！");
            }
        }

        /// <summary>
        /// 读取文件到字节码
        /// </summary>
        /// <param name="FileID">文件编号</param>
        /// <param name="FileName">文件名称</param>
        /// <returns>文件内容</returns>
        public static Byte[] GetFile(string FileID,out string FileName)
        {
            string sql = string.Format("SELECT FILEID,FILENAME,FILECONTENT FROM T_FILE WHERE FILEID='{0}'"
                , FileID);
            DataTable dtTemp = SQLHelper.Instance.GetDataTable(sql);
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                FileName = dtTemp.Rows[0]["FILENAME"].ToString();
                return (Byte[])dtTemp.Rows[0]["FILECONTENT"];
            }
            else
            {
                FileName = string.Empty;
                return null;
            }
        }

    }
}
