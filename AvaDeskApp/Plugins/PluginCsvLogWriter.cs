﻿using System;
using System.IO;
using System.Globalization;

using Positec;
using Plugin;
using System.Collections.Generic;

public class PluginCsvLogWriter : IPlugin {
  private void Write(PluginData pd) {
    string fn = Path.Combine(DeskApp.DirData, pd.Name + ".csv");
    string dts = string.Format("{0} {1}", pd.Config.Date, pd.Config.Time); // parsable DateTime string
    DateTime dt = DateTime.ParseExact(dts, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    bool nf = !File.Exists(fn); // write column header only on new file

    using (StreamWriter sw = new StreamWriter(fn, true)) {
      var d = pd.Data;
      Battery b = d.Battery;
      Statistic s = d.Statistic;

      if( nf ) sw.WriteLine("dt tm;ls;le;lz;lk;bt.t;bt.v;bt.p;bt.c;st.b;st.d;st.wt;dmp0;dmp1;dmp2;rsi");
      sw.Write(string.Format("{0};", dt));
      sw.Write(string.Format("{0};{1};{2};{3};", d.LastState, d.LastError, d.LastZone, d.Lock));
      sw.Write(string.Format("{0};{1};{2};{3};", b.Temp, b.Volt, b.Perc, b.Charging));
      sw.Write(string.Format("{0};{1};{2};", s.Blade, s.Distance, s.WorkTime));
      sw.Write(string.Format("{0};{1};{2};{3}", d.Orient[0], d.Orient[1], d.Orient[2], d.RecvSignal));
      sw.WriteLine();
    }
  }

  string IPlugin.Desc {
    get { return "Write mower data to CSV file: <name>.csv"; }
  }
  List<PluginParaBase> IPlugin.Paras => new();
  void IPlugin.Doit(PluginData pd) {
    Write(pd);
  }
  void IPlugin.Todo(PluginData pd) {
    Write(pd);
  }

  void IDisposable.Dispose() { }
}
