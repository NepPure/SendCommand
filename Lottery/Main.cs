using BilibiliDM_PluginFramework;

namespace Lottery
{
    public class Main : DMPlugin
    {
        private LotteryWindow w;
        public Main()
        {
            this.PluginName = "随机抽奖";
            this.PluginAuth = "宅急送队长";
            this.PluginCont = "DMPLottery@genteure.com";
            this.PluginDesc = "从弹幕中随机抽奖";
            this.PluginVer = "1.1";
            this.ReceivedDanmaku += Main_ReceivedDanmaku;
            w = new LotteryWindow(this);
        }

        private void Main_ReceivedDanmaku(object sender, ReceivedDanmakuArgs e)
        {
            if (e.Danmaku.MsgType == MsgTypeEnum.Comment)
            {
                w.AddDM(e.Danmaku);
            }
        }

        public override void Admin()
        { w.Show(); w.FocusTopMost(); }
    }
}