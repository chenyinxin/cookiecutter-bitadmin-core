using {{cookiecutter.project_name}}.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitAdminService
{
    public partial class ServiceManagerForm : Form
    {
        public ServiceManagerForm()
        {
            InitializeComponent();
        }

        IpcClientChannel channel = new IpcClientChannel();
        private void ServiceManagerForm_Load(object sender, EventArgs e)
        {
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterActivatedClientType(typeof(ServiceManager), "ipc://BitAdminChannel/WindowsServices");

                listView1.Select();
                LoadService();

                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.SaveLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LoadService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.SaveLog(ex);
            }
        }

        private void ToolStripMenuItemOne_Click(object sender, EventArgs e)
        {
            try
            {
                var select = listView1.SelectedItems[0];
                var key = select.Text;
                var status = select.SubItems[4].Text;

                ServiceManager manager = new ServiceManager();
                switch (status)
                {
                    case "已停止":
                        manager.Start(key);
                        MessageBox.Show("启动成功");
                        break;
                    case "已启动":
                        manager.Stop(key);
                        MessageBox.Show("停止成功");
                        break;
                    default:
                        MessageBox.Show("服务正在运行，不能停止");
                        break;
                }
                LoadService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.SaveLog(ex);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView1.SelectedItems.Count > 0)
            {
                contextMenuStrip1.Show(listView1, e.Location);//鼠标右键按下弹出菜单
            }
        }

        private void LoadService()
        {

            ServiceManager manager = new ServiceManager();
            var services = manager.GetServices();

            int index = 0;
            if (listView1.SelectedItems.Count > 0)
                index = listView1.SelectedItems[0].Index;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var service in services)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = service.Name;
                lvi.SubItems.Add(service.Group);
                lvi.SubItems.Add(service.StartTime);
                lvi.SubItems.Add(service.Interval + "秒");
                lvi.SubItems.Add(service.Status);
                lvi.SubItems.Add(service.PreviousFireTime);
                lvi.SubItems.Add(service.NextFireTime);
                lvi.SubItems.Add(service.Remark);

                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
            listView1.Items[index].Selected = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                LoadService();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.SaveLog(ex);
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {

        }
    }
}
