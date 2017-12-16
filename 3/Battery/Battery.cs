﻿using System;
using System.Windows.Forms;

namespace Battery
{
    public partial class Battery : Form
    {
        private readonly BatteryManager _manager = new BatteryManager();

        public Battery()
        {
            InitializeComponent();
        }

        private void Battery_Load(object sender, EventArgs e)
        {
            _manager.Init();
            if (_manager.State == "Online") timeoutBar.Enabled = false;
            UpdateBatteryStatus(null, null);
            UpdateTimer.Tick += UpdateBatteryStatus;
            UpdateTimer.Interval = 2000;
            UpdateTimer.Start();

            timeoutBar.Value = _manager.PrevTimeout;
            timeoutLabel.Text = @"Screen timeout in minutes(only for battery) 
                " + timeoutBar.Value + @" min now.";
        }

        private void UpdateBatteryStatus(object sender, EventArgs e)
        {
            _manager.UpdateState();
            State.Text = _manager.State;
            Percentage.Text = _manager.Percentage;
            timeLeft.Text = _manager.TimeLeft;
            if (_manager.PrevState != State.Text)
            {
                if (_manager.PrevState == "Offline")
                {
                    timeoutBar.Value = _manager.PrevTimeout;
                    timeoutLabel.Text = @"Screen timeout in minutes(only for battery) 
                        " + timeoutBar.Value + @" min now.";
                    timeoutBar.Enabled = false;
                }
                else
                {
                    timeoutBar.Enabled = true;
                }
                _manager.PrevState = State.Text;
            }
        }

        private void Battery_FormClosing(object sender, FormClosingEventArgs e)
        {
            timeoutBar.Value = _manager.PrevTimeout;
        }

        private void timeoutBar_ValueChanged(object sender, EventArgs e)
        {
            timeoutLabel.Text = @"Screen timeout in minutes(only for battery) 
                " + timeoutBar.Value + @" min now.";
            _manager.SetTimeout(timeoutBar.Value);
        }
    }
}