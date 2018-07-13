using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace MatrixGraphicsGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Matrix matrix;
        Collection collection;
        CollectionEditWindow collectionEditWindow;

        public MainWindow()
        {
            InitializeComponent();

            matrix = new Matrix(new Tuple<int, int>(8, 8), MatrixGrid, CodeTextBox, CodeTypeButton, NameTextBox);
        }

        /// <summary>
        /// Turns on necessary collection buttons
        /// </summary>
        void SwitchToCollectionMode()
        {
            AddToCollectionButton.IsEnabled = true;
            NameTextBox.IsEnabled = true;
            EditCollectionButton.IsEnabled = true;
            ExportCollectionButton.IsEnabled = true;

            if (collectionEditWindow != null)
                collectionEditWindow.Close();
        }

        private void DisableAllButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.SetAll(false);
        }

        private void EnableAllButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.SetAll(true);
        }

        private void InvertButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.InvertAll();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CodeTextBox.Text);
        }

        private void NewCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON (*.json)|*.json";
            if (dialog.ShowDialog() == true)
            {
                collection = new Collection(dialog.FileName);
                SwitchToCollectionMode();
            }
        }

        private void OpenCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON (*.json)|*.json";
            if (dialog.ShowDialog() == true)
            {
                collection = new Collection(dialog.FileName);
                SwitchToCollectionMode();
            }
        }

        private void AddToCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            collection.AddGraphic(matrix.GetMatrixData());

            if (collectionEditWindow != null)
                collectionEditWindow.DisplayCollection(collection, matrix, MatrixGrid);
        }

        private void EditCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            collectionEditWindow = new CollectionEditWindow();
            collectionEditWindow.DisplayCollection(collection, matrix, MatrixGrid);
            collectionEditWindow.Owner = this;

            collectionEditWindow.Show();
        }

        private void ShiftLeftButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.ShiftHorizontally(false);
        }

        private void ShiftRightButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.ShiftHorizontally(true);
        }

        private void ShiftUpwardsButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.ShiftVertically(true);
        }

        private void ShiftDownwardsButton_Click(object sender, RoutedEventArgs e)
        {
            matrix.ShiftVertically(false);
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            ResizeWindow resizeWindow = new ResizeWindow(matrix);
            resizeWindow.Show();
        }

        private void ExportCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Arduino/C (*.c)|*.c";
            if (dialog.ShowDialog() == true)
            {
                collection.Export(dialog.FileName);
                SwitchToCollectionMode();
            }
        }

        private void TextChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (collection == null)
                return;

            bool graphicExists = false;

            collection.Graphics.ForEach(x =>
            {
                if (NameTextBox.Text == x.Name)
                {
                    graphicExists = true;
                    AddToCollectionButton.Content = "Overwrite";
                    return;
                }
            });

            if (AddToCollectionButton != null && !graphicExists)
                AddToCollectionButton.Content = "Add to Collection";
                       
        }
    }
}
