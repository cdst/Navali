using System.ComponentModel;
using Loki;
using Loki.Common;

namespace Navali
{
    public class NavaliSettings : JsonSettings
    {

        private static NavaliSettings _instance;
        public static NavaliSettings Instance
        {
            get
            {
                return _instance ?? (_instance = new NavaliSettings());
            }
        }
        public NavaliSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name, string.Format("{0}.json", "Navali")))
        {
        }

        #region General


        private string _numcoinsmin;

        private bool _m1;
        private bool _m2;

        private bool _c1;
        private bool _c2;
        private bool _c3;
        private bool _c4;
        private bool _c5;
        private bool _c6;
        private bool _c7;
        private bool _c8;
        private bool _c9;
        private bool _c10;
        private bool _c11;
        private bool _c12;

        private bool _f1;
        private bool _f2;
        private bool _f3;
        private bool _f4;
        private bool _f5;
        private bool _f6;
        private bool _f7;
        private bool _f8;
        private bool _f9;
        private bool _f10;
        private bool _f11;
        private bool _f12;
        private bool _f13;
        private bool _f14;
        private bool _f15;
        private bool _f16;
        private bool _f17;
        private bool _f18;
        private bool _f19;
        private bool _f20;
        private bool _f21;
        private bool _f22;
        private bool _f23;
        private bool _f24;
        private bool _f25;

        private bool _e0;
        private bool _e1;
        private bool _e2;
        private bool _e3;
        private bool _e4;
        private bool _e5;
        private bool _e6;
        private bool _e7;
        private bool _e8;
        private bool _e9;
        private bool _e10;
        private bool _e11;
        private bool _e12;
        private bool _e13;
        private bool _e14;
        private bool _e15;
        private bool _e16;
        private bool _e17;
        private bool _e18;
        private bool _e19;
        private bool _e20;
        private bool _e21;
        private bool _e22;
        private bool _e23;
        private bool _e24;
        private bool _e25;
        private bool _e26;
        private bool _e27;
        private bool _e28;
        private bool _e29;
        private bool _e30;
        private bool _e31;
        private bool _e32;
        private bool _e33;
        private bool _e34;
        private bool _e35;
        private bool _e36;
        private bool _e37;
        private bool _e38;
        private bool _e39;
        private bool _e40;
        private bool _e41;
        private bool _e42;
        private bool _e43;
        private bool _e44;
        private bool _e45;
        private bool _e46;
        private bool _e47;

        private bool _s0;
        private bool _s1;
        private bool _s2;
        private bool _s3;
        private bool _s4;
        private bool _s5;

        private bool _n0;
        private bool _n1;
        private bool _n2;
        private bool _n3;
        private bool _n4;
        private bool _n5;
        private bool _n6;
        private bool _n7;
        private bool _n8;
        private bool _n9;
        private bool _n10;
        private bool _n11;
        private bool _n12;
        private bool _n13;
        private bool _n14;
        private bool _n15;
        private bool _n16;
        private bool _n17;
        private bool _n18;
        private bool _n19;
        private bool _n20;
        private bool _n21;
        private bool _n22;
        private bool _n23;
        private bool _n24;
        private bool _n25;
        private bool _n26;
        private bool _n27;
        private bool _n28;
        private bool _n29;
        private bool _n30;
        private bool _n31;
        private bool _n32;
        private bool _n33;
        private bool _n34;
        private bool _n35;
        private bool _n36;
        private bool _n37;
        private bool _n38;
        private bool _n39;
        private bool _n40;
        private bool _n41;
        private bool _n42;

        private bool _o0;
        private bool _o1;
        private bool _o2;
        private bool _o3;
        private bool _o4;
        private bool _o5;

        private bool _p0;
        private bool _p1;
        private bool _p2;
        private bool _p3;
        private bool _p4;
        private bool _p5;
        private bool _p6;
        private bool _p7;
        private bool _p8;
        private bool _p9;
        private bool _p10;
        private bool _p11;
        private bool _p12;
        private bool _p13;
        private bool _p14;
        private bool _p15;
        private bool _p16;
        private bool _p17;
        private bool _p18;
        private bool _p19;
        private bool _p20;
        private bool _p21;
        private bool _p22;
        private bool _p23;
        private bool _p24;
        private bool _p25;
        private bool _p26;
        private bool _p27;
        private bool _p28;
        private bool _p29;
        private bool _p30;
        private bool _p31;
        private bool _p32;
        private bool _p33;
        private bool _p34;
        private bool _p35;
        private bool _p36;
        private bool _p37;
        private bool _p38;
        private bool _p39;
        private bool _p40;
        private bool _p41;
        private bool _p42;
        private bool _p43;
        private bool _p44;
        private bool _p45;
        private bool _p46;
        private bool _p47;
        private bool _p48;
        private bool _p49;
        private bool _p50;
        private bool _p51;

        private bool _t0;
        private bool _t1;
        private bool _t2;
        private bool _t3;
        private bool _t4;
        private bool _t5;

        private bool _x0;
        private bool _x1;
        private bool _x2;
        private bool _x3;
        private bool _x4;
        private bool _x5;
        private bool _x6;
        private bool _x7;


        [DefaultValue("20")]
        public string NumCoinsMin
        {
            get { return _numcoinsmin; }
            set
            {
                if (value.Equals(_numcoinsmin))
                {
                    return;
                }
                _numcoinsmin = value;
                NotifyPropertyChanged(() => NumCoinsMin);
            }
        }
        [DefaultValue(false)]
        public bool m1
        {
            get { return _m1;  }
            set
            {
                if (value == _m1) return;
                _m1 = value;
                NotifyPropertyChanged(() => m1);
            }
        }
        [DefaultValue(false)]
        public bool m2
        {
            get { return _m2; }
            set { if (value == _m2) return; _m2 = value; NotifyPropertyChanged(() => m2); }
        }
        [DefaultValue(false)]
        public bool c1
        {
            get { return _c1; }
            set { if (value == _c1) return; _c1 = value; NotifyPropertyChanged(() => c1); }
        }
        [DefaultValue(false)]
        public bool c2
        {
            get { return _c2; }
            set { if (value == _c2) return; _c2 = value; NotifyPropertyChanged(() => c2); }
        }
        [DefaultValue(false)]
        public bool c3
        {
            get { return _c3; }
            set { if (value == _c3) return; _c3 = value; NotifyPropertyChanged(() => c3); }
        }
        [DefaultValue(false)]
        public bool c4
        {
            get { return _c4; }
            set { if (value == _c4) return; _c4 = value; NotifyPropertyChanged(() => c4); }
        }
        [DefaultValue(false)]
        public bool c5
        {
            get { return _c5; }
            set { if (value == _c5) return; _c5 = value; NotifyPropertyChanged(() => c5); }
        }
        [DefaultValue(false)]
        public bool c6
        {
            get { return _c6; }
            set { if (value == _c6) return; _c6 = value; NotifyPropertyChanged(() => c6); }
        }
        [DefaultValue(false)]
        public bool c7
        {
            get { return _c7; }
            set { if (value == _c7) return; _c7 = value; NotifyPropertyChanged(() => c7); }
        }
        [DefaultValue(false)]
        public bool c8
        {
            get { return _c8; }
            set { if (value == _c8) return; _c8 = value; NotifyPropertyChanged(() => c8); }
        }
        [DefaultValue(false)]
        public bool c9
        {
            get { return _c9; }
            set { if (value == _c9) return; _c9 = value; NotifyPropertyChanged(() => c9); }
        }
        [DefaultValue(false)]
        public bool c10
        {
            get { return _c10; }
            set { if (value == _c10) return; _c10 = value; NotifyPropertyChanged(() => c10); }
        }
        [DefaultValue(false)]
        public bool c11
        {
            get { return _c11; }
            set { if (value == _c11) return; _c11 = value; NotifyPropertyChanged(() => c11); }
        }
        [DefaultValue(false)]
        public bool c12
        {
            get { return _c12; }
            set { if (value == _c12) return; _c12 = value; NotifyPropertyChanged(() => c12); }
        }
        [DefaultValue(false)]
        public bool f1
        {
            get { return _f1; }
            set { if (value == _f1) return; _f1 = value; NotifyPropertyChanged(() => f1); }
        }
        [DefaultValue(false)]
        public bool f2
        {
            get { return _f2; }
            set { if (value == _f2) return; _f2 = value; NotifyPropertyChanged(() => f2); }
        }
        [DefaultValue(false)]
        public bool f3
        {
            get { return _f3; }
            set { if (value == _f3) return; _f3 = value; NotifyPropertyChanged(() => f3); }
        }
        [DefaultValue(false)]
        public bool f4
        {
            get { return _f4; }
            set { if (value == _f4) return; _f4 = value; NotifyPropertyChanged(() => f4); }
        }
        [DefaultValue(false)]
        public bool f5
        {
            get { return _f5; }
            set { if (value == _f5) return; _f5 = value; NotifyPropertyChanged(() => f5); }
        }

        [DefaultValue(false)]
        public bool f6
        {
            get { return _f6; }
            set { if (value == _f6) return; _f6 = value; NotifyPropertyChanged(() => f6); }
        }

        [DefaultValue(false)]
        public bool f7
        {
            get { return _f7; }
            set { if (value == _f7) return; _f7 = value; NotifyPropertyChanged(() => f7); }
        }

        [DefaultValue(false)]
        public bool f8
        {
            get { return _f8; }
            set { if (value == _f8) return; _f8 = value; NotifyPropertyChanged(() => f8); }
        }

        [DefaultValue(false)]
        public bool f9
        {
            get { return _f9; }
            set { if (value == _f9) return; _f9 = value; NotifyPropertyChanged(() => f9); }
        }

        [DefaultValue(false)]
        public bool f10
        {
            get { return _f10; }
            set { if (value == _f10) return; _f10 = value; NotifyPropertyChanged(() => f10); }
        }

        [DefaultValue(false)]
        public bool f11
        {
            get { return _f11; }
            set { if (value == _f11) return; _f11 = value; NotifyPropertyChanged(() => f11); }
        }

        [DefaultValue(false)]
        public bool f12
        {
            get { return _f12; }
            set { if (value == _f12) return; _f12 = value; NotifyPropertyChanged(() => f12); }
        }

        [DefaultValue(false)]
        public bool f13
        {
            get { return _f13; }
            set { if (value == _f13) return; _f13 = value; NotifyPropertyChanged(() => f13); }
        }

        [DefaultValue(false)]
        public bool f14
        {
            get { return _f14; }
            set { if (value == _f14) return; _f14 = value; NotifyPropertyChanged(() => f14); }
        }

        [DefaultValue(false)]
        public bool f15
        {
            get { return _f15; }
            set { if (value == _f15) return; _f15 = value; NotifyPropertyChanged(() => f15); }
        }

        [DefaultValue(false)]
        public bool f16
        {
            get { return _f16; }
            set { if (value == _f16) return; _f16 = value; NotifyPropertyChanged(() => f16); }
        }

        [DefaultValue(false)]
        public bool f17
        {
            get { return _f17; }
            set { if (value == _f17) return; _f17 = value; NotifyPropertyChanged(() => f17); }
        }

        [DefaultValue(false)]
        public bool f18
        {
            get { return _f18; }
            set { if (value == _f18) return; _f18 = value; NotifyPropertyChanged(() => f18); }
        }

        [DefaultValue(false)]
        public bool f19
        {
            get { return _f19; }
            set { if (value == _f19) return; _f19 = value; NotifyPropertyChanged(() => f19); }
        }

        [DefaultValue(false)]
        public bool f20
        {
            get { return _f20; }
            set { if (value == _f20) return; _f20 = value; NotifyPropertyChanged(() => f20); }
        }

        [DefaultValue(false)]
        public bool f21
        {
            get { return _f21; }
            set { if (value == _f21) return; _f21 = value; NotifyPropertyChanged(() => f21); }
        }

        [DefaultValue(false)]
        public bool f22
        {
            get { return _f22; }
            set { if (value == _f22) return; _f22 = value; NotifyPropertyChanged(() => f22); }
        }

        [DefaultValue(false)]
        public bool f23
        {
            get { return _f23; }
            set { if (value == _f23) return; _f23 = value; NotifyPropertyChanged(() => f23); }
        }

        [DefaultValue(false)]
        public bool f24
        {
            get { return _f24; }
            set { if (value == _f24) return; _f24 = value; NotifyPropertyChanged(() => f24); }
        }

        [DefaultValue(false)]
        public bool f25
        {
            get { return _f25; }
            set { if (value == _f25) return; _f25 = value; NotifyPropertyChanged(() => f25); }
        }
        [DefaultValue(false)]
        public bool e0
        {
            get { return _e0; }
            set { if (value == _e0) return; _e0 = value; NotifyPropertyChanged(() => e0); }
        }
        [DefaultValue(false)]
        public bool e1
        {
            get { return _e1; }
            set { if (value == _e1) return; _e1 = value; NotifyPropertyChanged(() => e1); }
        }
        [DefaultValue(false)]
        public bool e2
        {
            get { return _e2; }
            set { if (value == _e2) return; _e2 = value; NotifyPropertyChanged(() => e2); }
        }
        [DefaultValue(false)]
        public bool e3
        {
            get { return _e3; }
            set { if (value == _e3) return; _e3 = value; NotifyPropertyChanged(() => e3); }
        }
        [DefaultValue(false)]
        public bool e4
        {
            get { return _e4; }
            set { if (value == _e4) return; _e4 = value; NotifyPropertyChanged(() => e4); }
        }
        [DefaultValue(false)]
        public bool e5
        {
            get { return _e5; }
            set { if (value == _e5) return; _e5 = value; NotifyPropertyChanged(() => e5); }
        }
        [DefaultValue(false)]
        public bool e6
        {
            get { return _e6; }
            set { if (value == _e6) return; _e6 = value; NotifyPropertyChanged(() => e6); }
        }
        [DefaultValue(false)]
        public bool e7
        {
            get { return _e7; }
            set { if (value == _e7) return; _e7 = value; NotifyPropertyChanged(() => e7); }
        }
        [DefaultValue(false)]
        public bool e8
        {
            get { return _e8; }
            set { if (value == _e8) return; _e8 = value; NotifyPropertyChanged(() => e8); }
        }
        [DefaultValue(false)]
        public bool e9
        {
            get { return _e9; }
            set { if (value == _e9) return; _e9 = value; NotifyPropertyChanged(() => e9); }
        }
        [DefaultValue(false)]
        public bool e10
        {
            get { return _e10; }
            set { if (value == _e10) return; _e10 = value; NotifyPropertyChanged(() => e10); }
        }
        [DefaultValue(false)]
        public bool e11
        {
            get { return _e11; }
            set { if (value == _e11) return; _e11 = value; NotifyPropertyChanged(() => e11); }
        }
        [DefaultValue(false)]
        public bool e12
        {
            get { return _e12; }
            set { if (value == _e12) return; _e12 = value; NotifyPropertyChanged(() => e12); }
        }
        [DefaultValue(false)]
        public bool e13
        {
            get { return _e13; }
            set { if (value == _e13) return; _e13 = value; NotifyPropertyChanged(() => e13); }
        }
        [DefaultValue(false)]
        public bool e14
        {
            get { return _e14; }
            set { if (value == _e14) return; _e14 = value; NotifyPropertyChanged(() => e14); }
        }
        [DefaultValue(false)]
        public bool e15
        {
            get { return _e15; }
            set { if (value == _e15) return; _e15 = value; NotifyPropertyChanged(() => e15); }
        }
        [DefaultValue(false)]
        public bool e16
        {
            get { return _e16; }
            set { if (value == _e16) return; _e16 = value; NotifyPropertyChanged(() => e16); }
        }
        [DefaultValue(false)]
        public bool e17
        {
            get { return _e17; }
            set { if (value == _e17) return; _e17 = value; NotifyPropertyChanged(() => e17); }
        }
        [DefaultValue(false)]
        public bool e18
        {
            get { return _e18; }
            set { if (value == _e18) return; _e18 = value; NotifyPropertyChanged(() => e18); }
        }
        [DefaultValue(false)]
        public bool e19
        {
            get { return _e19; }
            set { if (value == _e19) return; _e19 = value; NotifyPropertyChanged(() => e19); }
        }
        [DefaultValue(false)]
        public bool e20
        {
            get { return _e20; }
            set { if (value == _e20) return; _e20 = value; NotifyPropertyChanged(() => e20); }
        }
        [DefaultValue(false)]
        public bool e21
        {
            get { return _e21; }
            set { if (value == _e21) return; _e21 = value; NotifyPropertyChanged(() => e21); }
        }
        [DefaultValue(false)]
        public bool e22
        {
            get { return _e22; }
            set { if (value == _e22) return; _e22 = value; NotifyPropertyChanged(() => e22); }
        }
        [DefaultValue(false)]
        public bool e23
        {
            get { return _e23; }
            set { if (value == _e23) return; _e23 = value; NotifyPropertyChanged(() => e23); }
        }
        [DefaultValue(false)]
        public bool e24
        {
            get { return _e24; }
            set { if (value == _e24) return; _e24 = value; NotifyPropertyChanged(() => e24); }
        }
        [DefaultValue(false)]
        public bool e25
        {
            get { return _e25; }
            set { if (value == _e25) return; _e25 = value; NotifyPropertyChanged(() => e25); }
        }
        [DefaultValue(false)]
        public bool e26
        {
            get { return _e26; }
            set { if (value == _e26) return; _e26 = value; NotifyPropertyChanged(() => e26); }
        }
        [DefaultValue(false)]
        public bool e27
        {
            get { return _e27; }
            set { if (value == _e27) return; _e27 = value; NotifyPropertyChanged(() => e27); }
        }
        [DefaultValue(false)]
        public bool e28
        {
            get { return _e28; }
            set { if (value == _e28) return; _e28 = value; NotifyPropertyChanged(() => e28); }
        }
        [DefaultValue(false)]
        public bool e29
        {
            get { return _e29; }
            set { if (value == _e29) return; _e29 = value; NotifyPropertyChanged(() => e29); }
        }
        [DefaultValue(false)]
        public bool e30
        {
            get { return _e30; }
            set { if (value == _e30) return; _e30 = value; NotifyPropertyChanged(() => e30); }
        }
        [DefaultValue(false)]
        public bool e31
        {
            get { return _e31; }
            set { if (value == _e31) return; _e31 = value; NotifyPropertyChanged(() => e31); }
        }
        [DefaultValue(false)]
        public bool e32
        {
            get { return _e32; }
            set { if (value == _e32) return; _e32 = value; NotifyPropertyChanged(() => e32); }
        }
        [DefaultValue(false)]
        public bool e33
        {
            get { return _e33; }
            set { if (value == _e33) return; _e33 = value; NotifyPropertyChanged(() => e33); }
        }
        [DefaultValue(false)]
        public bool e34
        {
            get { return _e34; }
            set { if (value == _e34) return; _e34 = value; NotifyPropertyChanged(() => e34); }
        }
        [DefaultValue(false)]
        public bool e35
        {
            get { return _e35; }
            set { if (value == _e35) return; _e35 = value; NotifyPropertyChanged(() => e35); }
        }
        [DefaultValue(false)]
        public bool e36
        {
            get { return _e36; }
            set { if (value == _e36) return; _e36 = value; NotifyPropertyChanged(() => e36); }
        }
        [DefaultValue(false)]
        public bool e37
        {
            get { return _e37; }
            set { if (value == _e37) return; _e37 = value; NotifyPropertyChanged(() => e37); }
        }
        [DefaultValue(false)]
        public bool e38
        {
            get { return _e38; }
            set { if (value == _e38) return; _e38 = value; NotifyPropertyChanged(() => e38); }
        }
        [DefaultValue(false)]
        public bool e39
        {
            get { return _e39; }
            set { if (value == _e39) return; _e39 = value; NotifyPropertyChanged(() => e39); }
        }
        [DefaultValue(false)]
        public bool e40
        {
            get { return _e40; }
            set { if (value == _e40) return; _e40 = value; NotifyPropertyChanged(() => e40); }
        }
        [DefaultValue(false)]
        public bool e41
        {
            get { return _e41; }
            set { if (value == _e41) return; _e41 = value; NotifyPropertyChanged(() => e41); }
        }
        [DefaultValue(false)]
        public bool e42
        {
            get { return _e42; }
            set { if (value == _e42) return; _e42 = value; NotifyPropertyChanged(() => e42); }
        }
        [DefaultValue(false)]
        public bool e43
        {
            get { return _e43; }
            set { if (value == _e43) return; _e43 = value; NotifyPropertyChanged(() => e43); }
        }
        [DefaultValue(false)]
        public bool e44
        {
            get { return _e44; }
            set { if (value == _e44) return; _e44 = value; NotifyPropertyChanged(() => e44); }
        }
        [DefaultValue(false)]
        public bool e45
        {
            get { return _e45; }
            set { if (value == _e45) return; _e45 = value; NotifyPropertyChanged(() => e45); }
        }
        [DefaultValue(false)]
        public bool e46
        {
            get { return _e46; }
            set { if (value == _e46) return; _e46 = value; NotifyPropertyChanged(() => e46); }
        }
        [DefaultValue(false)]
        public bool e47
        {
            get { return _e47; }
            set { if (value == _e47) return; _e47 = value; NotifyPropertyChanged(() => e47); }
        }
        [DefaultValue(false)]
        public bool s0
        {
            get { return _s0; }
            set { if (value == _s0) return; _s0 = value; NotifyPropertyChanged(() => s0); }
        }
        [DefaultValue(false)]
        public bool s1
        {
            get { return _s1; }
            set { if (value == _s1) return; _s1 = value; NotifyPropertyChanged(() => s1); }
        }
        [DefaultValue(false)]
        public bool s2
        {
            get { return _s2; }
            set { if (value == _s2) return; _s2 = value; NotifyPropertyChanged(() => s2); }
        }
        [DefaultValue(false)]
        public bool s3
        {
            get { return _s3; }
            set { if (value == _s3) return; _s3 = value; NotifyPropertyChanged(() => s3); }
        }
        [DefaultValue(false)]
        public bool s4
        {
            get { return _s4; }
            set { if (value == _s4) return; _s4 = value; NotifyPropertyChanged(() => s4); }
        }
        [DefaultValue(false)]
        public bool s5
        {
            get { return _s5; }
            set { if (value == _s5) return; _s5 = value; NotifyPropertyChanged(() => s5); }
        }
        [DefaultValue(false)]
        public bool n0
        {
            get { return _n0; }
            set { if (value == _n0) return; _n0 = value; NotifyPropertyChanged(() => n0); }
        }
        [DefaultValue(false)]
        public bool n1
        {
            get { return _n1; }
            set { if (value == _n1) return; _n1 = value; NotifyPropertyChanged(() => n1); }
        }
        [DefaultValue(false)]
        public bool n2
        {
            get { return _n2; }
            set { if (value == _n2) return; _n2 = value; NotifyPropertyChanged(() => n2); }
        }
        [DefaultValue(false)]
        public bool n3
        {
            get { return _n3; }
            set { if (value == _n3) return; _n3 = value; NotifyPropertyChanged(() => n3); }
        }
        [DefaultValue(false)]
        public bool n4
        {
            get { return _n4; }
            set { if (value == _n4) return; _n4 = value; NotifyPropertyChanged(() => n4); }
        }
        [DefaultValue(false)]
        public bool n5
        {
            get { return _n5; }
            set { if (value == _n5) return; _n5 = value; NotifyPropertyChanged(() => n5); }
        }
        [DefaultValue(false)]
        public bool n6
        {
            get { return _n6; }
            set { if (value == _n6) return; _n6 = value; NotifyPropertyChanged(() => n6); }
        }
        [DefaultValue(false)]
        public bool n7
        {
            get { return _n7; }
            set { if (value == _n7) return; _n7 = value; NotifyPropertyChanged(() => n7); }
        }
        [DefaultValue(false)]
        public bool n8
        {
            get { return _n8; }
            set { if (value == _n8) return; _n8 = value; NotifyPropertyChanged(() => n8); }
        }
        [DefaultValue(false)]
        public bool n9
        {
            get { return _n9; }
            set { if (value == _n9) return; _n9 = value; NotifyPropertyChanged(() => n9); }
        }
        [DefaultValue(false)]
        public bool n10
        {
            get { return _n10; }
            set { if (value == _n10) return; _n10 = value; NotifyPropertyChanged(() => n10); }
        }
        [DefaultValue(false)]
        public bool n11
        {
            get { return _n11; }
            set { if (value == _n11) return; _n11 = value; NotifyPropertyChanged(() => n11); }
        }
        [DefaultValue(false)]
        public bool n12
        {
            get { return _n12; }
            set { if (value == _n12) return; _n12 = value; NotifyPropertyChanged(() => n12); }
        }
        [DefaultValue(false)]
        public bool n13
        {
            get { return _n13; }
            set { if (value == _n13) return; _n13 = value; NotifyPropertyChanged(() => n13); }
        }
        [DefaultValue(false)]
        public bool n14
        {
            get { return _n14; }
            set { if (value == _n14) return; _n14 = value; NotifyPropertyChanged(() => n14); }
        }
        [DefaultValue(false)]
        public bool n15
        {
            get { return _n15; }
            set { if (value == _n15) return; _n15 = value; NotifyPropertyChanged(() => n15); }
        }
        [DefaultValue(false)]
        public bool n16
        {
            get { return _n16; }
            set { if (value == _n16) return; _n16 = value; NotifyPropertyChanged(() => n16); }
        }
        [DefaultValue(false)]
        public bool n17
        {
            get { return _n17; }
            set { if (value == _n17) return; _n17 = value; NotifyPropertyChanged(() => n17); }
        }
        [DefaultValue(false)]
        public bool n18
        {
            get { return _n18; }
            set { if (value == _n18) return; _n18 = value; NotifyPropertyChanged(() => n18); }
        }
        [DefaultValue(false)]
        public bool n19
        {
            get { return _n19; }
            set { if (value == _n19) return; _n19 = value; NotifyPropertyChanged(() => n19); }
        }
        [DefaultValue(false)]
        public bool n20
        {
            get { return _n20; }
            set { if (value == _n20) return; _n20 = value; NotifyPropertyChanged(() => n20); }
        }
        [DefaultValue(false)]
        public bool n21
        {
            get { return _n21; }
            set { if (value == _n21) return; _n21 = value; NotifyPropertyChanged(() => n21); }
        }
        [DefaultValue(false)]
        public bool n22
        {
            get { return _n22; }
            set { if (value == _n22) return; _n22 = value; NotifyPropertyChanged(() => n22); }
        }
        [DefaultValue(false)]
        public bool n23
        {
            get { return _n23; }
            set { if (value == _n23) return; _n23 = value; NotifyPropertyChanged(() => n23); }
        }
        [DefaultValue(false)]
        public bool n24
        {
            get { return _n24; }
            set { if (value == _n24) return; _n24 = value; NotifyPropertyChanged(() => n24); }
        }
        [DefaultValue(false)]
        public bool n25
        {
            get { return _n25; }
            set { if (value == _n25) return; _n25 = value; NotifyPropertyChanged(() => n25); }
        }
        [DefaultValue(false)]
        public bool n26
        {
            get { return _n26; }
            set { if (value == _n26) return; _n26 = value; NotifyPropertyChanged(() => n26); }
        }
        [DefaultValue(false)]
        public bool n27
        {
            get { return _n27; }
            set { if (value == _n27) return; _n27 = value; NotifyPropertyChanged(() => n27); }
        }
        [DefaultValue(false)]
        public bool n28
        {
            get { return _n28; }
            set { if (value == _n28) return; _n28 = value; NotifyPropertyChanged(() => n28); }
        }
        [DefaultValue(false)]
        public bool n29
        {
            get { return _n29; }
            set { if (value == _n29) return; _n29 = value; NotifyPropertyChanged(() => n29); }
        }
        [DefaultValue(false)]
        public bool n30
        {
            get { return _n30; }
            set { if (value == _n30) return; _n30 = value; NotifyPropertyChanged(() => n30); }
        }
        [DefaultValue(false)]
        public bool n31
        {
            get { return _n31; }
            set { if (value == _n31) return; _n31 = value; NotifyPropertyChanged(() => n31); }
        }
        [DefaultValue(false)]
        public bool n32
        {
            get { return _n32; }
            set { if (value == _n32) return; _n32 = value; NotifyPropertyChanged(() => n32); }
        }
        [DefaultValue(false)]
        public bool n33
        {
            get { return _n33; }
            set { if (value == _n33) return; _n33 = value; NotifyPropertyChanged(() => n33); }
        }
        [DefaultValue(false)]
        public bool n34
        {
            get { return _n34; }
            set { if (value == _n34) return; _n34 = value; NotifyPropertyChanged(() => n34); }
        }
        [DefaultValue(false)]
        public bool n35
        {
            get { return _n35; }
            set { if (value == _n35) return; _n35 = value; NotifyPropertyChanged(() => n35); }
        }
        [DefaultValue(false)]
        public bool n36
        {
            get { return _n36; }
            set { if (value == _n36) return; _n36 = value; NotifyPropertyChanged(() => n36); }
        }
        [DefaultValue(false)]
        public bool n37
        {
            get { return _n37; }
            set { if (value == _n37) return; _n37 = value; NotifyPropertyChanged(() => n37); }
        }
        [DefaultValue(false)]
        public bool n38
        {
            get { return _n38; }
            set { if (value == _n38) return; _n38 = value; NotifyPropertyChanged(() => n38); }
        }
        [DefaultValue(false)]
        public bool n39
        {
            get { return _n39; }
            set { if (value == _n39) return; _n39 = value; NotifyPropertyChanged(() => n39); }
        }
        [DefaultValue(false)]
        public bool n40
        {
            get { return _n40; }
            set { if (value == _n40) return; _n40 = value; NotifyPropertyChanged(() => n40); }
        }
        [DefaultValue(false)]
        public bool n41
        {
            get { return _n41; }
            set { if (value == _n41) return; _n41 = value; NotifyPropertyChanged(() => n41); }
        }
        [DefaultValue(false)]
        public bool n42
        {
            get { return _n42; }
            set { if (value == _n42) return; _n42 = value; NotifyPropertyChanged(() => n42); }
        }
        [DefaultValue(false)]
        public bool o0
        {
            get { return _o0; }
            set { if (value == _o0) return; _o0 = value; NotifyPropertyChanged(() => o0); }
        }
        [DefaultValue(false)]
        public bool o1
        {
            get { return _o1; }
            set { if (value == _o1) return; _o1 = value; NotifyPropertyChanged(() => o1); }
        }
        [DefaultValue(false)]
        public bool o2
        {
            get { return _o2; }
            set { if (value == _o2) return; _o2 = value; NotifyPropertyChanged(() => o2); }
        }
        [DefaultValue(false)]
        public bool o3
        {
            get { return _o3; }
            set { if (value == _o3) return; _o3 = value; NotifyPropertyChanged(() => o3); }
        }
        [DefaultValue(false)]
        public bool o4
        {
            get { return _o4; }
            set { if (value == _o4) return; _o4 = value; NotifyPropertyChanged(() => o4); }
        }
        [DefaultValue(false)]
        public bool o5
        {
            get { return _o5; }
            set { if (value == _o5) return; _o5 = value; NotifyPropertyChanged(() => o5); }
        }
        [DefaultValue(false)]
        public bool p0
        {
            get { return _p0; }
            set { if (value == _p0) return; _p0 = value; NotifyPropertyChanged(() => p0); }
        }
        [DefaultValue(false)]
        public bool p1
        {
            get { return _p1; }
            set { if (value == _p1) return; _p1 = value; NotifyPropertyChanged(() => p1); }
        }
        [DefaultValue(false)]
        public bool p2
        {
            get { return _p2; }
            set { if (value == _p2) return; _p2 = value; NotifyPropertyChanged(() => p2); }
        }
        [DefaultValue(false)]
        public bool p3
        {
            get { return _p3; }
            set { if (value == _p3) return; _p3 = value; NotifyPropertyChanged(() => p3); }
        }
        [DefaultValue(false)]
        public bool p4
        {
            get { return _p4; }
            set { if (value == _p4) return; _p4 = value; NotifyPropertyChanged(() => p4); }
        }
        [DefaultValue(false)]
        public bool p5
        {
            get { return _p5; }
            set { if (value == _p5) return; _p5 = value; NotifyPropertyChanged(() => p5); }
        }
        [DefaultValue(false)]
        public bool p6
        {
            get { return _p6; }
            set { if (value == _p6) return; _p6 = value; NotifyPropertyChanged(() => p6); }
        }
        [DefaultValue(false)]
        public bool p7
        {
            get { return _p7; }
            set { if (value == _p7) return; _p7 = value; NotifyPropertyChanged(() => p7); }
        }
        [DefaultValue(false)]
        public bool p8
        {
            get { return _p8; }
            set { if (value == _p8) return; _p8 = value; NotifyPropertyChanged(() => p8); }
        }
        [DefaultValue(false)]
        public bool p9
        {
            get { return _p9; }
            set { if (value == _p9) return; _p9 = value; NotifyPropertyChanged(() => p9); }
        }
        [DefaultValue(false)]
        public bool p10
        {
            get { return _p10; }
            set { if (value == _p10) return; _p10 = value; NotifyPropertyChanged(() => p10); }
        }
        [DefaultValue(false)]
        public bool p11
        {
            get { return _p11; }
            set { if (value == _p11) return; _p11 = value; NotifyPropertyChanged(() => p11); }
        }
        [DefaultValue(false)]
        public bool p12
        {
            get { return _p12; }
            set { if (value == _p12) return; _p12 = value; NotifyPropertyChanged(() => p12); }
        }
        [DefaultValue(false)]
        public bool p13
        {
            get { return _p13; }
            set { if (value == _p13) return; _p13 = value; NotifyPropertyChanged(() => p13); }
        }
        [DefaultValue(false)]
        public bool p14
        {
            get { return _p14; }
            set { if (value == _p14) return; _p14 = value; NotifyPropertyChanged(() => p14); }
        }
        [DefaultValue(false)]
        public bool p15
        {
            get { return _p15; }
            set { if (value == _p15) return; _p15 = value; NotifyPropertyChanged(() => p15); }
        }
        [DefaultValue(false)]
        public bool p16
        {
            get { return _p16; }
            set { if (value == _p16) return; _p16 = value; NotifyPropertyChanged(() => p16); }
        }
        [DefaultValue(false)]
        public bool p17
        {
            get { return _p17; }
            set { if (value == _p17) return; _p17 = value; NotifyPropertyChanged(() => p17); }
        }
        [DefaultValue(false)]
        public bool p18
        {
            get { return _p18; }
            set { if (value == _p18) return; _p18 = value; NotifyPropertyChanged(() => p18); }
        }
        [DefaultValue(false)]
        public bool p19
        {
            get { return _p19; }
            set { if (value == _p19) return; _p19 = value; NotifyPropertyChanged(() => p19); }
        }
        [DefaultValue(false)]
        public bool p20
        {
            get { return _p20; }
            set { if (value == _p20) return; _p20 = value; NotifyPropertyChanged(() => p20); }
        }
        [DefaultValue(false)]
        public bool p21
        {
            get { return _p21; }
            set { if (value == _p21) return; _p21 = value; NotifyPropertyChanged(() => p21); }
        }
        [DefaultValue(false)]
        public bool p22
        {
            get { return _p22; }
            set { if (value == _p22) return; _p22 = value; NotifyPropertyChanged(() => p22); }
        }
        [DefaultValue(false)]
        public bool p23
        {
            get { return _p23; }
            set { if (value == _p23) return; _p23 = value; NotifyPropertyChanged(() => p23); }
        }
        [DefaultValue(false)]
        public bool p24
        {
            get { return _p24; }
            set { if (value == _p24) return; _p24 = value; NotifyPropertyChanged(() => p24); }
        }
        [DefaultValue(false)]
        public bool p25
        {
            get { return _p25; }
            set { if (value == _p25) return; _p25 = value; NotifyPropertyChanged(() => p25); }
        }
        [DefaultValue(false)]
        public bool p26
        {
            get { return _p26; }
            set { if (value == _p26) return; _p26 = value; NotifyPropertyChanged(() => p26); }
        }
        [DefaultValue(false)]
        public bool p27
        {
            get { return _p27; }
            set { if (value == _p27) return; _p27 = value; NotifyPropertyChanged(() => p27); }
        }
        [DefaultValue(false)]
        public bool p28
        {
            get { return _p28; }
            set { if (value == _p28) return; _p28 = value; NotifyPropertyChanged(() => p28); }
        }
        [DefaultValue(false)]
        public bool p29
        {
            get { return _p29; }
            set { if (value == _p29) return; _p29 = value; NotifyPropertyChanged(() => p29); }
        }
        [DefaultValue(false)]
        public bool p30
        {
            get { return _p30; }
            set { if (value == _p30) return; _p30 = value; NotifyPropertyChanged(() => p30); }
        }
        [DefaultValue(false)]
        public bool p31
        {
            get { return _p31; }
            set { if (value == _p31) return; _p31 = value; NotifyPropertyChanged(() => p31); }
        }
        [DefaultValue(false)]
        public bool p32
        {
            get { return _p32; }
            set { if (value == _p32) return; _p32 = value; NotifyPropertyChanged(() => p32); }
        }
        [DefaultValue(false)]
        public bool p33
        {
            get { return _p33; }
            set { if (value == _p33) return; _p33 = value; NotifyPropertyChanged(() => p33); }
        }
        [DefaultValue(false)]
        public bool p34
        {
            get { return _p34; }
            set { if (value == _p34) return; _p34 = value; NotifyPropertyChanged(() => p34); }
        }
        [DefaultValue(false)]
        public bool p35
        {
            get { return _p35; }
            set { if (value == _p35) return; _p35 = value; NotifyPropertyChanged(() => p35); }
        }
        [DefaultValue(false)]
        public bool p36
        {
            get { return _p36; }
            set { if (value == _p36) return; _p36 = value; NotifyPropertyChanged(() => p36); }
        }
        [DefaultValue(false)]
        public bool p37
        {
            get { return _p37; }
            set { if (value == _p37) return; _p37 = value; NotifyPropertyChanged(() => p37); }
        }
        [DefaultValue(false)]
        public bool p38
        {
            get { return _p38; }
            set { if (value == _p38) return; _p38 = value; NotifyPropertyChanged(() => p38); }
        }
        [DefaultValue(false)]
        public bool p39
        {
            get { return _p39; }
            set { if (value == _p39) return; _p39 = value; NotifyPropertyChanged(() => p39); }
        }
        [DefaultValue(false)]
        public bool p40
        {
            get { return _p40; }
            set { if (value == _p40) return; _p40 = value; NotifyPropertyChanged(() => p40); }
        }
        [DefaultValue(false)]
        public bool p41
        {
            get { return _p41; }
            set { if (value == _p41) return; _p41 = value; NotifyPropertyChanged(() => p41); }
        }
        [DefaultValue(false)]
        public bool p42
        {
            get { return _p42; }
            set { if (value == _p42) return; _p42 = value; NotifyPropertyChanged(() => p42); }
        }
        [DefaultValue(false)]
        public bool p43
        {
            get { return _p43; }
            set { if (value == _p43) return; _p43 = value; NotifyPropertyChanged(() => p43); }
        }
        [DefaultValue(false)]
        public bool p44
        {
            get { return _p44; }
            set { if (value == _p44) return; _p44 = value; NotifyPropertyChanged(() => p44); }
        }
        [DefaultValue(false)]
        public bool p45
        {
            get { return _p45; }
            set { if (value == _p45) return; _p45 = value; NotifyPropertyChanged(() => p45); }
        }
        [DefaultValue(false)]
        public bool p46
        {
            get { return _p46; }
            set { if (value == _p46) return; _p46 = value; NotifyPropertyChanged(() => p46); }
        }
        [DefaultValue(false)]
        public bool p47
        {
            get { return _p47; }
            set { if (value == _p47) return; _p47 = value; NotifyPropertyChanged(() => p47); }
        }
        [DefaultValue(false)]
        public bool p48
        {
            get { return _p48; }
            set { if (value == _p48) return; _p48 = value; NotifyPropertyChanged(() => p48); }
        }
        [DefaultValue(false)]
        public bool p49
        {
            get { return _p49; }
            set { if (value == _p49) return; _p49 = value; NotifyPropertyChanged(() => p49); }
        }
        [DefaultValue(false)]
        public bool p50
        {
            get { return _p50; }
            set { if (value == _p50) return; _p50 = value; NotifyPropertyChanged(() => p50); }
        }
        [DefaultValue(false)]
        public bool p51
        {
            get { return _p51; }
            set { if (value == _p51) return; _p51 = value; NotifyPropertyChanged(() => p51); }
        }
        [DefaultValue(false)]
        public bool t0
        {
            get { return _t0; }
            set { if (value == _t0) return; _t0 = value; NotifyPropertyChanged(() => t0); }
        }
        [DefaultValue(false)]
        public bool t1
        {
            get { return _t1; }
            set { if (value == _t1) return; _t1 = value; NotifyPropertyChanged(() => t1); }
        }
        [DefaultValue(false)]
        public bool t2
        {
            get { return _t2; }
            set { if (value == _t2) return; _t2 = value; NotifyPropertyChanged(() => t2); }
        }
        [DefaultValue(false)]
        public bool t3
        {
            get { return _t3; }
            set { if (value == _t3) return; _t3 = value; NotifyPropertyChanged(() => t3); }
        }
        [DefaultValue(false)]
        public bool t4
        {
            get { return _t4; }
            set { if (value == _t4) return; _t4 = value; NotifyPropertyChanged(() => t4); }
        }
        [DefaultValue(false)]
        public bool t5
        {
            get { return _t5; }
            set { if (value == _t5) return; _t5 = value; NotifyPropertyChanged(() => t5); }
        }
        [DefaultValue(false)]
        public bool x0
        {
            get { return _x0; }
            set { if (value == _x0) return; _x0 = value; NotifyPropertyChanged(() => x0); }
        }
        [DefaultValue(false)]
        public bool x1
        {
            get { return _x1; }
            set { if (value == _x1) return; _x1 = value; NotifyPropertyChanged(() => x1); }
        }
        [DefaultValue(false)]
        public bool x2
        {
            get { return _x2; }
            set { if (value == _x2) return; _x2 = value; NotifyPropertyChanged(() => x2); }
        }
        [DefaultValue(false)]
        public bool x3
        {
            get { return _x3; }
            set { if (value == _x3) return; _x3 = value; NotifyPropertyChanged(() => x3); }
        }
        [DefaultValue(false)]
        public bool x4
        {
            get { return _x4; }
            set { if (value == _x4) return; _x4 = value; NotifyPropertyChanged(() => x4); }
        }
        [DefaultValue(false)]
        public bool x5
        {
            get { return _x5; }
            set { if (value == _x5) return; _x5 = value; NotifyPropertyChanged(() => x5); }
        }
        [DefaultValue(false)]
        public bool x6
        {
            get { return _x6; }
            set { if (value == _x6) return; _x6 = value; NotifyPropertyChanged(() => x6); }
        }
        [DefaultValue(false)]
        public bool x7
        {
            get { return _x7; }
            set { if (value == _x7) return; _x7 = value; NotifyPropertyChanged(() => x7); }
        }
        #endregion
    }


}