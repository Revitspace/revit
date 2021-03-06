﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOCQR.Revit
{
    public partial class ProjectSelectFrm : Form
    {

        public Project SelectedProject;
        private DOCQRclient client;

        public ProjectSelectFrm(DOCQRclient Client)
        {
            InitializeComponent();
            client = Client;
        }

        private void ProjectSelectFrm_Load(object sender, EventArgs e)
        {
            // TODO: Remove
            //this.comboBox1.Items.Add("A)");
            this.comboBox1.DataSource = client.GetProjects().OrderBy( p => p.name).ToList();               // get info for drop down menu
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedProject = this.comboBox1.SelectedItem as Project;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
