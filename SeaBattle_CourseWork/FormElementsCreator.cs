using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public static class FormElementsCreator
    {
        //Метод создания кнопок
        public static Button CreateButton(string text, Color backColor, Size size)
        {
            Button button = new Button
            {
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = backColor,
                UseVisualStyleBackColor = false,
                Size = new Size(40, 40),
                Text = text,
                Font = new Font("Webdings", 24, FontStyle.Regular, GraphicsUnit.Point),
                TextAlign = ContentAlignment.TopCenter,
            };
            return button;
        }

        public static Button CreateButtonForMainMenu(string text, Color backColor, Size size)
        {
            Button button = new Button
            {
                ForeColor = Color.Black,
                BackColor = backColor,
                Text = text,
                Size = size,
                Font = new Font("Calibri", 15, FontStyle.Regular, GraphicsUnit.Point, 186),

            };
            return button;
        }

        public static Label CreateHeaderCell(int x, int y, string text, int CellSize)
        {
            var cell = new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text,
                Location = new Point(x, y),
                Width = CellSize,
                Height = CellSize
            };
            return cell;
        }

        public static Label CreateLabel(string text, Color color)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                ForeColor = color,
                TextAlign = ContentAlignment.TopLeft
            };
        }

        public static TextBox CreateTextBox(string text, Color color)
        {
            TextBox textBox = new TextBox();
            textBox.Width = 250;
            textBox.Font = new Font("Calibri", 26);
            textBox.ForeColor = Color.FromArgb(255, 174, 0);
            textBox.Text = text;
            textBox.Margin = Padding.Empty;
            textBox.MaxLength = 11;
            textBox.BorderStyle = BorderStyle.None;
            return textBox;
        }

        public static RadioButton CreateRadioButton(string text)
        {
            RadioButton radioButton = new RadioButton();
            radioButton.Text = text;
            radioButton.Font = new Font("Calibri", 20);
            radioButton.Width = 300;
            radioButton.Appearance = Appearance.Normal;
            return radioButton;
        }
    }


}
