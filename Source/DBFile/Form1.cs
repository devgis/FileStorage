using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileID = Guid.NewGuid().ToString();
            try
            {
                if (FileHelper.AddFile(fileID, textBox1.Text))
                {
                    MessageBox.Show("保存成功！");
                    textBox2.Text = fileID;
                }
                else
                {
                    MessageBox.Show("保存失败！");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("保存失败:"+ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileHelper.GetAndOpenFile(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileHelper.DownLoadFile(textBox2.Text, fbd.SelectedPath);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("保存文件出错：" + ex.Message);
                }
            }
        }
    }
}
