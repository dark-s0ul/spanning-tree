using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace WindowsFormsApplication4 {
	public partial class Form1 : Form {
		int n = 1;
		float[,] input, result;
		ArrayList koord = new ArrayList();

		public Form1() {
			InitializeComponent();
		}

		private void control() {
			input = new float[n, n];
			result = new float[n, n];
			bool flag = true;
			for(short i = 0; i < n; i++) {
				for(short j = 0; j < n; j++) {
					try {
						input[i, j] = Convert.ToSingle(dataGridView1[j + 1, i + 1].Value);
					} catch {
						input[i, j] = 0;
						flag = false;
					}
				}
			}
			if(flag == false) {
				MessageBox.Show("Input value has an incorrect format", "Error");
			}
		}

		private void calculate() {
			control();
			short[,] b = new short[n, n];
			float[] c;

			for(short i = 0; i < n; i++) {
				for(short j = 0; j < n; j++) {
					b[i, j] = -1;
				}
			}

			for(short i = 0; i < n; i++) {
				b[0, i] = i;
			}

			short re = 0;

			for(short i = 0; i < n; i++) {
				for(short j = 0; j < i; j++) {
					if(input[i, j] != 0) {
						re++;
					}
				}
			}
			c = new float[re];
			re = 0;

			for(short i = 0; i < n; i++) {
				for(short j = 0; j < i; j++) {
					if(input[i, j] != 0) {
						c[re] = input[i, j];
						re++;
					}
				}
			}

			float g;
			short l;
			while(true) {
				l = 0;
				for(short i = 1; i < re; i++) {
					if(c[i] < c[i - 1]) {
						g = c[i - 1];
						c[i - 1] = c[i];
						c[i] = g;
						l++;
					}
				}
				if(l == 0) break;
			}

			short com1 = 0, com2 = 0, n3;
			bool flag = false;
			for(short k = 0; k < re; k++) {
				for(short i = 0; i < n; i++) {
					for(short j = 0; j < i; j++) {
						if(c[k] == input[i, j] && c[k] != result[i, j]) {
							flag = false;
							for(short n1 = 0; n1 < n; n1++) {
								for(short n2 = 0; n2 < n; n2++) {
									if(i == b[n1, n2]) {
										com1 = n2;
										flag = true;
									}
								}
								if(flag) break;
							}
							flag = false;
							for(short n1 = 0; n1 < n; n1++) {
								for(short n2 = 0; n2 < n; n2++) {
									if(j == b[n1, n2]) {
										com2 = n2;
										flag = true;
									}
								}
								if(flag) break;
							}
							if(com1 != com2) {
								result[i, j] = c[k];
								result[j, i] = c[k];

								n3 = 0;
								for(short t = 0; t < n; t++) {
									if(b[t, com1] == -1) {
										while(b[n3, com2] != -1) {
											b[n3 + t, com1] = b[n3, com2];
											b[n3, com2] = -1;
											n3++;
										}
										break;
									}
								}
							}
						}
					}
				}
				c[k] = 0;
			}
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
			n = (byte)numericUpDown1.Value;
			dataGridView1.ColumnCount = n + 1;
			dataGridView1.RowCount = n + 1;
			dataGridView1[0, 0].ReadOnly = true;
			for(short i = 0; i < n; i++) {
				dataGridView1[0, i + 1].Value = i + 1;
				dataGridView1[i + 1, 0].Value = i + 1;
				dataGridView1[0, i + 1].ReadOnly = true;
				dataGridView1[i + 1, 0].ReadOnly = true;
				dataGridView1[i + 1, i + 1].Style.BackColor = Color.Gray;
				dataGridView1[i + 1, i + 1].Value = 0;
				dataGridView1[i + 1, i + 1].ReadOnly = true;
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
			numericUpDown1_ValueChanged(sender, e);
		}

		private void button2_Click(object sender, EventArgs e) {
			Random x = new Random();
			for(short i = 0; i < n; i++) {
				for(short j = 0; j < n; j++) {
					if(i != j) {
						dataGridView1[i + 1, j + 1].Value = Math.Max(0, x.Next(-4*n, 5*n));
					}
				}
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			if(MessageBox.Show("Are you sure you want to clean the matrix?", "Confirm your choice", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				for(short i = 0; i < n; i++) {
					for(short j = 0; j < n; j++) {
						if(i != j) {
							dataGridView1[i + 1, j + 1].Value = null;
							input[i, j] = 0;
							result[i, j] = 0;
						}
					}
				}
				koord.Clear();
				n = 1;
				numericUpDown1.Value = n;
				pictureBox1.Invalidate();
				button1.Enabled = false;
			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			if(e.ColumnIndex == e.RowIndex) e.Cancel = true;
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			button1.Enabled = true;
			try {
				Convert.ToDouble(dataGridView1[e.ColumnIndex, e.RowIndex].Value);
				dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
				dataGridView1[e.RowIndex, e.ColumnIndex].Value = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
			} catch {
				dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
			}
			pictureBox1.Invalidate();
		}

		private void button1_Click(object sender, EventArgs e) {
			koord.Clear();
			Random z1 = new Random();
			Point x = new Point();
			for(short i = 0; i < n; i++) {
				x.X = z1.Next(20, pictureBox1.ClientSize.Width - 20);
				x.Y = z1.Next(20, pictureBox1.ClientSize.Height - 20);
				koord.Add(x);
			}
			pictureBox1.Invalidate();
		}

		private void groupBox1_Enter(object sender, EventArgs e) {}

		private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) {}

		private void pictureBox1_Click(object sender, EventArgs e) {}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {}

		private void pictureBox1_Click_1(object sender, EventArgs e) {}

		public void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
			MouseButtons mouse = e.Button;
			if(mouse == MouseButtons.Left) {
				koord.Add(e.Location);
			}
			n = koord.Count;
			numericUpDown1.Value = n;
			pictureBox1.Invalidate();
		}

		private void label1_Click(object sender, EventArgs e) {}

		public void pictureBox1_Paint(object sender, PaintEventArgs e) {
			if(koord.Count != n) return;
			Point[] pnt = (Point[])koord.ToArray(typeof(Point));
			Font ft = new Font("Times New Roman", 16);
			Size sz = new Size(8, 8);
			int k = 0;

			calculate();

			e.Graphics.Clear(pictureBox1.BackColor);
			Graphics gr = e.Graphics;
			for(short i = 0; i < n; i++) {
				for(short j = 0; j < n; j++) {
					if(input[i, j] != 0 && result[i, j] == 0) {
						e.Graphics.DrawString(input[i, j].ToString(), ft, Brushes.DarkSlateBlue, (pnt[i].X + pnt[j].X) / 2, (pnt[i].Y + pnt[j].Y) / 2);
						e.Graphics.DrawLine(Pens.DarkSlateGray, pnt[i].X + 4, pnt[i].Y + 4, pnt[j].X + 4, pnt[j].Y + 4);
					} else if(result[i, j] != 0) {
						e.Graphics.DrawString(result[i, j].ToString(), ft, Brushes.Green, (pnt[i].X + pnt[j].X) / 2, (pnt[i].Y + pnt[j].Y) / 2);
						e.Graphics.DrawLine(Pens.DarkRed, pnt[i].X + 4, pnt[i].Y + 4, pnt[j].X + 4, pnt[j].Y + 4);
					}
				}
			}

			foreach(Point pt in koord) {
				e.Graphics.FillEllipse(Brushes.Black, new Rectangle(pt, sz));
				e.Graphics.DrawString((k + 1).ToString(), ft, Brushes.Black, pt.X - 20, pt.Y - 20);
				k++;
			}
			ft.Dispose();
		}
	}
}
