using System;
using System.Windows;
using System.Windows.Controls;

namespace MatrixGraphicsGenerator
{
    public class Matrix : MatrixBase
    {
        public Matrix(Tuple<int, int> matrixSize, Grid grid, TextBox codeTextBox, Button codeButton, TextBox nameTextBox) :
            base (matrixSize, grid, codeTextBox, codeButton, nameTextBox) { }

        /// <summary>
        /// Shifts the matrix horizontally
        /// </summary>
        public void ShiftHorizontally(bool shiftRight)
        {
            foreach (MatrixRow row in rows)
            {
                if (shiftRight)
                    row.Code >>= 1;
                else
                    row.Code <<= 1;
            }

            UpdateCode(currentType, false);
        }

        /// <summary>
        /// Shifts the matrix vertically
        /// </summary>
        public void ShiftVertically(bool shiftUpwards)
        {
            if (shiftUpwards)
            {
                // Iterate forwards
                for (int i = 0; i < rows.Length; i++)
                {
                    if (i + 1 == rows.Length)
                        rows[i].Code = 0;
                    else
                        rows[i].Code = rows[i + 1].Code;
                }
            }
            else
            {
                // Iterate backwards
                for (int i = rows.Length - 1; i >= 0; i--)
                {
                    if (i == 0)
                        rows[i].Code = 0;
                    else
                        rows[i].Code = rows[i - 1].Code;
                }
            }
        }

        /// <summary>
        /// Inverts all of the LEDs
        /// </summary>
        public void InvertAll()
        {
            foreach (MatrixRow row in rows)
            {
                foreach (LED led in row.LEDs)
                {
                    led.Enabled = !led.Enabled;
                }
            }

            UpdateCode(currentType, true);
        }

        /// <summary>
        /// Sets all of the LEDs to a state
        /// </summary>
        public void SetAll(bool state)
        {
            foreach (MatrixRow row in rows)
            {
                foreach (LED led in row.LEDs)
                {
                    led.Enabled = state;
                }
            }

            UpdateCode(currentType, true);
        }
    }
}
