using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonSongRepriseHelper
{
    public class P2Step4OutTower
    {
        //20|2022-09-04T23:13:35.8640000+08:00|4000D877|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|91.00|84.41|0.00|0.00|bdf520c9a974df9a //A左外塔
        //20|2022-09-04T23:07:48.0550000+08:00|4000D607|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|100.00|82.00|0.00|0.00|35faa154ff1055b9 //A中外塔
        //20|2022-09-04T23:22:04.2630000+08:00|4000DA06|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|109.00|84.41|0.00|0.00|f4f81cce623b5de1 //A右外

        //20|2022-09-04T23:13:35.8640000+08:00|4000D879|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|109.00|115.59|0.00|0.00|1fab6b480a17af6b//C外左
        //20|2022-09-04T14:00:08.2630000+08:00|40002A5B|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|100.00|118.00|0.00|0.00|3f0d0a4a3597d90d//C外中
        //20|2022-09-04T23:13:35.8640000+08:00|4000D879|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|91.00|115.59|0.00|0.00|1fab6b480a17af6b//C外右


        //20|2022-09-04T14:13:20.4160000+08:00|40003246|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|115.59|91.00|0.00|0.00|a18c7421c856b53a//B外左
        //20|2022-09-04T23:13:35.8640000+08:00|4000D87B|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|118.00|100.00|0.00|0.00|0231361d21a4df78//B外中
        //20|2022-09-04T14:00:08.2630000+08:00|40002A5C|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|115.59|109.00|0.00|0.00|8478cc8cf10baa66//B外右

        //20|2022-09-04T23:07:48.0550000+08:00|4000D603|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|84.41|109.00|0.00|0.00|31c71581ee482a91 //D外左
        //20|2022-09-04T23:07:48.0550000+08:00|4000D603|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|82.00|100.00|0.00|0.00|31c71581ee482a91 //D外中
        //20|2022-09-04T23:13:35.8640000+08:00|4000D878|圣骑士埃尔姆诺斯特|737C|信仰|E0000000||11.700|84.41|91.00|0.00|0.00|38f13a89ab9c55a2 //D外右

        public bool tAOutMid;
        public bool tAOutLeft;
        public bool tAOutRight;

        public bool tCOutMid;
        public bool tCOutLeft;
        public bool tCOutRight;

        public bool tBOutMid;
        public bool tBOutLeft;
        public bool tBOutRight;

        public bool tDOutMid;
        public bool tDOutLeft;
        public bool tDOutRight;

        public P2Step4OutTower()
        {
            tAOutMid = false;
            tAOutLeft = false;
            tAOutRight = false;
            tCOutMid = false;
            tCOutLeft = false;
            tCOutRight = false;
            tBOutMid = false;
            tBOutLeft = false;
            tBOutRight = false;
            tDOutMid = false;
            tDOutLeft = false;
            tDOutRight = false;
        }

        public void PrintLog()
        {
            if (tAOutMid)
            {
                Log.Print("A外中塔存在");
            }
            if (tAOutLeft)
            {
                Log.Print("A外左塔存在");
            }
            if (tAOutRight)
            {
                Log.Print("A外右塔存在");
            }

            if (tCOutMid)
            {
                Log.Print("C外中塔存在");
            }
            if (tCOutLeft)
            {
                Log.Print("C外左塔存在");
            }
            if (tCOutRight)
            {
                Log.Print("C外右塔存在");
            }

            if (tBOutMid)
            {
                Log.Print("B外中塔存在");
            }
            if (tBOutLeft)
            {
                Log.Print("B外左塔存在");
            }
            if (tBOutRight)
            {
                Log.Print("B外右塔存在");
            }

            if (tDOutMid)
            {
                Log.Print("D外中塔存在");
            }
            if (tDOutLeft)
            {
                Log.Print("D外左塔存在");
            }
            if (tDOutRight)
            {
                Log.Print("D外右塔存在");
            }
        }

        public void SetExistOutTower(double x,double y)
        {
            if (x == 100.00 && y == 82.00)
            {
                tAOutMid = true;
            }
            if (x == 91.00 && y == 84.41)
            {
                tAOutLeft = true;
            }
            if (x == 109.00 && y == 84.41)
            {
                tAOutRight = true;
            }

            if (x == 109.00 && y == 115.59)
            {
                tCOutLeft = true;
            }
            if (x == 100.00 && y == 118.00)
            {
                tCOutMid = true;
            }
            if (x == 91.00 && y == 115.59)
            {
                tCOutRight = true;
            }

            if (x == 115.59 && y == 91.00)
            {
                tBOutLeft = true;
            }
            if (x == 118.00 && y == 100.00)
            {
                tBOutMid = true;
            }
            if (x == 115.59 && y == 109.00)
            {
                tBOutRight = true;
            }

            if (x == 84.41 && y == 109.00)
            {
                tDOutLeft = true;
            }
            if (x == 82.00 && y == 100.00)
            {
                tDOutMid = true;
            }
            if (x == 84.41 && y == 91.00)
            {
                tDOutRight = true;
            }
        }
    }
}
