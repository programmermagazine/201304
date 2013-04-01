using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace game_00
{
    public partial class Form1 : Form
    {
        //宣告變數(基本型別)
        //int 代表變數的類型 整數
        //x,y 變數的名稱
        private int x;
        private int y;

        private int x2;
        private int y2;

        //method,方法
        //Form1,名稱
        public Form1()
        {
            InitializeComponent();

            //等號,是設定意思,不是等於
            x = 200;//設定x的數值
            y = 100;

            x2 = 400 ;
            y2 = 400 ;

            //timer1是一個計時器(鬧鐘)物件
            //timer1.Interval 多久響
            timer1.Interval = 1000 / 30;//1/30秒
            timer1.Start();
        }

        //method,方法,功能
        //onPaint, 名稱
        private void onPaint(object sender, 
                                PaintEventArgs e)
        {
            //PaintEventArgs是一個類別 繪圖事件參數
            //e 是類別的實體(物件)

            //.取出物件的資料
            //.呼叫物件的功能

            //e.Graphics 
            //取出e這個物件的Graphcis這個附屬物件

            //Graphics是一個物件,提供繪圖功能的物件
            //其中一個功能(方法)叫做DrawEllipse

            //Pens.Black
            //Pens 先不解釋
            //Pens.Black 就是一個黑色的筆

            //畫出遊戲畫面
            e.Graphics.DrawEllipse(Pens.Red,
                            x, y, 50, 50);

            e.Graphics.DrawEllipse(Pens.Blue,
                            x2, y2, 50, 50);
        }

        //定時,呼叫onTimer
        //時間到了,就會呼叫onTimier
        private void onTimer(object sender, EventArgs e)
        {
            //讓藍色球靠近紅色球
            //tx,ty是下一步的座標
            int tx;
            int ty;

            //Math.Sqrt
            //Math 先不解釋
            //(int),(double) 先不解釋
            //Sqrt 是一個(method)功能
            //取開根號

            int dist = (int)Math.Sqrt (
                        (double)(x-x2)*(x-x2)+
                        (y-y2)*(y-y2)) ;

            if (dist != 0)
            {
                tx = x2 + (x - x2) * 5 / dist;
                ty = y2 + (y - y2) * 5 / dist;

                x2 = tx;//設定
                y2 = ty;
            }

            //方法
            Invalidate();//通知重繪畫面
        }

        //按下某個按鍵的時候
        //會呼叫這個方法
        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
            //Event事件
            //Args參數
            //KeyPressEventArgs e

            //e.KeyChar 按鍵編號

            //if (e.KeyChar == 100)
            //檢查 e.KeyChar == 100
            //== 才是"等於"

            if (e.KeyChar == 97)
                x -= 10;
            if (e.KeyChar == 119)
                y -= 10;
            if (e.KeyChar == 115)
                y += 10;
            if (e.KeyChar == 100)
                x += 10;//x的數值累加1
        }
    }
}
