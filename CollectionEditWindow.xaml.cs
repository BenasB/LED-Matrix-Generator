using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatrixGraphicsGenerator
{
    /// <summary>
    /// Interaction logic for CollectionEditWindow.xaml
    /// </summary>
    public partial class CollectionEditWindow : Window
    {
        public CollectionEditWindow()
        {
            InitializeComponent();
        }

        public void DisplayCollection(Collection collection, Matrix matrix, Grid mainGrid)
        {
            CollectionGrid.RowDefinitions.Clear();
            CollectionGrid.Children.Clear();

            // Iterate through the list
            for (int row = 0; row < collection.Graphics.Count; row++)
            {
                MatrixData graphicData = collection.Graphics[row];

                RowDefinition rowDefinition = new RowDefinition();

                RowDefinition spacer = new RowDefinition();
                spacer.Height = new GridLength(5);

                Label nameLabel = new Label();
                nameLabel.Background = new SolidColorBrush(Colors.LightGray);
                nameLabel.MinWidth = 200;
                nameLabel.VerticalContentAlignment = VerticalAlignment.Center;
                nameLabel.Content = graphicData.Name;

                Button editButton = new Button();
                editButton.Content = "Edit";
                editButton.Width = 50;

                editButton.Click += (s, e) =>
                {
                    matrix.SetMatrix(graphicData);
                };

                Button removeButton = new Button();
                removeButton.Content = "Remove";
                removeButton.Width = 50;

                removeButton.Click += (s, e) =>
                {
                    collection.Graphics.Remove(graphicData);
                    collection.Save();

                    DisplayCollection(collection, matrix, mainGrid);
                };

                Grid.SetColumn(nameLabel, 0);
                Grid.SetRow(nameLabel, 2 * row);
                Grid.SetColumn(editButton, 1);
                Grid.SetRow(editButton, 2 * row);
                Grid.SetColumn(removeButton, 2);
                Grid.SetRow(removeButton, 2 * row);

                CollectionGrid.RowDefinitions.Add(rowDefinition);
                if (row != collection.Graphics.Count - 1)
                    CollectionGrid.RowDefinitions.Add(spacer);

                CollectionGrid.Children.Add(nameLabel);
                CollectionGrid.Children.Add(editButton);
                CollectionGrid.Children.Add(removeButton);
            }
        }
    }
}
