using BilibiliDM_PluginFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SendCommand
{
    internal partial class SendCommandWindow : Window, INotifyPropertyChanged
    {
        private Main m;
        private List<DanmakuModel> list = new List<DanmakuModel>();

        public int DanmakuCount => list.Count;

        public string Output
        {
            get { return output; }
            set
            {
                if (value == output) return;
                output = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Output)));
            }
        }
        private string output = "设置好条件后，启用插件记录弹幕到抽奖池。不启用插件不会记录弹幕";

        internal SendCommandWindow(Main main)
        {
            this.m = main;
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        internal void FocusTopMost()
        {
            Topmost = true;
            Topmost = false;
        }

        internal void AddDM(DanmakuModel danmaku)
        {
            if (IsCheckXZ)
            {
                JObject j = JObject.Parse(danmaku.RawData);
                if (((JArray)j["info"][3]).Count < 2 || j["info"][3][1].ToString() != getXZName)
                { return; }// 不添加到抽奖列表
            }
            lock (list)
            {
                // 如果列表中超量就循环删除
                while (list.Count >= getListMaxNum)
                { list.RemoveAt(0); }

                list.Add(danmaku);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DanmakuCount)));
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lock (list)
            {
                int selectNum = GetSelectNum;
                if (list.Count < selectNum)
                {
                    Output = $"弹幕数量不够，当前只有{list.Count}条弹幕";
                    return;
                }

                Random r = new Random();
                bool vipadmin = (bool)DisplayVipAdmin.IsChecked;
                bool ct = (bool)DisplayCommentText.IsChecked;

                string str = "----====抽奖结果====----";

                for (int i = 0; i < selectNum; i++)
                {
                    DanmakuModel d = list[r.Next(list.Count)];
                    list.Remove(d);

                    if (i > 0)
                    { str += $"{Environment.NewLine}-----============-----"; }

                    str += $"{Environment.NewLine}第{i + 1}名：『{d.UserName}』";

                    if (vipadmin)
                    { str += (d.isVIP ? "，老爷" : "") + (d.isAdmin ? "，房管" : ""); }

                    if (ct)
                    { str += $"{Environment.NewLine}弹幕内容：{d.CommentText}"; }
                }

                str += $"{Environment.NewLine}-----============-----";
                Output = str;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DanmakuCount)));
        }

        internal void ClearList()
        {
            list.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DanmakuCount)));
        }

        private bool IsCheckXZ = false;
        private string getXZName = "";
        private int getListMaxNum = 30;

        public event PropertyChangedEventHandler PropertyChanged;

        private int GetSelectNum { get { return int.Parse(SelectNumComboBox.SelectionBoxItem.ToString()); } }

        private void XZCheck_Click(object sender, RoutedEventArgs e)
        { Shit(() => { IsCheckXZ = (bool)(sender as CheckBox).IsChecked; }); }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        { Shit(() => { getXZName = (sender as TextBox).Text; }); }

        private void ListMaxCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { Shit(() => { int.TryParse((sender as ComboBox).SelectionBoxItem.ToString(), out getListMaxNum); }); }

        private void Shit(Action a)
        { new Thread(() => { Thread.Sleep(5); Dispatcher.Invoke(a); }).Start(); }
    }
}
