using LibraryWinForms.Models;
using LibraryWinForms.Repositories;
using LibraryWinForms.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryWinForms
{
    public partial class Form1 : Form
    {
        private LibraryService service;

        private ComboBox cmbType;
        private TextBox txtTitle;
        private TextBox txtYear;
        private TextBox txtExtra;

        private DataGridView grid;

        private Button btnAdd;
        private Button btnDelete;
        private Button btnStats;
        private Button btnEdit;

        public Form1()
        {
            service = new LibraryService(new LibraryRepository());
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Library System";
            this.Size = new Size(900, 500);

            Panel left = new Panel { Dock = DockStyle.Left, Width = 250 };
            Panel right = new Panel { Dock = DockStyle.Fill };

            this.Controls.Add(right);
            this.Controls.Add(left);

            // TYPE
            cmbType = new ComboBox();
            cmbType.Items.Add("Книга");
            cmbType.Items.Add("Журнал");
            cmbType.SelectedIndex = 0;
            cmbType.Location = new Point(20, 20);

            txtTitle = CreateBox("Название", new Point(20, 60));
            txtYear = CreateBox("Год", new Point(20, 100));
            txtExtra = CreateBox("Автор / Выпуск", new Point(20, 140));

            // BUTTONS
            btnAdd = new Button { Text = "Добавить", Location = new Point(20, 190), Width = 200 };
            btnDelete = new Button { Text = "Удалить", Location = new Point(20, 230), Width = 200 };
            btnStats = new Button { Text = "Статистика", Location = new Point(20, 270), Width = 200 };
            btnEdit = new Button { Text = "Редактировать", Location = new Point(20, 310), Width = 200 };

            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnStats.Click += BtnStats_Click;
            btnEdit.Click += BtnEdit_Click;

            left.Controls.Add(cmbType);
            left.Controls.Add(txtTitle);
            left.Controls.Add(txtYear);
            left.Controls.Add(txtExtra);
            left.Controls.Add(btnAdd);
            left.Controls.Add(btnDelete);
            left.Controls.Add(btnStats);
            left.Controls.Add(btnEdit);

            // GRID
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.Columns.Add("Id", "ID");
            grid.Columns.Add("Type", "Тип");
            grid.Columns.Add("Title", "Название");
            grid.Columns.Add("Year", "Год");
            grid.Columns.Add("Extra", "Автор / Выпуск");

            right.Controls.Add(grid);

            RefreshList();
        }

        private TextBox CreateBox(string placeholder, Point location)
        {
            TextBox tb = new TextBox();
            tb.Text = placeholder;
            tb.ForeColor = Color.Gray;
            tb.Location = location;
            tb.Width = 200;

            tb.Enter += (s, e) =>
            {
                if (tb.Text == placeholder)
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;
                }
            };

            tb.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.ForeColor = Color.Gray;
                }
            };

            return tb;
        }

        // ADD
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            int year = int.Parse(txtYear.Text); if (cmbType.SelectedIndex == 0)
                service.AddBook(title, year, txtExtra.Text);
            else
                service.AddMagazine(title, year, int.Parse(txtExtra.Text));

            RefreshList();
        }

        // DELETE
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
            service.Remove(id);

            RefreshList();
        }

        // EDIT
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);

            string title = txtTitle.Text;
            int year = int.Parse(txtYear.Text);
            string extra = txtExtra.Text;

            service.Update(id, title, year, extra);

            RefreshList();
        }

        // STATS
        private void BtnStats_Click(object sender, EventArgs e)
        {
            MessageBox.Show(service.GetStats());
        }

        // REFRESH GRID
        private void RefreshList()
        {
            grid.Rows.Clear();

            foreach (var item in service.GetAll())
            {
                string type = item is Book ? "Книга" : "Журнал";

                string extra = "";

                if (item is Book b)
                    extra = b.Author;
                else if (item is Magazine m)
                    extra = m.Issue.ToString();

                grid.Rows.Add(item.Id, type, item.Title, item.Year, extra);
            }
        }
    }
}