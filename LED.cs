using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MatrixGraphicsGenerator
{
    public class LED
    {
        Ellipse ellipse;
        bool enabled;

        public int RowIndex { get; private set; }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (enabled)
                    ellipse.Fill = new SolidColorBrush(Colors.Red);
                else
                    ellipse.Fill = new SolidColorBrush(Colors.White);
            }
        }

        public LED(MatrixBase matrixBase, Ellipse ledEllipse, int rowIndex)
        {
            ellipse = ledEllipse;
            RowIndex = rowIndex;
            Enabled = false;

            ellipse.MouseLeftButtonDown += (s, e) =>
            {
                Enabled = !Enabled;
                matrixBase.UpdateCode(RowIndex);
            };
        }
    }
}
