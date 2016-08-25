using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using GTLutils;

namespace CentralControl
{
    public partial class DataQueryForm : Form
    {
        public ControlForm FatherForm;

        private string select, where;

        List<string> selecteditems;

        protected void display()
        {
            if (!where.Equals(""))
                textBox1.Text = "select " + select + " from " + DBUtil.chinesetoenglish[dataTableComboBox.Text] + " where " + where;
            else
                textBox1.Text = "select " + select + " from " + DBUtil.chinesetoenglish[dataTableComboBox.Text];
        }

        public DataQueryForm()
        {
            InitializeComponent();
            /*foreach (DeviceType type in EnumHelper.TypeEnums)
            {
                deviceTypeComboBox.Items.Add(EnumHelper.getDeviceTypeString(type));
            }
            deviceTypeComboBox.SelectedIndex = 0;*/

            operationTypeComboBox.Items.Add("=");
            operationTypeComboBox.Items.Add("!=");
            //operationTypeComboBox.SelectedIndex = 0;

            foreach (Logics logic in LogicHelper.LogicEnums)
            {
                logicTypeComboBox.Items.Add(LogicHelper.getLogicString(logic));
            }
            logicTypeComboBox.SelectedIndex = 0;
            select = "";
            where = "";

        }

        private void DataQueryForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            FatherForm.Enabled = false;
            ArrayList list = DBUtil.getTableList();
            dataTableComboBox.Items.Clear();
            foreach (String s in list)
            {
                dataTableComboBox.Items.Add(DBUtil.englishtochinese[s]);
            }
            dataTableComboBox.SelectedIndex = 0;
            //segmentComboBox.Items.Add("数据插入时间");
            //segmentComboBox.Items.Add("仪器标识");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataQueryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

        private void dataTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tableName = dataTableComboBox.SelectedItem.ToString();
            ArrayList list = DBUtil.getTableColumns(DBUtil.chinesetoenglish[tableName]);
            filtercombobox.Items.Clear();
            segmentComboBox.Items.Clear();
            //filtercombobox.Items.Add("所有属性");
            foreach (String s in list) 
            {
                filtercombobox.Items.Add(DBUtil.englishtochinese[s]);
                segmentComboBox.Items.Add(DBUtil.englishtochinese[s]);
            }
            if(list.Count > 0) filtercombobox.SelectedIndex = 0;
            textBox1.Text = "";
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (opValueTextBox.Text.Equals("") || operationTypeComboBox.Text.Equals("") || segmentComboBox.Text.Equals(""))
                return;
            String con = "";
            if (DBUtil.cancompare[segmentComboBox.Text] != 1)
                con = DBUtil.chinesetoenglish[segmentComboBox.SelectedItem.ToString()] + " " + operationTypeComboBox.SelectedItem.ToString() + " '" + opValueTextBox.Text + "' ";
            else
                con = DBUtil.chinesetoenglish[segmentComboBox.SelectedItem.ToString()] + " " + operationTypeComboBox.SelectedItem.ToString() + " " + opValueTextBox.Text + " ";
            String opStr = "and";
            if (logicTypeComboBox.SelectedIndex > 0) opStr = "or";
            if (where.Equals("")) where = con;
            else where = "(" + where + ") " + opStr + " " + con;
            display();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            select = "";
            where = "";
            textBox1.Clear();
            searchResultListView.Clear();
        }

        private void searchDataButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请添加要查看的属性");
                return;
            }
                //textBox1.Text = "select * from " + DBUtil.chinesetoenglish[dataTableComboBox.Text];
            List<List<string>> list = DBUtil.executeQueryCmd(textBox1.Text);
            if (list.Count == 0)
            {
                MessageBox.Show("数据库查询语句有误");
                return;
            }
            searchResultListView.Columns.Clear();
            for (int i = 0; i < list[0].Count; i++)
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = DBUtil.englishtochinese[(String)list[0][i]];
                searchResultListView.Columns.Add(header);
            }

            searchResultListView.Items.Clear();
            for (int i=1; i<list.Count(); i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = list[i][0];
                for (int j = 1; j < list[0].Count; j++)
                {
                    item.SubItems.Add(list[i][j]);
                }
                searchResultListView.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String con = DBUtil.chinesetoenglish[filtercombobox.Text];
            if (select.Equals("")) select = con;
            else select = select+" , " + con;
            display();
        }

        private void filtercombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void segmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DBUtil.cancompare[segmentComboBox.Text] == 0)
            {
                operationTypeComboBox.Items.Clear();
                operationTypeComboBox.Items.Add("=");
                operationTypeComboBox.Items.Add("!=");
            }
            else
            {
                operationTypeComboBox.Items.Clear();
                foreach (Operations op in OperationHelper.OpeEnums)
                {
                    operationTypeComboBox.Items.Add(OperationHelper.getOperationString(op));
                }
            }

        }
    }
}
