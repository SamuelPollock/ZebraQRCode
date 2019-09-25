using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZebraQRCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 打印机的指令用“^”作为开头，任何打印指令都是从^XA开始，到^XZ结束
            if (string.IsNullOrEmpty(textBox1.Text)) return;
            StringBuilder strZPL = new StringBuilder();
            strZPL.Append("^XA");  // 开始指令
            strZPL.Append("^FO108,55");  // ^FOx,y x代表横坐标，y代表纵坐标

            /*
             * ^BQa,b,c
             * a代表二维码方向，默认是N
             * b代表二维码的版本，可选值有【1,2】，1是原始版本，2是增强版本，推荐用2，因为1扫不出来。
             * c代表二维码的放大程度，可选值【1-10】
             */
            strZPL.Append("^BQN,2,10");

            /*
             * ^FDab,cd^FS
             * a：错误纠正率，【H,Q,M,L】H是超高可靠度，L是高密度，建议使用H
             * b：数据输入模式，【A,M】A是自动模式（参数c可省略），M是手动模式（需要指定参数c的字符类型）
             * c：字符模式，【N】数字，【A】字符，【B】字节，【K】Kanji（日文汉字）
             * d：二维码的内容
             */
            strZPL.Append("^FDMM,A" + textBox1.Text + "^FS");
            strZPL.Append("^XZ");  // 结束指令

            serialPort1.Write(strZPL.ToString());
        }
    }
}
