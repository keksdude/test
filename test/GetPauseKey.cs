﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class GetPauseKey : Form
    {
        public delegate void ChangedEventHandler(DataSet sender);
        public event ChangedEventHandler Changed;

        public GetPauseKey()
        {
            InitializeComponent();
        }
    }
}
