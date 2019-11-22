using BilibiliDM_PluginFramework;
using SendCommand.Utils;
using System.Diagnostics;

namespace SendCommand
{
    public class Main : DMPlugin
    {
        private readonly SendCommandWindow w;
        public Main()
        {
            this.PluginName = "调戏素素";
            this.PluginAuth = "野原小牛";
            this.PluginCont = "oyyz@vip.qq.com";
            this.PluginDesc = "赠送礼物可以调戏素素使其鬼畜";
            this.PluginVer = VersionHelper.GetCurrentVersion();
            this.ReceivedDanmaku += Main_ReceivedDanmaku;
            w = new SendCommandWindow(this);
        }

        private void Main_ReceivedDanmaku(object sender, ReceivedDanmakuArgs e)
        {
            if (e.Danmaku.MsgType == MsgTypeEnum.Comment)
            {
                w.AddDM(e.Danmaku);
            }
        }

        public override void Admin()
        {
            w.Show();
            w.FocusTopMost();
        }
    }
}