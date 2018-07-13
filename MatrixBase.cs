using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MatrixGraphicsGenerator
{
    public abstract class MatrixBase
    {
        const int elementSize = 45;

        TextBox codeText;
        TextBox nameText;
        Button codeTypeButton;
        Grid grid;

        protected MatrixRow[] rows;
        protected CodeType currentType = CodeType.HEX;
        Tuple<int, int> size;

        public enum CodeType
        {
            HEX, BIN
        }

        public MatrixBase(Tuple<int, int> matrixSize, Grid matrixGrid, TextBox codeTextBox, Button codeButton, TextBox nameTextBox)
        {
            // Set UI elements
            codeText = codeTextBox;
            nameText = nameTextBox;
            codeTypeButton = codeButton;
            grid = matrixGrid;

            codeTypeButton.Click += (s, e) =>
            {
                UpdateCode(currentType == CodeType.HEX ? CodeType.BIN : CodeType.HEX, true);
            };

            CreateMatrix(matrixSize);
            UpdateCode(CodeType.HEX, true);
        }

        /// <summary>
        /// Used when changes appear only in one row (e.g. LED is toggled)
        /// </summary>
        public void UpdateCode(int rowIndex)
        {
            rows[rowIndex].RecalculateCode();
            UpdateCode(currentType, false);
        }

        public void UpdateCode(CodeType type, bool recalculateAll)
        {
            if (recalculateAll)
            {
                foreach (MatrixRow row in rows)
                {
                    row.RecalculateCode();
                }
            }

            codeText.Text = GetCode(type);

            if (currentType != type)
                codeTypeButton.Content = "To " + currentType.ToString();

            currentType = type;
        }

        /// <summary>
        /// Sets the matrix according to MatrixData (from collection)
        /// </summary>
        public void SetMatrix(MatrixData data)
        {
            nameText.Text = data.Name;
            CreateMatrix(data.Size);

            // Enable LEDs
            for (int j = 0; j < size.Item2; j++)
            {
                for (int i = 0; i < size.Item1; i++)
                {
                    int rowCode = data.RowCodes[j];
                    int place = 1 << Math.Abs(i - size.Item1 + 1);

                    if ((rowCode & place) == place)
                        rows[j].LEDs[i].Enabled = true;
                }
            }

            UpdateCode(currentType, true);
        }

        /// <summary>
        /// Returns MatrixData used for storing in a collection
        /// </summary>
        public MatrixData GetMatrixData()
        {
            string name = nameText.Text;
            string code = codeText.Text;
            List<int> rowCodes = new List<int>();
            foreach (MatrixRow row in rows)
            {
                rowCodes.Add(row.Code);
            }

            return new MatrixData(name, code, rowCodes, size);
        }

        /// <summary>
        /// Creates a matrix including the visual representation
        /// </summary>
        public void CreateMatrix(Tuple<int, int> matrixSize)
        {
            size = matrixSize;
            rows = new MatrixRow[size.Item2];

            // Clear the grid to make sure nothing is left over
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            // Resize
            grid.Width = elementSize * size.Item1;
            grid.Height = elementSize * size.Item2;

            // Create collumns and rows
            for (int i = 0; i < size.Item1; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                grid.ColumnDefinitions.Add(column);
            }

            for (int i = 0; i < size.Item2; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(elementSize);
                grid.RowDefinitions.Add(row);

                rows[i] = new MatrixRow(size.Item1);
            }

            // Iterate through the matrix
            for (int row = 0; row < rows.Length; row++)
            {
                for (int column = 0; column < rows[row].LEDs.Length; column++)
                {
                    // Create visual representation of the LED
                    Ellipse ellipse = new Ellipse();
                    ellipse.Stroke = new SolidColorBrush(Colors.Black);

                    // Create the "back-end" of the LED
                    LED led = new LED(this, ellipse, row);
                    rows[row].LEDs[column] = led;

                    // Assign the ellipse to the grid
                    Grid.SetColumn(ellipse, column);
                    Grid.SetRow(ellipse, row);
                    grid.Children.Add(ellipse);
                }
            }
        }

        /// <summary>
        /// Generates a code for the matrix with prefixes
        /// </summary>
        string GetCode(CodeType type)
        {
            StringBuilder builder = new StringBuilder();
            foreach (MatrixRow row in rows)
            {
                switch (type)
                {
                    case CodeType.HEX:
                        builder.Append("0x");
                        builder.Append(Convert.ToString(row.Code, 16));
                        break;
                    case CodeType.BIN:
                        builder.Append("0b");
                        builder.Append(Convert.ToString(row.Code, 2));
                        break;
                }

                if (row != rows[size.Item2 - 1])
                    builder.Append(", ");
            }

            return builder.ToString();
        }

        protected class MatrixRow
        {
            int code;

            public LED[] LEDs { get; private set; }
            public int Code
            {
                get { return code; }
                set
                {
                    int fullRow = ~(1 << (LEDs.Length));
                    if (value > fullRow)
                        code = fullRow & value;
                    else
                        code = value;

                    // Enable LEDs according to the new code
                    for (int i = 0; i < LEDs.Length; i++)
                    {
                        int place = 1 << Math.Abs(i - LEDs.Length + 1);

                        if ((code & place) == place)
                            LEDs[i].Enabled = true;
                        else
                            LEDs[i].Enabled = false;
                    }
                }
            }

            public MatrixRow(int size)
            {
                LEDs = new LED[size];
            }

            public void RecalculateCode()
            {
                code = 0;
                for (int i = 0; i < LEDs.Length; i++)
                {
                    if (LEDs[i].Enabled)
                        code += 1 << Math.Abs(i - LEDs.Length + 1);
                }
            }
        }
    }
}
